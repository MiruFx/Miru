using System.Text.Json;

namespace Miru.Foundation
{
    public class JsonConverter : IJsonConverter
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions= new()
        {
            IncludeFields = true,
        };

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }

        public string Serialize<T>(T @object)
        {
            return JsonSerializer.Serialize<T>(@object, JsonSerializerOptions);
        }
    }
}