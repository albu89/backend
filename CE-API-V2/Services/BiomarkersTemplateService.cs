using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;
using System.Text.Json;

namespace CE_API_V2.Services
{
    public class BiomarkersTemplateService : IBiomarkersTemplateService
    {
        public async Task<IEnumerable<BiomarkerSchemaDto>> GetTemplate()
        {
            using StreamReader reader = new StreamReader("BiomarkersSchema.json");
            var json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Deserialize<List<BiomarkerSchemaDto>>(json, options);
        }
    }
}