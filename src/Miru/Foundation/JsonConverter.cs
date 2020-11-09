using System.Text.Json;

namespace Miru.Foundation
{
    public class JsonConverter : IJsonConverter
    {
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public string Serialize<T>(T @object)
        {
            return JsonSerializer.Serialize<T>(@object);
        }
    }
}