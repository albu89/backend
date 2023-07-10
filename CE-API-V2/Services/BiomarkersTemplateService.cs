using CE_API_V2.Models.DTO;
using System.Text.Json;
using CE_API_V2.Services.Interfaces;
using AutoMapper;
using CE_API_V2.Constants;

namespace CE_API_V2.Services
{
    public class BiomarkersTemplateService : IBiomarkersTemplateService
    {
        private const string GeneralBiomarkerSchemaFile = "BiomarkersSchema.json";

        private readonly IMapper _mapper;

        public BiomarkersTemplateService(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<BiomarkerSchemaDto>> GetTemplate(string locale = LocalizationConstants.DefaultLocale)
        {
            var generalBiomarkerSchema = await DeserializeSchema<List<BiomarkersGeneral>>(GeneralBiomarkerSchemaFile);

            var localizedSchemaFilePath = TryGetLocalizedFilePath(locale);
            var localizedBiomarkerSchema = await DeserializeSchema<List<BiomarkersLocalized>>(localizedSchemaFilePath);

            return CombineSchemas(generalBiomarkerSchema, localizedBiomarkerSchema);
        }

        private async Task<T?> DeserializeSchema<T>(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            var file = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var deserialized = JsonSerializer.Deserialize<T>(file, options);

            return deserialized;
        }

        private List<BiomarkerSchemaDto> CombineSchemas(List<BiomarkersGeneral> generalBiomarkerSchema, List<BiomarkersLocalized> localizedBiomarkerSchema)
        {

            var biomarkerSchemaList  = new List<BiomarkerSchemaDto>();

            foreach (var item in generalBiomarkerSchema)
            {
                var langSpec = localizedBiomarkerSchema.Find(x => x.Id.Equals(item.Id));
                var biomarkerSchemaDto = _mapper.Map<BiomarkerSchemaDto>(item);
                _mapper.Map(langSpec, biomarkerSchemaDto);

                biomarkerSchemaList.Add(biomarkerSchemaDto);
            }
            
            return biomarkerSchemaList;
        }

        private static string TryGetLocalizedFilePath(string locale)
        {
            var path = string.Concat("BiomarkersSchema.info.",locale, ".json");

            if (File.Exists(path))
                return path;
            
            path = string.Concat("BiomarkersSchema.info.", LocalizationConstants.DefaultLocale, ".json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The requested file was not found. Requested File: {path}");
            }

            return path;
        }
    }
}