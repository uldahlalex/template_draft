using ApiHelpers.ApiHelpers;
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
            .AddSingleton<HardcodedValues>()
            .AddSingleton<CredentialService>()

            //Exclusively from API Helpers
            .AddSingleton<TokenService>()
            .AddSingleton<EndpointHelpers>()
            .AddSingleton<EndpointHelperFacade>();

            //Exclusively from Bootstrapping Helpers: Documentation Helpers
            .AddSingleton<SwaggerDefinition>()
            .AddHostedService<SwaggerJsonGeneratorService>()
            
            //Dependent on at least one service in previous layer
            //Exclusively from DB Helpers
            .AddSingleton<BuildDbContainer>()
            .AddSingleton<DbScripts>()


        // if (Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
        //     .Equals(HardcodedValues.Environments.Testing))
        //     builder.WebHost.UseUrls("http://localhost:9999");

        var app = builder.Build();


        return app;
    }
}