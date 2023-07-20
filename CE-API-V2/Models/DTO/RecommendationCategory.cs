namespace CE_API_V2.Models.DTO
{
    public class RecommendationCategory
    {
        public int Id { get; set; }
        public string ShortText { get; set; }
        public string LongText { get; set; }
        public string LowerLimit { get; set; }
        public string UpperLimit { get; set; }
        public string RiskValue { get; set; }
    }
}
