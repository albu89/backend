using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces
{
    public interface IScoreSummaryUtility
    {
        public ScoringResponseSummary SetAdditionalScoringParams(ScoringResponseSummary scoreSummaryScore,
            string locale = LocalizationConstants.DefaultLocale);

        public IEnumerable<RecommendationCategory> GetCategories(string locale = LocalizationConstants.DefaultLocale);
    }
}
