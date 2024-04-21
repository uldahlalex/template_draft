using System.Text.Json;
using Dapper;
using Npgsql;
using IndependentHelpers;

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
            Environment.SetEnvironmentVariable(EnvVarNames.PG_CONN, Constants.LOCAL_POSTGRES);
        Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
                          JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddAwesomeServices(new ServiceConfiguration()
        {
            
        });

        var app = builder.Build();
        app.UseAwesomeServices(new AppConfiguration()
        {
            
        });


        return Task.FromResult(app);
    }
}