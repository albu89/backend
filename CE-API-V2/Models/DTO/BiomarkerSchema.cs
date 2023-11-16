using System.Text.Json.Serialization;
namespace CE_API_V2.Models.DTO
{
    public class CadRequestSchema
    {
        /// <summary>
        /// Categories of all markers
        /// </summary>
        public BiomarkerCategories Categories { get; set; }
        /// <summary>
        /// Markers for Medical History
        /// </summary>
        public Anamnesis[] MedicalHistory { get; set; }
        /// <summary>
        /// Markers for lab results
        /// </summary>
        public Biomarker[] LabResults { get; set; }

        [JsonIgnore]
        public IEnumerable<BiomarkerBase> AllMarkers => LabResults.Concat<BiomarkerBase>(MedicalHistory);
    }

    public class BiomarkerCategories
    {
        /// <summary>
        /// Categories of markers for Medical History
        /// </summary>
        public MedicalHistoryCategories MedicalHistory { get; set; }
        /// <summary>
        /// Categories of markers for LabResults
        /// </summary>
        public LabResultsCategories LabResults { get; set; }
    }
    public class LabResultsCategories
    {
        public string Enzymes { get; set; }
        public string Bloodsugar { get; set; }
        public string Metabolite { get; set; }
        public string Lipids { get; set; }
        public string Protein { get; set; }
        public string Hematology { get; set; }
    }
    public class MedicalHistoryCategories
    {
        public string Anamnesis { get; set; }
        public string Medication { get; set; }
        public string Clinicalfindings { get; set; }
    }

    public class BiomarkerBase
    {
        /// <summary>
        /// Identifier for this biomarker
        /// </summary>
        /// <example>0</example>
        public string Id { get; set; }
        /// <summary>
        /// Index indicating the users preferred order for this biomarker
        /// </summary>
        /// <example>5</example>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>Enzymes</example>
        public string CategoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>Lorem ipsum</example>
        public string InfoText { get; set; }
        /// <summary>
        /// Expected Data type for this marker
        /// </summary>
        /// <example>string, integer, float, options</example>
        public string Type { get; set; }
        /// <summary>
        /// The name to be displayed for this Marker.
        /// </summary>
        /// <example>Age</example>
        public string DisplayName { get; set; }
    }

    public class Anamnesis : BiomarkerBase
    {
        /// <summary>
        /// The Unit in which the Markers value is expected
        /// </summary>
        /// <example>years</example>
        public BiomarkerSchemaUnit Unit { get; set; }
    }

    public class Biomarker : BiomarkerBase
    {   
        /// <summary>
        /// A list of Units this marker can be entered in
        /// </summary>
        public BiomarkerSchemaUnit[] Units { get; set; }
        /// <summary>
        /// The unit for this marker selected by the user
        /// </summary>
        public string PreferredUnit { get; set; }
    }
}
