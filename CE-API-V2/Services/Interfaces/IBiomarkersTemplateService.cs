using CE_API_V2.Models.DTO;

namespace CE_API_V2.Utility
{
    public interface IBiomarkersTemplateService
    {
        public Task<BiomarkersTemplateDTO> GetTemplate();
    }
}