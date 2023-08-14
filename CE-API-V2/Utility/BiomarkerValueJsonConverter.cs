using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Utility;

public class BiomarkerValueJsonConverter<T> : JsonConverter<BiomarkerValue<T>> where T : struct, IConvertible 
{
    public override BiomarkerValue<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var myOptions = new JsonSerializerOptions(options);
        myOptions.PropertyNameCaseInsensitive = true;
        myOptions.Converters.Add(new JsonStringEnumConverter());
        var data = jsonDoc.RootElement.ToString();
        var test = typeof(T);
        foreach (var property in test.GetFields())
        {
            var descriptionName = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DescriptionAttribute))?.ConstructorArguments.FirstOrDefault().ToString();
            if (!string.IsNullOrEmpty(descriptionName))
            {
                data = data.Replace(descriptionName, $"\"{property.Name}\"");
            }
        }
        
        var result = JsonSerializer.Deserialize<BiomarkerValue<T>>(data, myOptions);
        return result;
    }
    public override void Write(Utf8JsonWriter writer, BiomarkerValue<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}