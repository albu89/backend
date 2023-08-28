using CE_API_V2.Constants;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;

namespace CE_API_V2.Services.Interfaces
{
    public interface IScoreSummaryUtility
    {
        public ScoringResponse SetAdditionalScoringParams(ScoringResponse scoreResponse,
            string locale = LocalizationConstants.DefaultLocale);

        public IEnumerable<RecommendationCategory> GetCategories(string locale = LocalizationConstants.DefaultLocale, ScoreSummaryUtility.PrevalenceClass valueClass = default);
        bool CalculateIfUpdatePossible(ScoringRequestModel scoringRequestModel);
    }
}
