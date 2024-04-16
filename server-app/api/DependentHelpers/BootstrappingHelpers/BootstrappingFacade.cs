using api.DependentHelpers.BootstrappingHelpers.DbHelper;
using api.DependentHelpers.BootstrappingHelpers.Documentatio;

namespace api.DependentHelpers.BootstrappingHelpers;

public class BootstrappingFacade(BuildDbContainer buildDbContainer, DbScripts dbScripts, SwaggerDefinition swaggerDefinition, SwaggerJsonGeneratorService swaggerJsonGeneratorService)
{
    public BuildDbContainer BuildDbContainer { get; } = buildDbContainer;
    public DbScripts DbScripts { get; } = dbScripts;
    public SwaggerDefinition SwaggerDefinition { get; } = swaggerDefinition;
    public SwaggerJsonGeneratorService SwaggerJsonGeneratorService { get; } = swaggerJsonGeneratorService;
}

public static class BootstrappingFacadeExtensions
{
    public static IServiceCollection AddBootstrappingFacade(this IServiceCollection services)
    {
        //todo lifetime adjustment
        //todo pg conn
        services.AddNpgsqlDataSource("HardcodedValues.LOCAL_POSTGRES", cfg => cfg.EnableParameterLogging());

        services.AddSingleton<BuildDbContainer>();
        services.AddSingleton<DbScripts>();
        services.AddSingleton<SwaggerDefinition>();
        services.AddSingleton<BootstrappingFacade>();
        services.AddHostedService<SwaggerJsonGeneratorService>(); //todo hosted service diff
        

        return services;
    }
}

public class UseBootstrappingFacade(BootstrappingFacade bootstrappingFacade, UtilitiesFacade utilitiesFacade)
{
    public void Init()
    {
        
        //Evaluating environment
        // if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)))
        // {
        //     Console.WriteLine("No JWT_KEY found in environment variables. Setting default harcoded value.");
        //     Environment.SetEnvironmentVariable(KeyNames.JWT_KEY, HardcodedValues.JWT_KEY);
        // }
        //
        // Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
        //                   JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));
        
        //Starting DB in container
        // var skip = Environment.GetEnvironmentVariable(KeyNames.SKIP_DB_CONTAINER_BUILDING) ?? "false";
        // if (!skip.Equals("true"))
        //     await app.Services.GetService<BuildDbContainer>()
        //         .StartDbInContainer(Environment.GetEnvironmentVariable(KeyNames.PG_CONN) ??
        //                             HardcodedValues.LOCAL_POSTGRES);
        
        //Rebuilding DB
        // if (!Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
        //         .Equals(HardcodedValues.Environments.Production))
        // {
        //     app.Services.GetService<DbScripts>()!.RebuildDbSchema();
        //     app.Services.GetService<DbScripts>()!.SeedDB();
        // }
    }
}