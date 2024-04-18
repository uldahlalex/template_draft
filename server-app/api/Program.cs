using System.Net.Http.Headers;
using System.Text.Json;
using api.Boilerplate;
using api.Boilerplate.DbHelpers;
using Carter;
using Dapper;
using FluentValidation;
using IndependentHelpers.InjectableServices;
using JWT.Builder;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();
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
            .AddProblemDetails()
            .AddNpgsqlDataSource(Environment.GetEnvironmentVariable(keyNames.PG_CONN)!, cfg => cfg.EnableParameterLogging())
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
        }).AddHostedService<SwaggerJsonGeneratorService>();

        if (Environment.GetEnvironmentVariable(keyNames.ASPNETCORE_ENVIRONMENT)!.Equals(values.Testing))
            builder.WebHost.UseUrls("http://localhost:9999");
    
        var app = builder.Build();


        app.Use(async (context, next) =>        //Making custom responses upon exceptions
        {
            try
            {
                await next();
            }
            catch (System.Security.Authentication.AuthenticationException)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                throw;
            }
            catch (ValidationException)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                throw;
            }
            catch (System.ComponentModel.DataAnnotations.ValidationException)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                throw;
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                throw;
            }
        }).UseStatusCodePages(async statusCodeContext =>
                await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                    .ExecuteAsync(statusCodeContext.HttpContext))
            .UseSwagger()
            .UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); })
            .UseCors(options =>
            {
                options.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            app.MapCarter();

        return app;
    }
}