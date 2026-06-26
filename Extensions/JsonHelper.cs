using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

namespace StarQ.Shared.Extensions
{
    public class Float2Converter : JsonConverter<float2>
    {
        public override void WriteJson(JsonWriter writer, float2 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteEndArray();
        }

        public override float2 ReadJson(
            JsonReader reader,
            Type objectType,
            float2 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            JToken token = JToken.Load(reader);

            if (token.Type == JTokenType.Array)
            {
                var arr = token.ToObject<float[]>();
                return arr?.Length >= 2 ? new float2(arr[0], arr[1]) : float2.zero;
            }

            if (token.Type == JTokenType.Object)
            {
                return new float2(token["x"]?.Value<float>() ?? 0, token["y"]?.Value<float>() ?? 0);
            }

            return float2.zero;
        }
    }
}
