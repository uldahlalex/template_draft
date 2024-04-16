using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using api.DependentHelpers.BootstrappingHelpers;
using api.Independent.KeysAndValues;
using Carter;
using EndpointHelpers.Security;
using FluentValidation;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();
        

        app.Run();
    }

    public static async Task<WebApplication> Startup()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)))
        {
            Console.WriteLine("No JWT_KEY found in environment variables. Setting default harcoded value.");
            Environment.SetEnvironmentVariable(KeyNames.JWT_KEY, HardcodedValues.JWT_KEY);
        }

        Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
                          JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));



        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddProblemDetails()
            .AddNpgsqlDataSource(HardcodedValues.LOCAL_POSTGRES, cfg => cfg.EnableParameterLogging())
            .AddSingleton<CredentialService>()
            .AddSingleton<TokenService>()
            .AddSingleton<UtilitiesFacade>()
            .AddBootstrappingFacade()
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer();
        builder.Services.AddHostedService<SwaggerJsonGeneratorService>();
       
        if (Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
            .Equals(HardcodedValues.Environments.Testing))
            builder.WebHost.UseUrls("http://localhost:9999");

        var app = builder.Build();

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
        });


        app.UseStatusCodePages(async statusCodeContext =>
            await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                .ExecuteAsync(statusCodeContext.HttpContext));

        app.UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); })
            .UseCors(options =>
            {
                options.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        app.MapCarter();
        var skip = Environment.GetEnvironmentVariable(KeyNames.SKIP_DB_CONTAINER_BUILDING) ?? "false";
        if (!skip.Equals("true"))
            await app.Services.GetService<BuildDbContainer>()
                .StartDbInContainer(Environment.GetEnvironmentVariable(KeyNames.PG_CONN) ??
                                                      HardcodedValues.LOCAL_POSTGRES);
        return app;
    }
}