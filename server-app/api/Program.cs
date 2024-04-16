using api.DependentHelpers.BootstrappingHelpers;
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
            .AddProblemDetails()
            .AddUtilitiesFacade()
            .AddBootstrappingFacade()
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer();
       
        // if (Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
        //     .Equals(HardcodedValues.Environments.Testing))
        //     builder.WebHost.UseUrls("http://localhost:9999");

        var app = builder.Build();
        return app;
    }
}