using Azure;
using Azure.Communication.Email;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Models.Records;

namespace CE_API_V2.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IEmailBuilder _emailBuilder;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailClientService _emailClientService;
    private readonly IConfiguration _config;
    private static string MailRecipient;

    public CommunicationService(IEmailBuilder emailBuilder, IEmailClientService emailClientService, IWebHostEnvironment env, IConfiguration config)
    {
        _emailBuilder = emailBuilder;
        _emailClientService = emailClientService;
        _env = env;
        _config = config;
        MailRecipient = _config.GetValue<string>("AzureCommunicationService:MailRecipient");
    }

    public async Task<EmailSendStatus> SendAccessRequest(AccessRequest access)
    {
        var emailConfiguration = _emailBuilder.GetEmailConfiguration(access);
        var emailClient = _emailClientService.GetEmailClient();
        var emailResult = await SendEMail(emailClient, emailConfiguration);

        return emailResult;
    }

    private async Task<EmailSendStatus> SendEMail(EmailClient emailClient, EMailConfiguration emailConfig, WaitUntil waituntil = WaitUntil.Completed)
    {
        try
        {
            if (_env.IsDevelopment())
            {
                return EmailSendStatus.Succeeded;
            }

            EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                waituntil,
                emailConfig.Sender,
                MailRecipient,
                emailConfig.Subject,
                emailConfig.HtmlContent,
                null,
                CancellationToken.None);
            
            EmailSendResult statusMonitor = emailSendOperation.Value;

            if (statusMonitor is null)
            {
                return EmailSendStatus.Failed;
            }

            return statusMonitor.Status;
        }
        catch (RequestFailedException ex)
        {
            throw new NotImplementedException();
        }
    }
}