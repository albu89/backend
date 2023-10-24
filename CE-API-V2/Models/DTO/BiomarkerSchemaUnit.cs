using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace CE_API_V2.Models.DTO
{
    public class BiomarkerSchemaUnit
    {
        /// <summary>
        /// Signifies if the Unit is in SI or Conventional.
        /// </summary>
        /// <example>SI</example>
        public UnitType UnitType { get; set; }

        /// <summary>
        /// Signifies the units shorthand, i.e. g/L, mg/mmol, etc.
        /// </summary>
        /// <example>mg/ml</example>
        public string Shorthand { get; set; }
        
        /// <summary>
        /// The conversion factor to be used when converting back to SI units.
        /// </summary>
        /// <example>0.324</example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? ConversionFactor { get; set; }

        /// <summary>
        /// The minimum allowed value for the property in this unit.
        /// </summary>
        /// <example>0.23</example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Minimum { get; set; }

        /// <summary>
        /// The maximum allowed value for the property in this unit.
        /// </summary>
        /// <example>100.25</example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Maximum { get; set; }
        
        /// <summary>
        /// Provides a list of Inputs that may be entered into Markers of type 'string'
        /// </summary>
        public Option[] Options { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Value expected by the API.
        /// </summary>
        /// <example>Screening</example>
        public string Value { get; set; }
        /// <summary>
        /// Name that's displayed for this Option.
        /// </summary>
        /// <example>pathological</example>
        public string DisplayName { get; set; }
        /// <summary>
        /// ID of a marker impacting visibility.
        /// </summary>
        /// <example>prior_CAD</example>
        public string SideEffectId { get; set; }
        /// <summary>
        /// If this value is set, the marker is hidden.
        /// </summary>
        /// <example>true</example>
        public string SideEffectValue { get; set; }
    }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UnitType
    {
        SI,
        Conventional
    }
}