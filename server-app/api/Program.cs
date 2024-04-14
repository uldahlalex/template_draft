using System.Text.Json;
using api.BootstrappingHelpers.DbHelpers;
using api.BootstrappingHelpers.Documentation;
using api.EndpointHelpers.Security;
using api.Independent.GlobalValues;
using Carter;
using FluentValidation;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();
        if (!Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT).Equals(HardcodedValues.Environments.Production))
        {
            app.Services.GetService<DbScripts>()!.RebuildDbSchema();
            app.Services.GetService<DbScripts>()!.SeedDB();
        }

        app.Run();
    }

    public static async Task<WebApplication> Startup()
    {        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)))
             {
                 Console.WriteLine("No JWT_KEY found in environment variables. Setting default harcoded value.");
                 Environment.SetEnvironmentVariable(KeyNames.JWT_KEY, HardcodedValues.JWT_KEY);
             }
        
        Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
                          JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));
        
        if (!Environment.GetEnvironmentVariable(KeyNames.SKIP_DB_CONTAINER_BUILDING).Equals("true"))
            await BuildDbContainer.StartDbInContainer(Environment.GetEnvironmentVariable(KeyNames.PG_CONN) ??
                                                      HardcodedValues.LOCAL_POSTGRES);
        
        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddProblemDetails()
            .AddNpgsqlDataSource(HardcodedValues.LOCAL_POSTGRES, cfg => cfg.EnableParameterLogging())
            .AddSingleton<DbScripts>()
            .AddSingleton<CredentialService>()
            .AddSingleton<TokenService>()
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer()
            .AddSwaggerDefinition();
        builder.Services.AddHostedService<SwaggerJsonGeneratorService>();

        if (Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT).Equals(HardcodedValues.Environments.Testing))
            builder.WebHost.UseUrls("http://localhost:9999");
    
        var app = builder.Build();

        //todo er dette i stedet for default exc handler?
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

           app .UseSwagger()
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