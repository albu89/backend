using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces;

public interface IScoringTemplateService
{
    public Task<ScoreSchema> GetTemplate( string userId, string locale = LocalizationConstants.DefaultLocale);
}