using CE_API_V2.Services.Interfaces;

namespace CE_API_V2.Services
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        private const string SubPath = @"Templates/";

        public string GetRequestBodyTemplate()
        {
            var path = Path.Combine(SubPath, "RequestAccessEmailBody.html");
            using StreamReader reader = new StreamReader(path);

            return reader.ReadToEnd();
        }
    }
}
