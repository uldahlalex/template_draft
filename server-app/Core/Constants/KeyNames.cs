namespace Core;

/// <summary>
///     I prefer strongly typed values, so I don't hardcode strings, I look up keys by variable. If the values are
///     "typical" they can be found under HardcodedValues.cs
/// </summary>
public  class KeyNames
{
    public  string JWT_KEY = nameof(JWT_KEY);
    public  string ASPNETCORE_ENVIRONMENT = nameof(ASPNETCORE_ENVIRONMENT);
    public  string SKIP_DB_CONTAINER_BUILDING = nameof(SKIP_DB_CONTAINER_BUILDING);
    public  string PG_CONN = nameof(PG_CONN);
}