using System.Reflection;
using System.Text.Json;
using api.Setup;
using Dapper;
using IndependentHelpers;
using Npgsql;

namespace api;

public static class Program
{
    public static async Task Main()
    {
        var app = await Startup();

        if (!Environment.GetEnvironmentVariable(EnvVarNames.ASPNETCORE_ENVIRONMENT)!.Equals(Constants.Production))
        {
            var ds = app.Services.GetRequiredService<NpgsqlDataSource>();
            int attempts = 0;
            while (attempts < 100)
            {
                try
                {
                    var assembly = Assembly.GetAssembly(typeof(api.Setup.DbScriptExecuter));
                    foreach (var resourceName in assembly.GetManifestResourceNames())
                    {
                        Console.WriteLine("RESOURCE NAME: "+resourceName);
                    }
                    await DbScriptExecuter.ExecuteScript("scripts.PostgresSchema.sql", ds);
                    await DbScriptExecuter.ExecuteScript("scripts.SeedDb.sql", ds);
                    break;
                }
                catch (NpgsqlException)
                {
                    attempts++;
                    Task.Delay(100).Wait();
                    if (attempts == 100)
                        throw new InvalidOperationException("Could not connect to database.");
                }
            }
        }

        app.Run();
    }
 public static Task<WebApplication> Startup()
    {
        var builder = WebApplication.CreateBuilder();
        
        // Configure logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        SetupEnvironmentVariables();

        // Log the environment for debugging purposes
        Console.WriteLine("BUILDING API WITH ENVIRONMENT: "+
            JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));

        // Add services to the application
        AddAwesomeServices(builder);

        var app = builder.Build();

        // Use the configured services
        app.UseAwesomeServices(new AppConfiguration()
        {
            SkipDbContainer = Environment.GetEnvironmentVariable(EnvVarNames.ASPNETCORE_ENVIRONMENT)!.Equals(Constants.Development)
        });
        
        Console.WriteLine("Application has been successfully built and configured.");

        return Task.FromResult(app);
    }

    private static void SetupEnvironmentVariables()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvVarNames.PG_CONN)))
        {
            Environment.SetEnvironmentVariable(EnvVarNames.PG_CONN, Constants.DEFAULT_LOCAL_POSTGRES);
            Console.WriteLine($"Environment variable '{EnvVarNames.PG_CONN}' set to default: {Constants.DEFAULT_LOCAL_POSTGRES}");
        }
        
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)))
        {
            Environment.SetEnvironmentVariable(EnvVarNames.JWT_KEY, Constants.DEFAULT_JWT_KEY);
            Console.WriteLine($"Environment variable '{EnvVarNames.JWT_KEY}' set to default: {Constants.DEFAULT_JWT_KEY}");
        }
    }

    private static void AddAwesomeServices(WebApplicationBuilder builder)
    {
        try
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{currentAssembly.GetName().Name}.xml");
            builder.Services.AddAwesomeServices(new ServiceConfiguration
                {
                    // Configuration details here
                    
                },
                new List<Assembly> { currentAssembly }, // Assemblies containing modules
                new List<string> { xmlPath }); // Paths to XML documentation files

            Console.WriteLine("Services have been successfully added.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}