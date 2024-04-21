namespace IndependentHelpers;

/// <summary>
/// Strongly typed string constants
/// </summary>
public static class Constants
{
    public const string JWT_KEY =
        "hdsfkyudsfksahfkdsahfffukdsafhkdsaufhidsafhkdsahfkdsahfiudsahfkdsahfkudsahfkudsahfkudsahfkudsahfkdsahfkuds";

    public const string LOCAL_POSTGRES =
        "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true;MaxPoolSize=5;";


    public const string Production = nameof(Production);
    public const string Testing = nameof(Testing);
    public const string Development = nameof(Development);
}