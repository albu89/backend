using Azure.Communication.Email;
using Azure;
using CE_API_V2.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CE_API_V2.Services
{
    public class EmailClientService : IEmailClientService
    {
        private readonly IConfiguration _config;

        public EmailClientService(IConfiguration config)
        {
            _config = config;
        }

        public EmailClient GetEmailClient()
        {
            var connectionSettings = GetConnectionSettings();

            return new EmailClient(connectionSettings.Endpoint, connectionSettings.KeyCredential);
        }

        private AzureConnectionSettings GetConnectionSettings()
        {
            var section = _config.GetSection("AzureCommunicationService");
            var endPoint = section["Endpoint"];
            var keyCredential = section["KeyCredential"];

            if (endPoint.IsNullOrEmpty() || keyCredential.IsNullOrEmpty())
            {
                return null;
            }

            return new()
            {
                Endpoint = new Uri(endPoint),
                KeyCredential = new AzureKeyCredential(keyCredential),
            };
        }

        private record AzureConnectionSettings
        {
            public AzureKeyCredential KeyCredential { get; set; }
            public Uri Endpoint { get; set; }
        }
    }
}
