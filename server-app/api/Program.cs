using System.Text.Json;
using api.Boilerplate;
using api.Boilerplate.DbHelpers;
using api.Boilerplate.ReusableHelpers.GlobalValues;
using api.Boilerplate.ReusableHelpers.Security;
using Carter;
using FluentValidation;
using Microsoft.OpenApi.Models;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();
        if (!Env.ASPNETCORE_ENVIRONMENT.Equals(StringConstants.Environments.Production))
        {
            app.Services.GetService<DbScripts>()!.RebuildDbSchema();
            app.Services.GetService<DbScripts>()!.SeedDB();
        }

        app.Run();
    }

    public static async Task<WebApplication> Startup()
    {
        Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
                          JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));
        if (!Env.ASPNETCORE_ENVIRONMENT.Equals(StringConstants.Environments.Production) &&
            !Env.SKIP_DB_CONTAINER_BUILDING.Equals("true"))
            await BuildDbContainer.StartDbInContainer();

        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddProblemDetails()
            .AddNpgsqlDataSource(Env.PG_CONN, cfg => cfg.EnableParameterLogging())
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer()
        .AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = StringConstants.JwtConstants.Authorization,
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
        });
        builder.Services.AddHostedService<SwaggerJsonGeneratorService>();

        if (Env.ASPNETCORE_ENVIRONMENT.Equals(StringConstants.Environments.Testing))
            builder.WebHost.UseUrls("http://localhost:9999");
    
        var app = builder.Build();

        app.Use(async (context, next) =>
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
        });


        app.UseStatusCodePages(async statusCodeContext =>
                await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                    .ExecuteAsync(statusCodeContext.HttpContext));

           app 
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