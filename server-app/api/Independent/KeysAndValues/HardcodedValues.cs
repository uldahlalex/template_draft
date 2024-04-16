namespace api.Independent.KeysAndValues;

public static class HardcodedValues
{
    public static string LOCAL_POSTGRES =
        "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Pooling=true;MaxPoolSize=5;";


    public static string JWT_KEY =
        "hdsfkyudsfksahfkdsahfffukdsafhkdsaufhidsafhkdsahfkdsahfiudsahfkdsahfkudsahfkudsahfkudsahfkudsahfkdsahfkuds";

    public static class Environments
    {
        public static string Production = nameof(Production);
        public static string Development = nameof(Development);
        public static string Testing = nameof(Testing);
    }
}