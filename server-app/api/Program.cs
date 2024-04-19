using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using api.Setup;
using ApiHelperServics;
using Carter;
using Dapper;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace api;

public class Program
{
    public static async Task Main()
    {
        var app = await Startup();

        if (!Environment.GetEnvironmentVariable(app.Services.GetService<KeyNamesService>()!.ASPNETCORE_ENVIRONMENT)!
                .Equals(app.Services.GetService<ValuesService>()!.Production))
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

    public static async Task<WebApplication> Startup()
    {
        // var keyNames = new KeyNamesService();
        // var values = new ValuesService();
        // if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(keyNames.PG_CONN)))
        //     Environment.SetEnvironmentVariable(keyNames.PG_CONN, values.LOCAL_POSTGRES);
        //
        // Console.WriteLine("BUILDING API WITH ENVIRONMENT: +" +
        //                   JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));

        var builder = WebApplication.CreateBuilder();

        builder.Services.AddAwesomeServices(new ServiceConfiguration());

        var app = builder.Build();

        app.UseAwesomeServices(new AppConfiguration());
    

        return app;
    }
}