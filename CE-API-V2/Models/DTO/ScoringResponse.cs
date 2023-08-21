using CE_API_V2.Utility;
namespace CE_API_V2.Models.DTO;

public class ScoringResponse
{
    public double? classifier_score { get; set; }
    public Guid RequestId { get; set; }
    public string? RiskValue { get; set; }
    public int RiskClass { get; set; }
    public string[]? Warnings { get; set; }
    public string? RecommendationSummary { get; set; }
    public string? RecommendationLongText { get; set; }
    public ScoreSummaryUtility.PrevalenceClass Prevalence { get; set; }
    public Biomarkers? Biomarkers { get; set; }
    public bool CanEdit { get; set; }
}