using CE_API_V2.Constants;
using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Services
{
    /// <inheritdoc/>
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        /// <inheritdoc/>
        public string GetRequestBodyTemplate()
        {
            var path = Path.Combine(LocalizationConstants.TemplatesSubpath, "RequestAccessEmailBody.html");
            using StreamReader reader = new StreamReader(path);

            return reader.ReadToEnd();
        }

        /// <inheritdoc/>
        public string GetActivateUserBodyTemplate()
        {
            var path = Path.Combine(LocalizationConstants.TemplatesSubpath, "ActivateUserEmailBody.html");
            using StreamReader reader = new StreamReader(path);

            return reader.ReadToEnd();
        }
    }
}
