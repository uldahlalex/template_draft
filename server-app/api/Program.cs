using ApiHelperServics;
using BootstrappingHelpers.BuilderPhaseHelpers;
using BootstrappingHelpers.StaticInvokable;
using BootstrappingHelpers.WebAppPhaseHelpers.Middleware;
using Carter;
using Dapper;
using IndependentHelpers.InjectableServices;
using Npgsql;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();

        SwaggerJsonGenerator.GenerateJsonFromSwaggerDefinition(app.Services);
        var keys = app.Services.GetRequiredService<KeyNamesService>();
        var values = app.Services.GetRequiredService<ValuesService>();
        if (!Environment.GetEnvironmentVariable(keys.ASPNETCORE_ENVIRONMENT).Equals(values.Production))
        {
            await BuildDbContainer.StartPostgresInContainer(keys.PG_CONN);
            var conn = app.Services.GetRequiredService<NpgsqlDataSource>();
            var open = conn.OpenConnection();
            var schema = File.ReadAllText("../../scripts/PostgresSchema.sql");
            var seed = File.ReadAllText("../../scripts/SeedDb.sql");
            open.Execute(schema);
            open.Execute(seed);
            open.Close();
        }

        app.Run();
    }

    public static async Task<WebApplication> Startup()
    {
        var builder = WebApplication.CreateBuilder();
        var keyNames = new KeyNamesService();
        var values = new ValuesService();
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(keyNames.PG_CONN)))
            Environment.SetEnvironmentVariable(keyNames.PG_CONN, values.LOCAL_POSTGRES);
        builder.Services
            .AddSingleton<KeyNamesService>(keyNames)
            .AddSingleton<ValuesService>(values)
            .AddNpgsqlDataSource(Environment.GetEnvironmentVariable(keyNames.PG_CONN)!,
                cf => cf.EnableParameterLogging())
            .AddProblemDetails()
            .AddCarter()
            .AddCors()
            .AddEndpointsApiExplorer()
            //Exclusively from API Helpers
            .AddSingleton<SecurityService>()
            .AddSingleton<ApiHelperFacade>() //The class to contain the other API helpers //todo refactor?
            .AddSwaggerDefinition(); // from BuilderPhaseHelpers

        if (Environment.GetEnvironmentVariable(keyNames.ASPNETCORE_ENVIRONMENT).Equals(values.Testing))
            builder.WebHost.UseUrls("http://localhost:9999");

        var app = builder.Build();

        app.UseCustomCors()
            .UseCustomExceptionHandler()
            .UseCustomStatusCodePages()
            .UseCustomSwaggerDefinition()
            .MapCarter();

        return app;
    }
}