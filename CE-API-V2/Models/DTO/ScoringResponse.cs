namespace CE_API_V2.Models.DTO;

public class ScoringResponse
{
    public int? classifier_class { get; set; }
    public double? classifier_score { get; set; }
    public int? classifier_sign { get; set; }
    public Guid RequestId { get; set; }
    public string? RiskValue { get; set; }
    public int RiskClass { get; set; }
    public string[]? Warnings { get; set; }
    public string? RecommendationSummary { get; set; }
    public string? RecommendationLongText { get; set; }
    public Biomarkers? Biomarkers { get; set; }
}