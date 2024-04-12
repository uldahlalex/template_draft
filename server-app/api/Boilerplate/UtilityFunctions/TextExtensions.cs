using System.Text.Json;

namespace api.Boilerplate.UtilityFunctions;

public static class TextExtensions
{
    public static T Deserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidCastException("Could not turn JSON string " + json + " into object of type " +
                                             typeof(T).Name);
    }

    public static string Serialize<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        });
    }
}