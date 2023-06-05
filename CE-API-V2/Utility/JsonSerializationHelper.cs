using System.Text.Json;

namespace CE_API_V2.Utility
{
    public static class JsonSerializationHelper
    {
        public static T? DeserializeObject<T>(string jsonString, JsonSerializerOptions? options = null)
        {
            options ??= new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                return JsonSerializer.Deserialize<T>(jsonString, options);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}
