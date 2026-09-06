using System.Text.Json.Serialization;

namespace LocaleManagement
{
    [JsonSerializable(typeof(Dictionary<string, string>))]
    internal partial class AppJsonContext : JsonSerializerContext { }
}
