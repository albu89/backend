using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces
{
    public interface IBiomarkersTemplateService
    {
        public Task<IEnumerable<BiomarkerSchemaDto>> GetTemplate();
    }
}