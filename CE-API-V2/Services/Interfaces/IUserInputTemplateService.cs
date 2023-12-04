using CE_API_V2.Constants;
using CE_API_V2.Models;
using CE_API_V2.Models.Records;

namespace CE_API_V2.Services.Interfaces;

public interface IUserInputTemplateService
{
    public Task<UserInputFormSchema> GetTemplate(UserIdsRecord userId, string locale = LocalizationConstants.DefaultLocale);
}