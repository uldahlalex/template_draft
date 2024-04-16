using ApiHelpers.ApiHelpers;
using BootstrappingHelpers.BootstrappingHelpers.DbHelper;
using BootstrappingHelpers.BootstrappingHelpers.Documentation;
using Carter;

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
        var builder = WebApplication.CreateBuilder();


        builder.Services
            //Only dependent on nuget packages and nothing else in my system
            .AddNpgsqlDataSource("HardcodedValues.LOCAL_POSTGRES", cfg => cfg.EnableParameterLogging())
            .AddProblemDetails()
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer()

            //Exclusively from Independent Helpers
            .AddSingleton<KeyNames>()
            .AddSingleton<Values>()

            //Exclusively from API Helpers
            .AddSingleton<Security>()
            
            
            .AddSingleton<ApiHelperFacade>() //The class to contain the other API helpers

            //Exclusively from Bootstrapping Helpers: Documentation Helpers
            .AddSwaggerDefinition()
            .AddHostedService<SwaggerJsonGeneratorService>() //todo Check hosted service
            
            //Exclusively from DB Helpers
            .AddSingleton<BuildDbContainer>()
            .AddSingleton<DbScripts>(); //Requires NpgsqlDataSource


        // if (Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
        //     .Equals(HardcodedValues.Environments.Testing))
        //     builder.WebHost.UseUrls("http://localhost:9999");

        var app = builder.Build();

        
        return app;
    }
}