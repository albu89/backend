using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CE_API_V2.DTO
{
    public class AiDto
    {
        #region Anamnesis
        //Anamnesis
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid Id { get; set; } //Todo 
        public DateTime Datum { get; set; }
        public float Age { get; set; } 
        public float Sex_0_female_1male { get; set; } 
        public float Gr_sse { get; set; }
        public float Gewicht { get; set; } 
        public float Thoraxschmerzen__0_keine_1_extr { get; set; } 
        public float Nicotin_0_nein_1_St__N__2_ja { get; set; } 
        #endregion

        #region Medication
        //Medication
        public float Diabetes_0_no_1_NIDDM_2_IDDM { get; set; } 
        public float Statin_od_Chol_senker { get; set; }
        public float Tc_Aggregation { get; set; } 
        public float ACE_od_ATII { get; set; } 
        public float CA_Antagonist { get; set; } 
        public float Betablocker { get; set; } 
        public float Diureticum { get; set; } 
        public float Nitrat_od_Dancor { get; set; }
        #endregion

        #region ClinicalFindings
        public float BD_syst { get; set; }  
        public float BD_diast { get; set; } 
        public float q_Zacken_0_nein_1_ja { get; set; }
        #endregion

        #region Enzymes
        public float Pankreas_Amylase { get; set; } 
        public float Alk_Phase { get; set; } 
        public float Troponin { get; set; } 
        public float ALAT { get; set; } 
        #endregion

        #region Diabetes
        public float Glucose { get; set; } 
        #endregion

        #region Metabolite
        public float Bilirubin { get; set; } 
        public float Harnstoff { get; set; } 
        public float Harnsaure { get; set; } 
        #endregion

        #region Lipids
        public float Cholesterin_gesamt { get; set; } 
        public float HDL { get; set; } 
        public float LDL { get; set; } 
        #endregion

        #region Proteine
        public float Total_Proteine { get; set; } 
        public float Albumin { get; set; } 
        #endregion

        #region Hematology
        public float Leuko { get; set; } 
        public float MCHC__g_l_oder___ { get; set; }
        #endregion

        #region Zusätzliche Properties? 
        //TODO
        [JsonPropertyName("custom_token")]
        public string CustomToken { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public int ScroingRecordID { get; set; }
        [NotMapped]
        public bool? incomplete { get; set; }
        [NotMapped]
        public string chosenOrgClient { get; set; }
        public string promocode { get; set; }
        [NotMapped]
        public string classifier_type { get; set; } 
        public string units { get; set; }
        [NotMapped]
        public bool? overwrite { get; set; }
        [NotMapped]
        public string PopulationRiskLevel { get; set; } // same field as "classifier_type", but only for API v3 REST API requests
        #endregion
    }
}
