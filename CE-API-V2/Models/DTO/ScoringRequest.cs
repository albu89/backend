using System.Text.Json.Serialization;
using CE_API_V2.Utility;
using static CE_API_V2.Models.Enum.PatientDataEnums;


namespace CE_API_V2.Models.DTO
{
    public class ScoringRequest : IScoringRequest
    {
        #region BaseInfo

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        #endregion

        #region Additional Properties

        [JsonPropertyName("prior_CAD")]
        public BiomarkerValue<bool> prior_CAD { get; set; }
        #endregion

        #region Anamnesis
        //Anamnesis
        [JsonPropertyName("age")]
        public BiomarkerValue<int> Age { get; set; }
        [JsonPropertyName("sex")]
        [JsonConverter(typeof(BiomarkerValueJsonConverter<Sex>))]
        public BiomarkerValue<Sex> Sex { get; set; }
        [JsonPropertyName("height")]
        public BiomarkerValue<int> Height { get; set; }
        [JsonPropertyName("weight")]
        public BiomarkerValue<int> Weight { get; set; }

        [JsonConverter(typeof(BiomarkerValueJsonConverter<ChestPain>))]
        [JsonPropertyName("chest_pain")]
        public BiomarkerValue<ChestPain> ChestPain { get; set; }

        [JsonConverter(typeof(BiomarkerValueJsonConverter<NicotineConsumption>))]
        [JsonPropertyName("nicotine")]
        public BiomarkerValue<NicotineConsumption> NicotineConsumption { get; set; }
        #endregion

        #region Medication
        //Medication
        // [JsonConverter(typeof(BiomarkerValueJsonConverter<DiabetesStatus>))]
        [JsonPropertyName("diabetes")]
        public BiomarkerValue<DiabetesStatus> Diabetes { get; set; } // = diabetes
        [JsonPropertyName("statin")]
        public BiomarkerValue<bool> CholesterolLowering_Statin { get; set; } // = statin
        [JsonPropertyName("tc_agg_inhibitor")]
        public BiomarkerValue<bool> TCAggregationInhibitor { get; set; } // tc_agg_inhibitor
        [JsonPropertyName("ace_inhibitor")]
        public BiomarkerValue<bool> ACEInhibitor { get; set; } // ace_inhibitor
        [JsonPropertyName("calcium_ant")]
        public BiomarkerValue<bool> CaAntagonist { get; set; } // calcium_ant?
        [JsonPropertyName("betablocker")]
        public BiomarkerValue<bool> Betablocker { get; set; }
        [JsonPropertyName("diuretic")]
        public BiomarkerValue<bool> Diuretic { get; set; }
        [JsonPropertyName("nitrate")]
        public BiomarkerValue<bool> OrganicNitrate { get; set; } // nitrate
        #endregion

        #region ClinicalFindigs
        [JsonPropertyName("systolic_bp")]
        public BiomarkerValue<int> SystolicBloodPressure { get; set; } // systolic_bp
        [JsonPropertyName("diastolic_bp")]
        public BiomarkerValue<int> DiastolicBloodPressure { get; set; } // diastolic_bp

        [JsonConverter(typeof(BiomarkerValueJsonConverter<RestingEcg>))]
        [JsonPropertyName("q_wave")]
        public BiomarkerValue<RestingEcg> RestingECG { get; set; }
        #endregion

        #region Enzymes
        [JsonPropertyName("amylase_p")]
        public BiomarkerValue<float> PancreaticAmylase { get; set; } // amylase_p
        [JsonPropertyName("alkaline")]
        public BiomarkerValue<float> AlkalinePhosphatase { get; set; } // alkaline
        [JsonPropertyName("hs_troponin_t")]
        public BiomarkerValue<float> HsTroponinT { get; set; } // hs_troponin_t
        [JsonPropertyName("alat")]
        public BiomarkerValue<float> Alat { get; set; } // alat
        #endregion

        #region Diabetes / Blood Sugar
        [JsonPropertyName("glucose")]
        public BiomarkerValue<float> GlucoseFasting { get; set; } // glucose
        #endregion

        #region Metabolite
        [JsonPropertyName("bilirubin")]
        public BiomarkerValue<float> Bilirubin { get; set; }
        [JsonPropertyName("urea")]
        public BiomarkerValue<float> Urea { get; set; }
        [JsonPropertyName("uric_acid")]
        public BiomarkerValue<float> UricAcid { get; set; }
        #endregion
        #region Lipids
        [JsonPropertyName("cholesterol")]
        public BiomarkerValue<float> Cholesterol { get; set; }
        [JsonPropertyName("hdl")]
        public BiomarkerValue<float> Hdl { get; set; }
        [JsonPropertyName("ldl")]
        public BiomarkerValue<float> Ldl { get; set; }
        #endregion

        #region Protein
        [JsonPropertyName("protein")]
        public BiomarkerValue<float> Protein { get; set; }
        [JsonPropertyName("albumin")]
        public BiomarkerValue<float> Albumin { get; set; }
        #endregion

        #region HematologyDto
        [JsonPropertyName("leukocyte")]
        public BiomarkerValue<float> Leukocytes { get; set; }
        [JsonPropertyName("mchc")]
        public BiomarkerValue<float> Mchc { get; set; }
        #endregion
    }

    public class ScoringRequestDraft : IScoringRequest
    {
        #region BaseInfo

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        #endregion

        #region Additional Properties

        [JsonPropertyName("prior_CAD")] public BiomarkerValue<bool?> prior_CAD { get; set; }

        #endregion

        #region Anamnesis

        //Anamnesis
        [JsonPropertyName("age")] public BiomarkerValue<int?> Age { get; set; }

        [JsonPropertyName("sex")]
        [JsonConverter(typeof(BiomarkerNullableEnumValueJsonConverter<Sex?>))]
        public BiomarkerValue<Sex?> Sex { get; set; }

        [JsonPropertyName("height")] public BiomarkerValue<int?> Height { get; set; }
        [JsonPropertyName("weight")] public BiomarkerValue<int?> Weight { get; set; }

        [JsonConverter(typeof(BiomarkerNullableEnumValueJsonConverter<ChestPain?>))]
        [JsonPropertyName("chest_pain")]
        public BiomarkerValue<ChestPain?> ChestPain { get; set; }

        [JsonConverter(typeof(BiomarkerNullableEnumValueJsonConverter<NicotineConsumption?>))]
        [JsonPropertyName("nicotine")]
        public BiomarkerValue<NicotineConsumption?> NicotineConsumption { get; set; }

        #endregion

        #region Medication

        //Medication
        [JsonConverter(typeof(BiomarkerNullableEnumValueJsonConverter<DiabetesStatus?>))]
        [JsonPropertyName("diabetes")] public BiomarkerValue<DiabetesStatus?> Diabetes { get; set; } // = diabetes
        [JsonPropertyName("statin")] public BiomarkerValue<bool?> CholesterolLowering_Statin { get; set; } // = statin

        [JsonPropertyName("tc_agg_inhibitor")]
        public BiomarkerValue<bool?> TCAggregationInhibitor { get; set; } // tc_agg_inhibitor

        [JsonPropertyName("ace_inhibitor")] public BiomarkerValue<bool?> ACEInhibitor { get; set; } // ace_inhibitor
        [JsonPropertyName("calcium_ant")] public BiomarkerValue<bool?> CaAntagonist { get; set; } // calcium_ant?
        [JsonPropertyName("betablocker")] public BiomarkerValue<bool?> Betablocker { get; set; }
        [JsonPropertyName("diuretic")] public BiomarkerValue<bool?> Diuretic { get; set; }
        [JsonPropertyName("nitrate")] public BiomarkerValue<bool?> OrganicNitrate { get; set; } // nitrate

        #endregion

        #region ClinicalFindigs

        [JsonPropertyName("systolic_bp")] public BiomarkerValue<int?> SystolicBloodPressure { get; set; } // systolic_bp

        [JsonPropertyName("diastolic_bp")]
        public BiomarkerValue<int?> DiastolicBloodPressure { get; set; } // diastolic_bp

        [JsonConverter(typeof(BiomarkerNullableEnumValueJsonConverter<RestingEcg?>))]
        [JsonPropertyName("q_wave")]
        public BiomarkerValue<RestingEcg?> RestingECG { get; set; }

        #endregion

        #region Enzymes

        [JsonPropertyName("amylase_p")] public BiomarkerValue<float?> PancreaticAmylase { get; set; } // amylase_p
        [JsonPropertyName("alkaline")] public BiomarkerValue<float?> AlkalinePhosphatase { get; set; } // alkaline
        [JsonPropertyName("hs_troponin_t")] public BiomarkerValue<float?> HsTroponinT { get; set; } // hs_troponin_t
        [JsonPropertyName("alat")] public BiomarkerValue<float?> Alat { get; set; } // alat

        #endregion

        #region Diabetes / Blood Sugar

        [JsonPropertyName("glucose")] public BiomarkerValue<float?> GlucoseFasting { get; set; } // glucose

        #endregion

        #region Metabolite

        [JsonPropertyName("bilirubin")] public BiomarkerValue<float?> Bilirubin { get; set; }
        [JsonPropertyName("urea")] public BiomarkerValue<float?> Urea { get; set; }
        [JsonPropertyName("uric_acid")] public BiomarkerValue<float?> UricAcid { get; set; }

        #endregion

        #region Lipids

        [JsonPropertyName("cholesterol")] public BiomarkerValue<float?> Cholesterol { get; set; }
        [JsonPropertyName("hdl")] public BiomarkerValue<float?> Hdl { get; set; }
        [JsonPropertyName("ldl")] public BiomarkerValue<float?> Ldl { get; set; }

        #endregion

        #region Protein

        [JsonPropertyName("protein")] public BiomarkerValue<float?> Protein { get; set; }
        [JsonPropertyName("albumin")] public BiomarkerValue<float?> Albumin { get; set; }

        #endregion

        #region HematologyDto

        [JsonPropertyName("leukocyte")] public BiomarkerValue<float?> Leukocytes { get; set; }
        [JsonPropertyName("mchc")] public BiomarkerValue<float?> Mchc { get; set; }

        #endregion
    }

    public interface IScoringRequest
    {
        public string? FirstName { get; set; } 

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
