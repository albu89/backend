using Microsoft.Extensions.Localization;

namespace CE_API_V2.Localization.JsonStringFactroy
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        public JsonStringLocalizerFactory()
        {
        }
        public IStringLocalizer Create(Type resourceSource) =>
            new JsonStringLocalizer();
        public IStringLocalizer Create(string baseName, string location) =>
            new JsonStringLocalizer();
    }
}
