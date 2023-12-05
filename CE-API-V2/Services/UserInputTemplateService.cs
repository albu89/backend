using AutoMapper;
using CE_API_V2.Constants;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CE_API_V2.Services;

public class UserInputTemplateService : IUserInputTemplateService
{
    private const string Subfolder = "Templates";

    private readonly IMapper _mapper;
    private readonly IScoreSummaryUtility _scoreSummaryUtility;
    private readonly IBiomarkersTemplateService _biomarkersTemplateService;
    private readonly IUserUOW _userUow;

    public UserInputTemplateService(IMapper mapper, IUserUOW userUow)
    {
        _mapper = mapper;
        _userUow = userUow;
    }

    public async Task<UserInputFormSchema> GetTemplate(UserIdsRecord userIdInformation,  string locale = LocalizationConstants.DefaultLocale)
    {
        var currentUser = _userUow.GetUser(userIdInformation.UserId, userIdInformation);

        var serializedSchema = await ReadLocalizedJsonTemplate(locale);
        var deserializedSchema = JsonSerializer.Deserialize<UserInputFormSchemaHeaders>(serializedSchema);

        if (currentUser is null)
        {
            var userInputFormSchemaEmpty = _mapper.Map<UserInputFormSchema>(deserializedSchema);
            userInputFormSchemaEmpty.Billing = deserializedSchema.Billing;

            return userInputFormSchemaEmpty;
        }

        var createUser = _mapper.Map<CreateUser>(currentUser);
        var userId = currentUser.UserId;

        var billing = _userUow.GetBilling(userId);
        
        var billingTemplate = billing is null 
            ? ManualMapper.MapFromBillingModelToBillingTemplate(billing, deserializedSchema.Billing)
            : ManualMapper.MapFromBillingModelToBillingTemplate(new BillingModel(), deserializedSchema.Billing);

        var userInputFormSchema = _mapper.Map<UserInputFormSchema>(deserializedSchema);

        _mapper.Map(createUser, userInputFormSchema);
        userInputFormSchema.Billing = billingTemplate;

        return userInputFormSchema;
    }

    private async Task<string> ReadLocalizedJsonTemplate(string locale)
    {
        var filePath = TryGetLocalizedFilePath(locale);
        using StreamReader reader = new StreamReader(filePath);
        var json = await reader.ReadToEndAsync();

        return json;
    }

    private static string TryGetLocalizedFilePath(string locale)
    {
        var fileName = string.Concat("UserInputFormSchema.", locale, ".json");
        var path = Path.Combine(Subfolder, fileName);

        if (File.Exists(path))
            return path;

        fileName = string.Concat("UserInputFormSchema.", LocalizationConstants.DefaultLocale, ".json");
        path = Path.Combine(Subfolder, fileName);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The requested file was not found. Requested File: {path}");
        }

        return path;
    }
}