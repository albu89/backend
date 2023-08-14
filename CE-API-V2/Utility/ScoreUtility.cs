using System.Globalization;
using System.Text.Json;
using AutoMapper;
using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using static CE_API_V2.Models.Enum.PatientDataEnums;

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

        public IEnumerable<RecommendationCategory> GetCategories(string locale = LocalizationConstants.DefaultLocale, PrevalenceClass prevalenceClass = default)
        {
            var filePathValues = GetFilePath("ClinicalSetting.", prevalenceClass.ToString());
            var filePathLocalizedText = GetFilePath($"Info.{prevalenceClass}.", locale);

            var staticPart = ReadFileContent(filePathValues);
            var localizedPart = ReadFileContent(filePathLocalizedText);

            var deserializedStaticPart = JsonSerializationHelper.DeserializeObject<RecommendationCategoryStaticPart[]>(staticPart);
            var deserializedLocalizedPart = JsonSerializationHelper.DeserializeObject<RecommendationCategoryLocalizedPart[]>(localizedPart);

            return CombineSchemas(deserializedStaticPart, deserializedLocalizedPart, prevalenceClass);
        }

        private ScoringResponse SetRecommendation(ScoringResponse scoreResponse, string locale)
        {
            var score = (double)scoreResponse.classifier_score;
            var priorCad = scoreResponse.Biomarkers.PriorCAD;
            var clinicalSetting = scoreResponse.Biomarkers.ClinicalSetting;

            var scoreRecommendation = GetScoreRecommendation(score, priorCad, clinicalSetting, locale);
            _mapper.Map(scoreRecommendation, scoreResponse);
            scoreResponse.Prevalence = scoreRecommendation.Prevalence;
            return scoreResponse;
        }

        private string GetFilePath(string filePartName, string valueClass)
        {
            var filePath = CombineFilePath(filePartName, valueClass);

            if (File.Exists(filePath))
                return filePath;

            filePath = CombineFilePath(filePartName, LocalizationConstants.DefaultLocale);

            CheckIfFileExists(filePath);

            return filePath;
        }
        
        private string CombineFilePath(string filePartName, string distinguisher)
        {
            var file = string.Concat("Recommendation.", filePartName, distinguisher, ".json");

            return Path.Combine(LocalizationConstants.TemplatesSubpath, file);
        }

        private void CheckIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The requested file was not found. Requested File: {filePath}");
            }
        }

        private string ReadFileContent(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }

        private IEnumerable<RecommendationCategory> CombineSchemas(RecommendationCategoryStaticPart[] recommendationSettings, RecommendationCategoryLocalizedPart[] recommendationLocalized, PrevalenceClass prevalenceCategory)
        {
            var recommendationCategories = new List<RecommendationCategory>();

            foreach (var item in recommendationSettings)
            {
                var langSpec = recommendationLocalized.FirstOrDefault(x => x.Id.Equals(item.Id));
                var recommendationCategory = _mapper.Map<RecommendationCategory>(item);
                _mapper.Map(langSpec, recommendationCategory);
                recommendationCategory.Prevalence = prevalenceCategory;
                recommendationCategories.Add(recommendationCategory);
            }

            return recommendationCategories;
        }

        private RecommendationCategory GetScoreRecommendation(double score, bool priorCad, ClinicalSetting clinicalSetting, string locale)
        {
            var valueClass = DeterminePrevalenceClass(clinicalSetting, priorCad);
            var categories = GetCategories(locale, valueClass);

            return GetMatchingCategory(categories, score);
        }

        private PrevalenceClass DeterminePrevalenceClass(ClinicalSetting clinicalSetting, bool priorCad)
            => clinicalSetting == ClinicalSetting.SecondaryCare || priorCad ? PrevalenceClass.Secondary : PrevalenceClass.Primary;

        private RecommendationCategory? GetMatchingCategory(IEnumerable<RecommendationCategory> recommendationCategories, double score)
        {
            RecommendationCategory? matchingObject = null;

            foreach (var category in recommendationCategories)
            {
                var parsedUpperLimit = ParseLimit(category.UpperLimit);
                var parsedLowerLimit = ParseLimit(category.LowerLimit);
                var insideUpperLimit = score < parsedUpperLimit;
                var insideOrLowerLimit = score >= parsedLowerLimit;
                if (insideUpperLimit && insideOrLowerLimit)
                {
                    matchingObject = category;
                    break;
                }
            }

            if (matchingObject is null)
            {
                var lastCategory = recommendationCategories.LastOrDefault();

                matchingObject = score > ParseLimit(lastCategory?.UpperLimit) ? recommendationCategories.LastOrDefault() : null;
            }

            return matchingObject;
        }

        private double ParseLimit(string value) => double.Parse(value, CultureInfo.InvariantCulture);
  
        public enum PrevalenceClass
        {
            Primary,
            Secondary
        }
    }
}
