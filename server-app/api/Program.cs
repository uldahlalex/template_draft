using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using api.Setup;
using ApiHelperServics;
using Carter;
using Dapper;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();

        #region Build Database and seed for normal runs not in production

        if (!Environment.GetEnvironmentVariable(app.Services.GetService<KeyNamesService>()!.ASPNETCORE_ENVIRONMENT)!
                .Equals(app.Services.GetService<ValuesService>()!.Production))
        {
            await BuildDbContainer.StartDbInContainer(Environment.GetEnvironmentVariable(
                app.Services.GetService<KeyNamesService>()!.PG_CONN)!);
            var conn = app.Services.GetRequiredService<NpgsqlDataSource>().OpenConnection();
            var schema = File.ReadAllText("../../scripts/PostgresSchema.sql");
            var seed = File.ReadAllText("../../scripts/SeedDb.sql");
            conn.Execute(schema);
            conn.Execute(seed);
            conn.Close();
        }

        #endregion

        app.Run();
    }

    public static async Task<WebApplication> Startup()
    {
        var keyNames = new KeyNamesService();
        var values = new ValuesService();
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(keyNames.PG_CONN)))
            Environment.SetEnvironmentVariable(keyNames.PG_CONN, values.LOCAL_POSTGRES);

        Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
                          JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));

        var builder = WebApplication.CreateBuilder();
        builder.Services

            #region Independent business logic

            .AddSingleton(keyNames)
            .AddSingleton(values)
            .AddSingleton<SecurityService>()
            .AddSingleton<ApiHelperFacade>()

            #endregion

            #region Nuget pacakes: Totally indepdendent

            .AddProblemDetails()
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            })

            #endregion

            #region Nuget pacakes: Dependent on Independent business logic

            .AddNpgsqlDataSource(Environment.GetEnvironmentVariable(keyNames.PG_CONN)!,
                cfg => cfg.EnableParameterLogging())

            #endregion

            #region Logic depdenent on previous builder layers

            .AddHostedService<SwaggerJsonGeneratorService>();
        if (Environment.GetEnvironmentVariable(keyNames.ASPNETCORE_ENVIRONMENT)!.Equals(values.Testing))
            builder.WebHost.UseUrls("http://localhost:9999");

        #endregion


        var app = builder.Build();


        #region Exception handling and response customization middleware

        app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (AuthenticationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    throw;
                }
                catch (ValidationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    throw;
                }
                catch (System.ComponentModel.DataAnnotations.ValidationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    throw;
                }
                catch (Exception)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    throw;
                }
            }).UseStatusCodePages(async statusCodeContext =>
                await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                    .ExecuteAsync(statusCodeContext.HttpContext))

            #endregion

            #region Documentation middleware

            .UseSwagger()
            .UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); })

            #endregion

            #region API logic middleware

            .UseCors(options =>
            {
                options.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        app.MapCarter();

        #endregion


        return app;
    }
}