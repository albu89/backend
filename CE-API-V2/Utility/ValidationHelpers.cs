using System.Text.Json.Serialization;
using CE_API_V2.Models.DTO;
namespace CE_API_V2.Utility;

public static class ValidationHelpers
{
    public static string? GetJsonPropertyKeyName(string propertyName, Type typeToCheck)
    {
        var props = typeToCheck.GetProperties();
        var prop = props.FirstOrDefault(x => x.Name == propertyName);
        var customAttribute = prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute;
        return customAttribute?.Name;
    }
    private static BiomarkerSchemaUnitDto? GetCorrespondingUnit(IEnumerable<BiomarkerSchemaDto> template, string requestUnitType, string propertyName)
    {

        if (requestUnitType == null)
            throw new ArgumentNullException(nameof(requestUnitType));
        var unitToTest = template.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequestDto)))?
            .Units?.FirstOrDefault(x => x.UnitType == requestUnitType);
        return unitToTest;
    }
    public static float GetMaxValue(string propertyName, string unitType, IEnumerable<BiomarkerSchemaDto> template)
    {
        return GetCorrespondingUnit(template, unitType, propertyName)?.Maximum ?? 0;
    }
    public static float GetMinValue(string propertyName, string unitType, IEnumerable<BiomarkerSchemaDto> template)
    {
        return GetCorrespondingUnit(template, unitType, propertyName)?.Minimum ?? 0;
    }
}