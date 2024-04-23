using System.Reflection;
using Dapper;
using Npgsql;

namespace api.Setup;

public static class DbScriptExecuter
{
    public static async Task<string> LoadSqlScriptAsync(string resourceName)
    {

        var assembly = Assembly.GetAssembly(typeof(api.Setup.DbScriptExecuter)); // Adjust if needed

        var resourcePath = $"api.{resourceName}";
        if (!ResourceExists(assembly, resourcePath))
            throw new FileNotFoundException($"Resource '{resourcePath}' not found. Check the namespace and resource name.");

        using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
        using (StreamReader reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }

    private static bool ResourceExists(Assembly assembly, string resourceName)
    {
        return Array.IndexOf(assembly.GetManifestResourceNames(), resourceName) >= 0;
    }

    public static async Task ExecuteScript(string scriptFile, NpgsqlDataSource ds)
    {
        using  (var conn = ds.OpenConnection())
        {
            Console.WriteLine("Executing script: " + scriptFile);
            var sql = await LoadSqlScriptAsync(scriptFile);
            conn.Execute(sql);
        }
    }
}