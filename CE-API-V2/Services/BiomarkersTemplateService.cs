using CE_API_V2.Models.DTO;
using System.Text.Json;
using CE_API_V2.Services.Interfaces;

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