using System.Reflection;
using System.Text.Json;
using Dapper;
using Npgsql;

namespace api;

public static class Program
{
    public static async Task Main()
    {
        var app = await Startup();

        if (!Environment.GetEnvironmentVariable(EnvVarNames.ASPNETCORE_ENVIRONMENT)!.Equals(Constants.Production))
        {
            var conn = app.Services.GetRequiredService<NpgsqlDataSource>().OpenConnection();
            var schema = File.ReadAllText("../../scripts/PostgresSchema.sql");
            var seed = File.ReadAllText("../../scripts/SeedDb.sql");
            conn.Execute(schema);
            conn.Execute(seed);
            conn.Close();
        }

        app.Run();
    }

    public static Task<WebApplication> Startup()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvVarNames.PG_CONN)))
            Environment.SetEnvironmentVariable(EnvVarNames.PG_CONN, Constants.DEFAULT_LOCAL_POSTGRES);
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)))
            Environment.SetEnvironmentVariable(EnvVarNames.JWT_KEY, Constants.DEFAULT_JWT_KEY);

        Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
                          JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));

        var builder = WebApplication.CreateBuilder();
        var currentAssembly = Assembly.GetExecutingAssembly();
        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{currentAssembly.GetName().Name}.xml");
        builder.Services.AddAwesomeServices(new ServiceConfiguration()
            {
                //DefaultDockerSocketPath = 
            },
            new List<Assembly> { currentAssembly }, // Assemblies containing Carter modules
            new List<string> { xmlPath }); // Paths to XML documentation files);

        var app = builder.Build();

        app.UseAwesomeServices(new AppConfiguration()
        {
        });

        return Task.FromResult(app);
    }
}