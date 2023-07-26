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
    public PatientDataEnums.ChestPain ChestPain { get; set; } // Enum
    public PatientDataEnums.NicotineConsumption Nicotine { get; set; } // Enum
    public PatientDataEnums.DiabetesStatus Diabetes { get; set; }
    public bool Statin { get; set; }
    public bool TcAggInhibitor { get; set; }
    public bool AceInhibitor { get; set; }
    public bool CaAntagonist { get; set; }
    public bool Betablocker { get; set; }
    public bool Diuretic { get; set; }
    public bool OganicNitrate { get; set; }
    public int SystolicBloodPressure { get; set; }
    public string SystolicBloodPressureUnit { get; set; }
    public int DiastolicBloodPressure { get; set; }
    public string DiastolicBloodPressureUnit { get; set; }
    public PatientDataEnums.RestingEcg RestingECG { get; set; } // Enum
    public float PancreaticAmylase { get; set; }
    public string PancreaticAmylaseUnit { get; set; }
    public float AlkalinePhosphate { get; set; }
    public string AlkalinePhosphataseUnit { get; set; }
    public float HsTroponinT { get; set; }
    public string HsTroponinTUnit { get; set; }
    public float Alat { get; set; }
    public string AlatUnit { get; set; }
    public float Glucose { get; set; }
    public string GlucoseUnit { get; set; }
    public float Bilirubin { get; set; }
    public string BilirubinUnit { get; set; }
    public float Urea { get; set; }
    public string UreaUnit { get; set; }
    public float UricAcid { get; set; }
    public string UricAcidUnit { get; set; }
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
    public float Leukocytes { get; set; }
    public string LeukocytesUnit { get; set; }
    public float Mchc { get; set; }
    public string MchcUnit { get; set; }
    public DateTimeOffset CreatedOn { get; }

}