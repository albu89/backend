using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Services;

/// <inheritdoc cref="EmailBuilder"/>
public class EmailBuilder : IEmailBuilder
{
    private readonly IEmailTemplateProvider _emailTemplateProvider;
    private readonly IConfiguration _config;
    private readonly IResponsibilityDeterminer _responsibilityDeterminer;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailBuilder"/> class.
    /// </summary>
    /// <param name="emailTemplateProvider">The email template provider.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="responsibilityDeterminer">The responsibility determiner.</param>
    public EmailBuilder(IEmailTemplateProvider emailTemplateProvider, IConfiguration configuration, IResponsibilityDeterminer responsibilityDeterminer)
    {
        _emailTemplateProvider = emailTemplateProvider;
        _config = configuration;
        _responsibilityDeterminer = responsibilityDeterminer;
    }

    /// <inheritdoc/>
    public EMailConfiguration GetRequestAccessEmailConfiguration(AccessRequest access)
    {
        var emailBody = _emailTemplateProvider.GetRequestBodyTemplate();
        var mappingDictionary = GetRequestAccessMappingDictionary(access);
        var htmlContent = FormatEmailContent(mappingDictionary, emailBody);
        var emailAddress = _responsibilityDeterminer.GetEmailAddress(access.Country, access.Organization, false);

        var sender = _config.GetValue<string>("AzureCommunicationService:MailFrom");
        var subject = _config.GetValue<string>("AzureCommunicationService:RequestMailSubject");

        return new()
        {
            HtmlContent = htmlContent,
            Recipient = emailAddress,
            Sender = sender ?? string.Empty,
            Subject = subject ?? string.Empty
        };
    }

    /// <inheritdoc/>
    public EMailConfiguration GetActivateUserEmailConfiguration(UserModel user)
    {
        var emailBody = _emailTemplateProvider.GetActivateUserBodyTemplate();
        var mappingDictionary = GetActivateUserMappingDictionary(user);
        var htmlContent = FormatEmailContent(mappingDictionary, emailBody);
        var emailAddress = _responsibilityDeterminer.GetEmailAddress(user.Country, user.Department, user.IsActive);

        var sender = _config.GetValue<string>("AzureCommunicationService:MailFrom");
        var subject = _config.GetValue<string>("AzureCommunicationService:ActivateUserMailSubject");

        return new()
        {
            HtmlContent = htmlContent,
            Recipient = emailAddress,
            Sender = sender ?? string.Empty,
            Subject = subject ?? string.Empty
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

    private Dictionary<string, string> GetRequestAccessMappingDictionary(AccessRequest user)
        => new()
        {
            { "{{{EmailAddress}}}", user.EmailAddress },
            { "{{{FirstName}}}", user.FirstName },
            { "{{{LastName}}}", user.Surname },
            { "{{{PhoneNumber}}}", user.PhoneNumber},
        };

    private Dictionary<string, string> GetActivateUserMappingDictionary(UserModel user)
        => new()
        {
            { "{{{Id}}}", user.UserId },
            { "{{{FirstName}}}", user.FirstName },
            { "{{{LastName}}}", user.Surname }
        };
}