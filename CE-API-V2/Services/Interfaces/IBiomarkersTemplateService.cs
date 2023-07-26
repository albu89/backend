using CE_API_V2.Constants;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces
{
    public interface IBiomarkersTemplateService
    {
        public Task<IEnumerable<BiomarkerSchema>> GetTemplate(string locale = LocalizationConstants.DefaultLocale);
        public Task<Dictionary<string, IEnumerable<BiomarkerSchema>>> GetAllTemplates();
    }
}