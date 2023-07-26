using System.Globalization;
using AutoMapper;
using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Utility
{
    public class ScoreSummaryUtility : IScoreSummaryUtility
    {
        private readonly IMapper _mapper;

        public ScoreSummaryUtility(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ScoringResponse SetAdditionalScoringParams(ScoringResponse scoreResponse, string locale = LocalizationConstants.DefaultLocale)
        {
            scoreResponse.Warnings = ScoreEvaluator.EvaluateWarnings();
            scoreResponse = SetRecommendation(scoreResponse, locale);
            
            return scoreResponse;
        }

        public IEnumerable<RecommendationCategory> GetCategories(string locale = LocalizationConstants.DefaultLocale)
        {
            var path = TryGetLocalizedFilePath(locale);
            using StreamReader reader = new StreamReader(path);
            var file = reader.ReadToEnd();

            var parsedDoc = JsonSerializationHelper.DeserializeObject<RecommendationCategory[]>(file);

            return parsedDoc;
        }

        private ScoringResponse SetRecommendation(ScoringResponse scoreResponse, string locale)
        {
            var scoreRecommendation = GetScoreRecommendation((double)scoreResponse.classifier_score, locale);
            _mapper.Map(scoreRecommendation, scoreResponse);

            return scoreResponse;
        }

        private RecommendationCategory GetScoreRecommendation(double score, string locale)
        {
            var categories = GetCategories(locale);

            return GetMatchingCategory(categories, score);
        }

        private string TryGetLocalizedFilePath(string locale)
        {
            var filePath = CombineFilePath(locale);

            if (File.Exists(filePath))
                return  filePath;

            filePath = CombineFilePath(LocalizationConstants.DefaultLocale);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The requested file was not found. Requested File: {filePath}");
            }

            return filePath;
        }

        private string CombineFilePath(string locale)
        {
            var file = string.Concat("Recommendation.", locale, ".json");
            return Path.Combine(LocalizationConstants.TemplatesSubpath, file);
        }

        private RecommendationCategory? GetMatchingCategory(IEnumerable<RecommendationCategory> recommendationCategories, double score)
        {
            RecommendationCategory matchingObject = null;

            foreach (var category in recommendationCategories)
            {
                var parsedUpperLimit = double.Parse(category.UpperLimit, CultureInfo.InvariantCulture);
                var parsedLowerLimit = double.Parse(category.LowerLimit, CultureInfo.InvariantCulture);
                var insideUpperLimit = score < parsedUpperLimit;
                var insideLowerLimit = score >= parsedLowerLimit;
                if (insideUpperLimit && insideLowerLimit)
                {
                    matchingObject = category;
                    break;
                }
            }

            return matchingObject;
        }
    }
}
