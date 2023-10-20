namespace CE_API_V2.Models.DTO
{
    public class ScoreSchema 
    {
        public string? ScoreHeader { get; set; }
        public string? Score { get; set; }
        public string? RiskHeader { get; set; }
        public string? Risk { get; set; }
        public string? RecommendationHeader { get; set; }
        public string? Recommendation { get; set; }
        public string? RecommendationExtended { get; set; }
        public string? RecommendationProbabilityHeader { get; set; }
        public string? WarningHeader { get; set; }
        public string[]? Warnings { get; set; }
        public string? InfoText { get; set; }
        public IDictionary<string, string>? Abbreviations { get; set; }
        public string? CadDefinitionHeader { get; set; }
        public string? CadDefinition { get; set; }
        public string? FootnoteHeader { get; set; }
        public string? Footnote { get; set; }
        public string? IntendedUseHeader { get; set; }
        public string? IntendedUse { get; set; }

        public string? RecommendationTableHeader { get; set; }
        public string? RecommendationScoreRangeHeader { get; set; }
        public IEnumerable<RecommendationCategory>? RecommendationCategories { get; set; }

        public CadRequestSchema? Biomarkers { get; set; }
    }
}



