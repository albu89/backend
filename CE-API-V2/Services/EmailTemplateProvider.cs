using CE_API_V2.Constants;
using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Services
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        public string GetRequestBodyTemplate()
        {
            var path = Path.Combine(LocalizationConstants.TemplatesSubpath, "RequestAccessEmailBody.html");
            using StreamReader reader = new StreamReader(path);

            return reader.ReadToEnd();
        }
    }
}
