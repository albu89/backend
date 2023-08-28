using System.Text.Json.Serialization;

namespace CE_API_V2.Models.DTO
{
    public class BiomarkerSchemaUnit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UnitType { get; set; }

        public string Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ConversionFactor { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Minimum { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Maximum { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Values { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Enum { get; set; }
        
        public Dictionary<string, string> DisplayNames { get; set; }

        public string ClinicalSetting { get; set; }
    }
}