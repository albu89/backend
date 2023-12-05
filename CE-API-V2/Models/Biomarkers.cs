using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CE_API_V2.Models.Enum;
namespace CE_API_V2.Models;

public class Biomarkers
{
    [Key] public Guid Id { get; set; }
    [JsonIgnore] public ScoringRequestModel Request { get; set; }
    public Guid RequestId { get; set; }

    [JsonIgnore] public ScoringResponseModel Response { get; set; }

    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; } // Enum
    public string ClinicalSettingUnit { get; set; }
    public string ClinicalSettingDisplayValue { get; set; }
    public bool PriorCAD { get; set; }
    public string PriorCADDisplayValue { get; set; }
    public int Age { get; set; }
    public string AgeUnit { get; set; }
    public string AgeDisplayValue { get; set; }
    public PatientDataEnums.Sex Sex { get; set; } // Enum
    public string SexDisplayValue { get; set; } // Enum
    public int Height { get; set; }
    public string HeightUnit { get; set; }
    public string HeightDisplayValue { get; set; }
    public int Weight { get; set; }
    public string WeightUnit { get; set; }
    public string WeightDisplayValue { get; set; }
    public PatientDataEnums.ChestPain Chestpain { get; set; } // Enum
    public string ChestpainDisplayValue { get; set; } // Enum
    public PatientDataEnums.NicotineConsumption Nicotine { get; set; } // Enum
    public string NicotineDisplayValue { get; set; } // Enum
    public PatientDataEnums.DiabetesStatus Diabetes { get; set; }
    public string DiabetesDisplayValue { get; set; }
    public bool Statin { get; set; }
    public string StatinDisplayValue { get; set; }
    public bool Tcagginhibitor { get; set; }
    public string TcagginhibitorDisplayValue { get; set; }
    public bool Aceinhibitor { get; set; }
    public string AceinhibitorDisplayValue { get; set; }
    public bool Calciumant { get; set; }
    public string CalciumantDisplayValue { get; set; }
    public bool Betablocker { get; set; }
    public string BetablockerDisplayValue { get; set; }
    public bool Diuretic { get; set; }
    public string DiureticDisplayValue { get; set; }
    public bool Nitrate { get; set; }
    public string NitrateDisplayValue { get; set; }
    public int Systolicbp { get; set; }
    public string SystolicbpUnit { get; set; }
    public string SystolicbpDisplayValue { get; set; }
    public int Diastolicbp { get; set; }
    public string DiastolicbpUnit { get; set; }
    public string DiastolicbpDisplayValue { get; set; }
    public PatientDataEnums.RestingEcg Qwave { get; set; } // Enum
    public string QwaveDisplayValue { get; set; } // Enum
    public float Amylasep { get; set; }
    public string AmylasepUnit { get; set; }
    public string AmylasepDisplayValue { get; set; }
    public float Alkaline { get; set; }
    public string AlkalineUnit { get; set; }
    public string AlkalineDisplayValue { get; set; }
    public float Hstroponint { get; set; }
    public string HstroponintUnit { get; set; }
    public string HstroponintDisplayValue { get; set; }
    public float Alat { get; set; }
    public string AlatUnit { get; set; }
    public string AlatDisplayValue { get; set; }
    public float Glucose { get; set; }
    public string GlucoseUnit { get; set; }
    public string GlucoseDisplayValue { get; set; }
    public float Bilirubin { get; set; }
    public string BilirubinUnit { get; set; }
    public string BilirubinDisplayValue { get; set; }
    public float Urea { get; set; }
    public string UreaUnit { get; set; }
    public string UreaDisplayValue { get; set; }
    public float Uricacid { get; set; }
    public string UricacidUnit { get; set; }
    public string UricacidDisplayValue { get; set; }
    public float Cholesterol { get; set; }
    public string CholesterolUnit { get; set; }
    public string CholesterolDisplayValue { get; set; }
    public float Hdl { get; set; }
    public string HdlUnit { get; set; }
    public string HdlDisplayValue { get; set; }
    public float Ldl { get; set; }
    public string LdlUnit { get; set; }
    public string LdlDisplayValue { get; set; }
    public float Protein { get; set; }
    public string ProteinUnit { get; set; }
    public string ProteinDisplayValue { get; set; }
    public float Albumin { get; set; }
    public string AlbuminUnit { get; set; }
    public string AlbuminDisplayValue { get; set; }
    public float Leukocyte { get; set; }
    public string LeukocyteUnit { get; set; }
    public string LeukocyteDisplayValue { get; set; }
    public float Mchc { get; set; }
    public string MchcUnit { get; set; }
    public string MchcDisplayValue { get; set; }
    public DateTimeOffset CreatedOn { get; }
}

public class BiomarkersDraft
{
    [Key]
    public Guid Id { get; set; }
    [JsonIgnore]
    public ScoringRequestModel Request { get; set; }
    public Guid RequestId { get; set; }

    [JsonIgnore]
    public ScoringResponseModel? Response { get; set; }

    public PatientDataEnums.ClinicalSetting? ClinicalSetting { get; set; } // Enum
    public string? ClinicalSettingUnit { get; set; }
    public string? ClinicalSettingDisplayValue { get; set; }
    public bool? PriorCAD { get; set; }
    public string? PriorCADDisplayValue { get; set; }
    public int? Age { get; set; }
    public string? AgeUnit { get; set; }
    public string? AgeDisplayValue { get; set; }
    public PatientDataEnums.Sex? Sex { get; set; } // Enum
    public string? SexDisplayValue { get; set; } // Enum
    public int? Height { get; set; }
    public string? HeightUnit { get; set; }
    public string? HeightDisplayValue { get; set; }
    public int? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public string? WeightDisplayValue { get; set; }
    public PatientDataEnums.ChestPain? Chestpain { get; set; } // Enum
    public string? ChestpainDisplayValue { get; set; } // Enum
    public PatientDataEnums.NicotineConsumption? Nicotine { get; set; } // Enum
    public string? NicotineDisplayValue { get; set; } // Enum
    public PatientDataEnums.DiabetesStatus? Diabetes { get; set; }
    public string? DiabetesDisplayValue { get; set; }
    public bool? Statin { get; set; }
    public string? StatinDisplayValue { get; set; }
    public bool? Tcagginhibitor { get; set; }
    public string? TcagginhibitorDisplayValue { get; set; }
    public bool? Aceinhibitor { get; set; }
    public string? AceinhibitorDisplayValue { get; set; }
    public bool? Calciumant { get; set; }
    public string? CalciumantDisplayValue { get; set; }
    public bool? Betablocker { get; set; }
    public string? BetablockerDisplayValue { get; set; }
    public bool? Diuretic { get; set; }
    public string? DiureticDisplayValue { get; set; }
    public bool? Nitrate { get; set; }
    public string? NitrateDisplayValue { get; set; }
    public int? Systolicbp { get; set; }
    public string? SystolicbpUnit { get; set; }
    public string? SystolicbpDisplayValue { get; set; }
    public int? Diastolicbp { get; set; }
    public string? DiastolicbpUnit { get; set; }
    public string? DiastolicbpDisplayValue { get; set; }
    public PatientDataEnums.RestingEcg? Qwave { get; set; } // Enum
    public string? QwaveDisplayValue { get; set; } // Enum
    public float? Amylasep { get; set; }
    public string? AmylasepUnit { get; set; }
    public string? AmylasepDisplayValue { get; set; }
    public float? Alkaline { get; set; }
    public string? AlkalineUnit { get; set; }
    public string? AlkalineDisplayValue { get; set; }
    public float? Hstroponint { get; set; }
    public string? HstroponintUnit { get; set; }
    public string? HstroponintDisplayValue { get; set; }
    public float? Alat { get; set; }
    public string? AlatUnit { get; set; }
    public string? AlatDisplayValue { get; set; }
    public float? Glucose { get; set; }
    public string? GlucoseUnit { get; set; }
    public string? GlucoseDisplayValue { get; set; }
    public float? Bilirubin { get; set; }
    public string? BilirubinUnit { get; set; }
    public string? BilirubinDisplayValue { get; set; }
    public float? Urea { get; set; }
    public string? UreaUnit { get; set; }
    public string? UreaDisplayValue { get; set; }
    public float? Uricacid { get; set; }
    public string? UricacidUnit { get; set; }
    public string? UricacidDisplayValue { get; set; }
    public float? Cholesterol { get; set; }
    public string? CholesterolUnit { get; set; }
    public string? CholesterolDisplayValue { get; set; }
    public float? Hdl { get; set; }
    public string? HdlUnit { get; set; }
    public string? HdlDisplayValue { get; set; }
    public float? Ldl { get; set; }
    public string? LdlUnit { get; set; }
    public string? LdlDisplayValue { get; set; }
    public float? Protein { get; set; }
    public string? ProteinUnit { get; set; }
    public string? ProteinDisplayValue { get; set; }
    public float? Albumin { get; set; }
    public string? AlbuminUnit { get; set; }
    public string? AlbuminDisplayValue { get; set; }
    public float? Leukocyte { get; set; }
    public string? LeukocyteUnit { get; set; }
    public string? LeukocyteDisplayValue { get; set; }
    public float? Mchc { get; set; }
    public string? MchcUnit { get; set; }
    public string? MchcDisplayValue { get; set; }
    public DateTimeOffset CreatedOn { get; }
    public DateTimeOffset UpdatedOn { get; }
}