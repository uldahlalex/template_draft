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
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer()
            .IndependentHelpers()
            .AddBootstrappingFacade()
            .AddDependentHelpersFacade();
     
       
        // if (Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
        //     .Equals(HardcodedValues.Environments.Testing))
        //     builder.WebHost.UseUrls("http://localhost:9999");

        var app = builder.Build();
        app.Services.GetService<BootstrappingFacade>()!.ApplicationPhaseInit();
        return app;
    }
}