using System.Text.Json.Serialization;

namespace CE_API_V2.Models.DTO
{
    public class BiomarkerOrder
    {
        #region Additional Properties
        
        [JsonPropertyName("prior_CAD")]
        public BiomarkerOrderEntry prior_CAD { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 0};
        #endregion

        #region Anamnesis
        //Anamnesis
        [JsonPropertyName("age")]
        public BiomarkerOrderEntry Age { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 1};

        [JsonPropertyName("sex")]        
        public BiomarkerOrderEntry Sex { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 2};

        [JsonPropertyName("height")]
        public BiomarkerOrderEntry Height { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 3};
        [JsonPropertyName("weight")]
        public BiomarkerOrderEntry Weight { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 4};
        
        
        [JsonPropertyName("chest_pain")]
        public BiomarkerOrderEntry ChestPain { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 5}; // Todo = chest pain?

        
        [JsonPropertyName("nicotine")]
        public BiomarkerOrderEntry NicotineConsumption { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 6};
        #endregion

        #region Medication
        //Medication
        
        [JsonPropertyName("diabetes")]
        public BiomarkerOrderEntry Diabetes { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 7}; // = diabetes
        [JsonPropertyName("statin")]
        public BiomarkerOrderEntry CholesterolLowering_Statin { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 8}; // = statin
        [JsonPropertyName("tc_agg_inhibitor")]
        public BiomarkerOrderEntry TCAggregationInhibitor { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 9}; // tc_agg_inhibitor
        [JsonPropertyName("ace_inhibitor")]
        public BiomarkerOrderEntry ACEInhibitor { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 10}; // ace_inhibitor
        [JsonPropertyName("calcium_ant")]
        public BiomarkerOrderEntry CaAntagonist { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 11}; // calcium_ant?
        [JsonPropertyName("betablocker")]
        public BiomarkerOrderEntry Betablocker { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 12};
        [JsonPropertyName("diuretic")]
        public BiomarkerOrderEntry Diuretic { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 13};
        [JsonPropertyName("nitrate")]
        public BiomarkerOrderEntry OrganicNitrate { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 14}; // nitrate
        #endregion

        #region ClinicalFindigs
        [JsonPropertyName("systolic_bp")]
        public BiomarkerOrderEntry SystolicBloodPressure { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 15}; // systolic_bp
        [JsonPropertyName("diastolic_bp")]
        public BiomarkerOrderEntry DiastolicBloodPressure { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 16}; // diastolic_bp

        
        [JsonPropertyName("q_wave")]
        public BiomarkerOrderEntry RestingECG { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 17}; 
        #endregion

        #region Enzymes
        [JsonPropertyName("amylase_p")]
        public BiomarkerOrderEntry PancreaticAmylase { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 18}; // amylase_p
        [JsonPropertyName("alkaline")]
        public BiomarkerOrderEntry AlkalinePhosphatase { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 19}; // alkaline
        [JsonPropertyName("hs_troponin_t")]
        public BiomarkerOrderEntry HsTroponinT { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 20}; // hs_troponin_t
        [JsonPropertyName("alat")]
        public BiomarkerOrderEntry Alat { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 21}; // alat
        #endregion

        #region Diabetes / Blood Sugar
        [JsonPropertyName("glucose")]
        public BiomarkerOrderEntry GlucoseFasting { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 22}; // glucose
        #endregion

        #region Metabolite
        [JsonPropertyName("bilirubin")]
        public BiomarkerOrderEntry Bilirubin { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 23};
        [JsonPropertyName("urea")]
        public BiomarkerOrderEntry Urea { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 24};
        [JsonPropertyName("uric_acid")]
        public BiomarkerOrderEntry UricAcid { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 25};
        #endregion

        #region Lipids
        [JsonPropertyName("cholesterol")]
        public BiomarkerOrderEntry Cholesterol { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 26};
        [JsonPropertyName("hdl")]
        public BiomarkerOrderEntry Hdl { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 27};
        [JsonPropertyName("ldl")]
        public BiomarkerOrderEntry Ldl { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 28};
        #endregion

        #region Protein
        [JsonPropertyName("protein")]
        public BiomarkerOrderEntry Protein { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 29};
        [JsonPropertyName("albumin")]
        public BiomarkerOrderEntry Albumin { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 30};
        #endregion

        #region HematologyDto
        [JsonPropertyName("leukocyte")]
        public BiomarkerOrderEntry Leukocytes { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 31};
        [JsonPropertyName("mchc")]
        public BiomarkerOrderEntry Mchc { get; set; } = new BiomarkerOrderEntry{ OrderNumber = 32};
        #endregion
    }
}