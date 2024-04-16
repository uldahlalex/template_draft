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
        services.AddSingleton<BuildDbContainer>();
        services.AddSingleton<DbScripts>();
        services.AddSingleton<SwaggerDefinition>();
        services.AddSingleton<SwaggerJsonGeneratorService>();
        services.AddSingleton<BootstrappingFacade>();
        services.BuildServiceProvider().GetService<BootstrappingFacade>();
        return services;
    }
}

public class UseBootstrappingFacade(BootstrappingFacade bootstrappingFacade, )
{
    public void Init()
    {
        if (!Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT)
                .Equals(HardcodedValues.Environments.Production))
        {
            app.Services.GetService<DbScripts>()!.RebuildDbSchema();
            app.Services.GetService<DbScripts>()!.SeedDB();
        }
    }
}