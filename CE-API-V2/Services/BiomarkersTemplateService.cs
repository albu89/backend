using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;
using System.Text.Json;

namespace CE_API_V2.Services
{
    public class BiomarkersTemplateService : IBiomarkersTemplateService
    {
        public async Task<BiomarkersTemplateDTO> GetTemplate()
        {
            using StreamReader reader = new StreamReader("BiomarkersSchema.json");
            var json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var biomarkers = JsonSerializer.Deserialize<List<BiomarkerDTO>>(json, options);
            var template = new BiomarkersTemplateDTO() { BiomarkerList = biomarkers };

            return template;
        }
    }
}