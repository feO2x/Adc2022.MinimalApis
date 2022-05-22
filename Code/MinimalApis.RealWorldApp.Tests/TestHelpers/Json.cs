using System.Text.Json;

namespace MinimalApis.RealWorldApp.Tests.TestHelpers;

public static class Json
{
    private static JsonSerializerOptions Options { get; } =
        new ()
        {
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    public static string SerializeToJson<T>(this T value) =>
        JsonSerializer.Serialize(value, Options);
}