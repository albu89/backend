using CE_API_V2.Models.DTO;
using System.Text.Json;
using System.Text.RegularExpressions;
using CE_API_V2.Services.Interfaces;
using AutoMapper;
using CE_API_V2.Constants;

namespace CE_API_V2.Services
{
    public class BiomarkersTemplateService : IBiomarkersTemplateService
    {
        
        private const string GeneralBiomarkerSchemaFile = "BiomarkersSchema";
        private const string GeneralBiomarkerSchemaInfoFile = "BiomarkersSchema.info";
        private const string GeneralBiomarkersSchemaFileEnding = ".json";

        private readonly IMapper _mapper;

        public BiomarkersTemplateService(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<BiomarkerSchema>> GetTemplate(string locale = LocalizationConstants.DefaultLocale)
        {
            return await GetTemplateFromUrl(locale, Path.Combine(LocalizationConstants.TemplatesSubpath, string.Concat(GeneralBiomarkerSchemaFile, GeneralBiomarkersSchemaFileEnding)));
        }
        private async Task<IEnumerable<BiomarkerSchema>> GetTemplateFromUrl(string locale, string filePath)
        {

            var generalBiomarkerSchema = await DeserializeSchema<List<BiomarkersGeneral>>(filePath);

            var localizedSchemaFilePath = TryGetLocalizedFilePath(locale);
            var localizedBiomarkerSchema = await DeserializeSchema<List<BiomarkersLocalized>>(localizedSchemaFilePath);

            return CombineSchemas(generalBiomarkerSchema, localizedBiomarkerSchema);
        }
        public async Task<Dictionary<string, IEnumerable<BiomarkerSchema>>> GetAllTemplates()
        {
            var result = new Dictionary<string, IEnumerable<BiomarkerSchema>>();
            var defaultTemplatePath = Path.Combine(LocalizationConstants.TemplatesSubpath, string.Concat(GeneralBiomarkerSchemaFile, GeneralBiomarkersSchemaFileEnding));
            var allFiles = Directory.GetFiles(LocalizationConstants.TemplatesSubpath).Where(x => x.Contains(GeneralBiomarkerSchemaInfoFile));
            foreach (var file in allFiles)
            {
                var locale = Regex.Match(file, $"{GeneralBiomarkerSchemaInfoFile}\\.(.*)\\.json").Groups.Values.FirstOrDefault().Value;
                var schema = await GetTemplateFromUrl(locale, defaultTemplatePath);
                result.Add(locale, schema);
            }
            return result;
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

        private List<BiomarkerSchema> CombineSchemas(List<BiomarkersGeneral> generalBiomarkerSchema, List<BiomarkersLocalized> localizedBiomarkerSchema)
        {

            var biomarkerSchemaList  = new List<BiomarkerSchema>();

            foreach (var item in generalBiomarkerSchema)
            {
                var langSpec = localizedBiomarkerSchema.Find(x => x.Id.Equals(item.Id));
                var biomarkerSchemaDto = _mapper.Map<BiomarkerSchema>(item);
                _mapper.Map(langSpec, biomarkerSchemaDto);

                if (langSpec.DisplayNames is not null)
                {
                    foreach (var unit in biomarkerSchemaDto.Units)
                    {
                        unit.DisplayNames = langSpec.DisplayNames.Where(x => unit.Enum.Contains(x.Key)).ToDictionary(i => i.Key, i => i.Value);
                    }
                }

                biomarkerSchemaList.Add(biomarkerSchemaDto);
            }
            
            return biomarkerSchemaList;
        }

        private static string TryGetLocalizedFilePath(string locale)
        {
            var path = Path.Combine(LocalizationConstants.TemplatesSubpath, string.Concat("BiomarkersSchema.info.",locale, ".json"));

            if (File.Exists(path))
                return path;
            
            path = Path.Combine(LocalizationConstants.TemplatesSubpath, string.Concat("BiomarkersSchema.info.", LocalizationConstants.DefaultLocale, ".json"));
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The requested file was not found. Requested File: {path}");
            }

            return path;
        }
    }
}