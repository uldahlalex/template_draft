namespace api.Independent.GlobalValues;

/// <summary>
///     I prefer strongly typed values, so I don't hardcode strings
/// </summary>
public static class KeyNames
{

    public static string JWT_KEY = nameof(JWT_KEY);
    public static string ASPNETCORE_ENVIRONMENT = nameof(ASPNETCORE_ENVIRONMENT);
    public static string SKIP_DB_CONTAINER_BUILDING = nameof(SKIP_DB_CONTAINER_BUILDING);
    public static string PG_CONN = nameof(PG_CONN);
}