using System.ComponentModel.DataAnnotations;
using CE_API_V2.Models.Enum;
namespace CE_API_V2.Models;

public class Biomarkers
{
    [Key]
    public Guid Id { get; set; }

    public ScoringRequest Request { get; set; }
    public Guid RequestId { get; set; }
    
    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; } // Enum
    
    public bool PriorCAD { get; set; }
    public int Age { get; set; }
    public PatientDataEnums.Sex Sex { get; set; } // Enum
    public int Height { get; set; }
    public int Weight { get; set; }
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
    public float SystolicBloodPressure { get; set; }
    public float DiastolicBloodPressure { get; set; }
    public PatientDataEnums.RestingEcg RestingECG { get; set; } // Enum
    public float PancreaticAmylase { get; set; }
    public float AlkalinePhosphate { get; set; }
    public float HsTroponinT { get; set; }
    public float Alat { get; set; }
    public float Glucose { get; set; }
    public float Bilirubin { get; set; }
    public float Urea { get; set; }
    public float UricAcid { get; set; }
    public float Cholesterol { get; set; }
    public float Hdl { get; set; }
    public float Ldl { get; set; }
    public float Protein { get; set; }
    public float Albumin { get; set; }
    public float Leukocytes { get; set; }
    public float Mchc { get; set; }
    public DateTimeOffset CreatedOn { get; }

}