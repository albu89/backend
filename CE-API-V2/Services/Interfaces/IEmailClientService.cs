using Azure.Communication.Email;

namespace CE_API_V2.Services.Interfaces
{
    public interface IEmailClientService
    {
        public EmailClient GetEmailClient();
    }
}
