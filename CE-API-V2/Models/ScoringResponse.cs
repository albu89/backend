    namespace CE_API_V2.Models
{
    /// <summary>
    /// Class holding the response data which is calculated/transferred by the AI
    /// </summary>
    public class ScoringResponse
    {
        public int Id { get; set; } 

        #region not used?
        public float Score { get; set; }
        public int Class { get; set; }
        public int is_CAD_plus { get; set; }
        public int is_H_plus { get; set; }
        public int error_code { get; set; }
        public string message { get; set; }
        #endregion

        public int? classifier_class { get; set; }
        public double? classifier_score { get; set; }
        public int? classifier_sign { get; set; }

        public int timestamp { get; set; }

        public string classifier_type { get; set; }

        public string username { get; set; }

        public bool? hidden { get; set; }

        public string orgclient { get; set; }
    }
}
