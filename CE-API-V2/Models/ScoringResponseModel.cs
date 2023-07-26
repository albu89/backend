    using System.Text.Json.Serialization;
    namespace CE_API_V2.Models
{
    /// <summary>
    /// Class holding the response data which is calculated/transferred by the AI
    /// </summary>
    public class ScoringResponseModel
    {
        public Guid Id { get; set; }

        public int? classifier_class { get; set; }
        public double? classifier_score { get; set; }
        public int? classifier_sign { get; set; }
        public DateTimeOffset CreatedOn { get; }
        
        [JsonIgnore]
        public ScoringRequestModel Request { get; set; }
        public Guid RequestId { get; set; }
    }
}
