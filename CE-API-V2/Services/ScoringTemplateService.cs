using System.Text.Json;
using AutoMapper;
using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.Identity.Client;

namespace CE_API_V2.Services;

public class ScoringTemplateService : IScoringTemplateService
{
    private const string Subfolder = "Templates";

    private readonly IMapper _mapper;
    private readonly IScoreSummaryUtility _scoreSummaryUtility;
    private readonly IBiomarkersTemplateService _biomarkersTemplateService;
    private readonly IUserUOW _userUow;

    public ScoringTemplateService(IMapper mapper, IBiomarkersTemplateService biomarkersTemplateService, IScoreSummaryUtility scoreSummaryUtility, IUserUOW userUow)
    {
        _mapper = mapper;
        _biomarkersTemplateService = biomarkersTemplateService;
        _scoreSummaryUtility = scoreSummaryUtility;
        _userUow = userUow;
    }

    public async Task<ScoreSummary> GetTemplate(string userId, string locale = LocalizationConstants.DefaultLocale)
    {
        var filePath = TryGetLocalizedFilePath(locale);
        using StreamReader reader = new StreamReader(filePath);
        var json = await reader.ReadToEndAsync();

        var deserializedSchema = JsonSerializer.Deserialize<ScoringSchema>(json);
        var categories = _scoreSummaryUtility.GetCategories(locale);
        var biomarkers = await _biomarkersTemplateService.GetTemplate(locale);
        biomarkers = _userUow.OrderTemplate(biomarkers, userId);

        var scoreSchemaDto = _mapper.Map<ScoreSummary>(deserializedSchema);
        scoreSchemaDto.Biomarkers = biomarkers.ToList();
        scoreSchemaDto.RecommendationCategories = categories;

        return scoreSchemaDto;
    }

    private static string TryGetLocalizedFilePath(string locale)
    {
        var fileName = string.Concat("ScoringSchema.", locale, ".json");
        var path = Path.Combine(Subfolder, fileName);

        if (File.Exists(path))
            return path;

        fileName = string.Concat("ScoringSchema.", LocalizationConstants.DefaultLocale, ".json");
        path = Path.Combine(Subfolder, fileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The requested file was not found. Requested File: {path}");
        }

        return path;
    }
}