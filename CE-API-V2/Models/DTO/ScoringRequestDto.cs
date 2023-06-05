using System.Text.Json.Serialization;
using CE_API_V2.Utility;
using static CE_API_V2.DTO.Enums.PatientDataEnums;


namespace CE_API_V2.DTO
{
    public class ScoringRequestDto
    {
        #region Additional Properties
        [JsonPropertyName("clinical_setting")]
        public BiomarkerValueDto<string> clinical_setting { get; set; }
        
        [JsonPropertyName("prior_CAD")]
        public BiomarkerValueDto<bool> prior_CAD { get; set; }
        #endregion

        #region Anamnesis
        [JsonPropertyName("patient_id")]
        //Anamnesis
        public BiomarkerValueDto<string> PatientId { get; set; }
        [JsonPropertyName("age")]
        public BiomarkerValueDto<float> Age { get; set; }
        [JsonPropertyName("sex")]
        [JsonConverter(typeof(BiomarkerValueJsonConverter<SexEnum>))]
        public BiomarkerValueDto<SexEnum> Sex { get; set; }
        [JsonPropertyName("height")]
        public BiomarkerValueDto<float> Height { get; set; }
        [JsonPropertyName("weight")]
        public BiomarkerValueDto<float> Weight { get; set; }
        
        [JsonConverter(typeof(BiomarkerValueJsonConverter<ThoraicPainEnum>))]
        [JsonPropertyName("chest_pain")]
        public BiomarkerValueDto<ThoraicPainEnum> ChestPain { get; set; } // Todo = chest pain?

        [JsonConverter(typeof(BiomarkerValueJsonConverter<NicotineConsumptionEnum>))]
        [JsonPropertyName("nicotine")]
        public BiomarkerValueDto<NicotineConsumptionEnum> NicotineConsumption { get; set; }
        #endregion

        #region Medication
        //Medication
        [JsonConverter(typeof(BiomarkerValueJsonConverter<DiabetesStatusEnum>))]
        [JsonPropertyName("diabetes")]
        public BiomarkerValueDto<DiabetesStatusEnum> Diabetes { get; set; } // = diabetes
        [JsonPropertyName("statin")]
        public BiomarkerValueDto<bool> CholesterolLowering_Statin { get; set; } // = statin
        [JsonPropertyName("tc_agg_inhibitor")]
        public BiomarkerValueDto<bool> TCAggregationInhibitor { get; set; } // tc_agg_inhibitor
        [JsonPropertyName("ace_inhibitor")]
        public BiomarkerValueDto<bool> ACEInhibitor { get; set; } // ace_inhibitor
        [JsonPropertyName("calcium_ant")]
        public BiomarkerValueDto<bool> CaAntagonist { get; set; } // calcium_ant?
        [JsonPropertyName("betablocker")]
        public BiomarkerValueDto<bool> Betablocker { get; set; }
        [JsonPropertyName("diuretic")]
        public BiomarkerValueDto<bool> Diuretic { get; set; }
        [JsonPropertyName("nitrate")]
        public BiomarkerValueDto<bool> OrganicNitrate { get; set; } // nitrate
        #endregion

        #region ClinicalFindigs
        [JsonPropertyName("systolic_bp")]
        public BiomarkerValueDto<float> SystolicBloodPressure { get; set; } // systolic_bp
        [JsonPropertyName("diastolic_bp")]
        public BiomarkerValueDto<float> DiastolicBloodPressure { get; set; } // diastolic_bp

        [JsonConverter(typeof(BiomarkerValueJsonConverter<RestingECGEnum>))]
        [JsonPropertyName("q_wave")]
        public BiomarkerValueDto<RestingECGEnum> RestingECG { get; set; } //Todo -> unterscheide primary und secondary care -> primary care
        #endregion

        #region Enzymes
        [JsonPropertyName("amylase_p")]
        public BiomarkerValueDto<float> PancreaticAmylase { get; set; } // amylase_p
        [JsonPropertyName("alkaline")]
        public BiomarkerValueDto<float> AlkalinePhosphatase { get; set; } // alkaline
        [JsonPropertyName("hs_troponin_t")]
        public BiomarkerValueDto<float> HsTroponinT { get; set; } // hs_troponin_t
        [JsonPropertyName("alat")]
        public BiomarkerValueDto<float> Alat { get; set; } // alat
        #endregion

        #region Diabetes / Blood Sugar
        [JsonPropertyName("glucose")]
        public BiomarkerValueDto<float> GlocuseFasting { get; set; } // glucose
        #endregion

        #region Metabolite
        [JsonPropertyName("bilirubin")]
        public BiomarkerValueDto<float> Bilirubin { get; set; }
        [JsonPropertyName("urea")]
        public BiomarkerValueDto<float> Urea { get; set; }
        [JsonPropertyName("uric_acid")]
        public BiomarkerValueDto<float> UricAcid { get; set; }
        #endregion

        #region Lipids
        [JsonPropertyName("cholesterol")]
        public BiomarkerValueDto<float> Cholesterol { get; set; }
        [JsonPropertyName("hdl")]
        public BiomarkerValueDto<float> Hdl { get; set; }
        [JsonPropertyName("ldl")]
        public BiomarkerValueDto<float> Ldl { get; set; }
        #endregion

        #region Protein
        [JsonPropertyName("protein")]
        public BiomarkerValueDto<float> Protein { get; set; }
        [JsonPropertyName("albumin")]
        public BiomarkerValueDto<float> Albumin { get; set; }
        #endregion

        #region HematologyDto
        [JsonPropertyName("leukocyte")]
        public BiomarkerValueDto<float> Leukocytes { get; set; }
        [JsonPropertyName("mchc")]
        public BiomarkerValueDto<float> Mchc { get; set; }
        #endregion
    }
}
