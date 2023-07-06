using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Services;

public class EmailBuilder : IEmailBuilder
{
    private readonly IEmailTemplateProvider _emailTemplateProvider;
    private readonly IConfiguration _config;

    public EmailBuilder(IEmailTemplateProvider emailTemplateProvider, IConfiguration configuration)
    {
        _emailTemplateProvider = emailTemplateProvider;
        _config = configuration;
    }

    public EMailConfiguration GetEmailConfiguration(AccessRequestDto accessDto)
    {
        var emailBody = _emailTemplateProvider.GetRequestBodyTemplate();
        var mappingDictionary = GetEmailMessageMappingDictionary(accessDto);
        var htmlContent = FormatEmailContent(mappingDictionary, emailBody);

        return new()
        {
            HtmlContent = htmlContent,
            Recipient = accessDto.EmailAddress,
            Sender = _config.GetValue<string>("AzureCommunicationService:MailFrom"),
            Subject = _config.GetValue<string>("AzureCommunicationService:RequestMailSubject"),
        };
    }

    private string FormatEmailContent(Dictionary<string, string> mappingDictionary, string? requestEmailBodyTemplate)
    {
        if (string.IsNullOrEmpty(requestEmailBodyTemplate))
        {
            return string.Empty;
        }

        foreach (var item in mappingDictionary)
        {
            requestEmailBodyTemplate = requestEmailBodyTemplate.Replace(item.Key, item.Value);
        }

        while (requestEmailBodyTemplate.Contains("\r\n"))
        {
            requestEmailBodyTemplate = requestEmailBodyTemplate.Replace("\r\n", "");
        }

        return requestEmailBodyTemplate;
    }

    private Dictionary<string, string> GetEmailMessageMappingDictionary(AccessRequestDto userDto)
    {
        return new()
        {
            { "{{{EmailAddress}}}", userDto.EmailAddress },
            { "{{{FirstName}}}", userDto.FirstName },
            { "{{{LastName}}}", userDto.Surname },
            { "{{{PhoneNumber}}}", userDto.PhoneNumber},
        };
    }
}
