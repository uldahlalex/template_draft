namespace api.Boilerplate.ReusableHelpers.GlobalValues;

/// <summary>
///     I prefer strongly typed values, so I don't hardcode strings
/// </summary>
public static class StringConstants
{
    public static class Environments
    {
        public static string Production = nameof(Production);
        public static string Development = nameof(Development);
        public static string Testing = nameof(Testing);
    }

    public static class JwtConstants
    {
        public static string Authorization = nameof(Authorization);
        public static string Payload = nameof(Payload);
    }
}