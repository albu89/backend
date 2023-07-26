using System.Reflection;
using System.Text.Json.Serialization;
using CE_API_V2.Models.DTO;
namespace CE_API_V2.Utility;

public static class ValidationHelpers
{
    public static string? GetJsonPropertyKeyName(string propertyName, Type typeToCheck)
    {
        var props = typeToCheck.GetProperties();
        if (props is null)
        {
            return "";
        }
        var prop = props.FirstOrDefault(x => x.Name == propertyName);
        var customAttribute = prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute;
        return customAttribute?.Name;
    }

    public static PropertyInfo? GetPropertyByJsonKey(string jsonKey, Type typeToCheck)
    {
        var props = typeToCheck.GetProperties();
        var prop = props.FirstOrDefault(prop => (prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute)?.Name == jsonKey);
        return prop;
    }

    private static BiomarkerSchemaUnit? GetCorrespondingUnit(IEnumerable<BiomarkerSchema> template, string requestUnitType, string propertyName)
    {

        if (requestUnitType == null)
            throw new ArgumentNullException(nameof(requestUnitType));
        var unitToTest = template.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequest)))?
            .Units?.FirstOrDefault(x => x.UnitType == requestUnitType);
        return unitToTest;
    }
    public static float GetMaxValue(string propertyName, string unitType, IEnumerable<BiomarkerSchema> template)
    {
        return GetCorrespondingUnit(template, unitType, propertyName)?.Maximum ?? 0;
    }
    public static float GetMinValue(string propertyName, string unitType, IEnumerable<BiomarkerSchema> template)
    {
        return GetCorrespondingUnit(template, unitType, propertyName)?.Minimum ?? 0;
    }

    public static IEnumerable<BiomarkerSchemaUnit> GetAllUnitsForProperty(string propertyName, IEnumerable<BiomarkerSchema> template)
    {
        var units = template.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequest)))?
            .Units;
        return units;
    }
}