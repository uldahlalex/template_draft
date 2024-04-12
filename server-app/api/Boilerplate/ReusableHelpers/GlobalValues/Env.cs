namespace api.Boilerplate.ReusableHelpers.GlobalValues;

public static class Env
{
    public static string PG_CONN = Environment.GetEnvironmentVariable(nameof(PG_CONN)) ??
                                   "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true;MaxPoolSize=5;";

    public static string SKIP_DB_CONTAINER_BUILDING =
        Environment.GetEnvironmentVariable(nameof(SKIP_DB_CONTAINER_BUILDING)) ?? "false";

    public static string ASPNETCORE_ENVIRONMENT =
        Environment.GetEnvironmentVariable(nameof(ASPNETCORE_ENVIRONMENT)) ?? "Development";

    public static string JWT_KEY = Environment.GetEnvironmentVariable(nameof(JWT_KEY)) ??
                                   "hdsfkyudsfksahfkdsahfffukdsafhkdsaufhidsafhkdsahfkdsahfiudsahfkdsahfkudsahfkudsahfkudsahfkudsahfkdsahfkuds";
}