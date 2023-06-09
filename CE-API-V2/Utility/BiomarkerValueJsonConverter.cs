using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using CE_API_V2.Models.DTO;
namespace CE_API_V2.Utility;

public class BiomarkerValueJsonConverter<T> : JsonConverter<BiomarkerValueDto<T>> where T : struct, IConvertible 
{
    public override BiomarkerValueDto<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                data = data.Replace(descriptionName.ToLower(), $"\"{property.Name.ToLower()}\"");
            }
        }
        
        var result = JsonSerializer.Deserialize<BiomarkerValueDto<T>>(data, myOptions);
        return result;
    }
    public override void Write(Utf8JsonWriter writer, BiomarkerValueDto<T> valueDto, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, valueDto, options);
    }
}