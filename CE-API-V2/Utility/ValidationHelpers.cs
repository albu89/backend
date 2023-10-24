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

    private static BiomarkerSchemaUnit? GetCorrespondingUnit(CadRequestSchema template, string requestUnitType, string propertyName)
    {
        if (requestUnitType == null)
            throw new ArgumentNullException(nameof(requestUnitType));
        if (!Enum.TryParse<UnitType>(requestUnitType, out var parsedUnit))
            parsedUnit = UnitType.SI;
        
        var unitToTest = template.LabResults.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequest)))?
            .Units?.FirstOrDefault(x => x.UnitType == parsedUnit) ?? template.MedicalHistory.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequest)))?.Unit;
        return unitToTest;
    }
    public static float GetMaxValue(string propertyName, string unitType, CadRequestSchema template)
    {
        return GetCorrespondingUnit(template, unitType, propertyName)?.Maximum ?? 0;
    }
    public static float GetMinValue(string propertyName, string unitType, CadRequestSchema template)
    {
        return GetCorrespondingUnit(template, unitType, propertyName)?.Minimum ?? 0;
    }

    public static IEnumerable<BiomarkerSchemaUnit> GetAllUnitsForProperty(string propertyName, CadRequestSchema template)
    {
        var units = template.LabResults.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequest)))?
            .Units ?? new [] { template.MedicalHistory.FirstOrDefault(x => x.Id == GetJsonPropertyKeyName(propertyName, typeof(ScoringRequest))).Unit };

        return units;
    }
}