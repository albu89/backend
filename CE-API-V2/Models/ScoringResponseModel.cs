    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;
    using CE_API_V2.Models.DTO;
    namespace CE_API_V2.Models
{
    /// <remarks>
    /// Class holding the response data which is calculated/transferred by the AI
    /// </remarks>
    public class ScoringResponseModel
    {
        public Guid Id { get; set; }

        public int? classifier_class { get; set; }
        public double? classifier_score { get; set; }
        public int? classifier_sign { get; set; }
        public DateTimeOffset CreatedOn { get; }
        
        public string? Score { get; set; }
        public string? Risk { get; set; }
        public int? RiskClass { get; set; }
        public string? Recommendation { get; set; }
        public string? RecommendationLong { get; set; }
        [NotMapped]
        public string[]? Warnings { get; set; }



        [JsonIgnore]
        public virtual Biomarkers Biomarkers { get; set; }
        public Guid? BiomarkersId { get; set; }
        
        [JsonIgnore]
        public ScoringRequestModel Request { get; set; }
        public Guid RequestId { get; set; }
    }
}
