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
    public StoredBiomarkers Biomarkers { get; set; }
    public bool CanEdit { get; set; }
    public bool IsDraft { get; set; }
}

public class StoredBiomarkers
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public BiomarkerReturnValue<object>[] Values { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}