using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CE_API_V2.Models.Enum;
namespace CE_API_V2.Models;

public class Biomarkers
{
    [Key]
    public Guid Id { get; set; }
    [JsonIgnore]
    public ScoringRequestModel Request { get; set; }
    public Guid RequestId { get; set; }

    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; } // Enum

    public bool PriorCAD { get; set; }
    public int Age { get; set; }
    public string AgeUnit { get; set; }
    public PatientDataEnums.Sex Sex { get; set; } // Enum
    public int Height { get; set; }
    public string HeightUnit { get; set; }
    public int Weight { get; set; }
    public string WeightUnit { get; set; }
    public PatientDataEnums.ChestPain Chestpain { get; set; } // Enum
    public PatientDataEnums.NicotineConsumption Nicotine { get; set; } // Enum
    public PatientDataEnums.DiabetesStatus Diabetes { get; set; }
    public bool Statin { get; set; }
    public bool Tcagginhibitor { get; set; }
    public bool Aceinhibitor { get; set; }
    public bool Calciumant { get; set; }
    public bool Betablocker { get; set; }
    public bool Diuretic { get; set; }
    public bool Nitrate { get; set; }
    public int Systolicbp { get; set; }
    public string SystolicbpUnit { get; set; }
    public int Diastolicbp { get; set; }
    public string DiastolicbpUnit { get; set; }
    public PatientDataEnums.RestingEcg Qwave { get; set; } // Enum
    public float Amylasep { get; set; }
    public string AmylasepUnit { get; set; }
    public float Alkaline { get; set; }
    public string AlkalineUnit { get; set; }
    public float Hstroponint { get; set; }
    public string HstroponintUnit { get; set; }
    public float Alat { get; set; }
    public string AlatUnit { get; set; }
    public float Glucose { get; set; }
    public string GlucoseUnit { get; set; }
    public float Bilirubin { get; set; }
    public string BilirubinUnit { get; set; }
    public float Urea { get; set; }
    public string UreaUnit { get; set; }
    public float Uricacid { get; set; }
    public string UricacidUnit { get; set; }
    public float Cholesterol { get; set; }
    public string CholesterolUnit { get; set; }
    public float Hdl { get; set; }
    public string HdlUnit { get; set; }
    public float Ldl { get; set; }
    public string LdlUnit { get; set; }
    public float Protein { get; set; }
    public string ProteinUnit { get; set; }
    public float Albumin { get; set; }
    public string AlbuminUnit { get; set; }
    public float Leukocyte { get; set; }
    public string LeukocyteUnit { get; set; }
    public float Mchc { get; set; }
    public string MchcUnit { get; set; }
    public DateTimeOffset CreatedOn { get; }

}