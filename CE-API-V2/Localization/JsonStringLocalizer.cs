using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace CE_API_V2.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly JsonSerializer _serializer = new JsonSerializer();
        public JsonStringLocalizer()
        {
        }
        public LocalizedString this[string name]
        {
            get
            {
                string value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            //notneeded
            return new List<LocalizedString>();
        }
        private string GetString(string key)
        {
            var category = key.Split(".")[0];
            var actualKey = key.Split(".")[1];
            string relativeFilePath = $"Resources/{category}_{CultureInfo.CurrentUICulture}.json";
            string fullFilePath = Path.GetFullPath(relativeFilePath);

            string relativeDefaultFilePath = $"Resources/{category}_en-GB.json";
            string fullDefaultFilePath = Path.GetFullPath(relativeDefaultFilePath);

            if (File.Exists(fullFilePath))
            {
                string result = GetValueFromJSON(actualKey, fullFilePath);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }else if(File.Exists(fullDefaultFilePath))
            {
                string result = GetValueFromJSON(actualKey, fullDefaultFilePath);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            return default;
        }
        private string GetValueFromJSON(string propertyName, string filePath)
        {
            if (propertyName == null) return default;
            if (filePath == null) return default;
            using (var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sReader = new StreamReader(str))
            using (var reader = new JsonTextReader(sReader))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == propertyName)
                    {
                        reader.Read();
                        return _serializer.Deserialize<string>(reader);
                    }
                }
                return default;
            }
        }
    }
}
