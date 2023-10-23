using CE_API_V2.Models.DTO;
using System.Text.Json;
using System.Text.RegularExpressions;
using CE_API_V2.Services.Interfaces;
using AutoMapper;
using CE_API_V2.Constants;
using static System.String;

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

        public async Task<CadRequestSchema> GetTemplate(string locale = LocalizationConstants.DefaultLocale)
        {
            return await GetTemplateFromUrl(locale, Path.Combine(LocalizationConstants.TemplatesSubpath, Concat(GeneralBiomarkerSchemaFile, GeneralBiomarkersSchemaFileEnding)));
        }

        private async Task<CadRequestSchema> GetTemplateFromUrl(string locale, string filePath)
        {

            var generalBiomarkerSchema = await DeserializeSchema<CadRequestSchema>(filePath);

            var localizedSchemaFilePath = TryGetLocalizedFilePath(locale);
            var localizedBiomarkerSchema = await DeserializeSchema<BiomarkersLocalizedNew>(localizedSchemaFilePath);

            return CombineSchemas(generalBiomarkerSchema, localizedBiomarkerSchema);
        }

        public async Task<Dictionary<string, CadRequestSchema>> GetAllTemplates()
        {
            var result = new Dictionary<string, CadRequestSchema>();
            var defaultTemplatePath = Path.Combine(LocalizationConstants.TemplatesSubpath, Concat(GeneralBiomarkerSchemaFile, GeneralBiomarkersSchemaFileEnding));
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

        private CadRequestSchema CombineSchemas(CadRequestSchema generalBiomarkerSchema, BiomarkersLocalizedNew localizedBiomarkerSchema)
        {
            // Map translations for MedicalHistory-Entries
            foreach (var entry in generalBiomarkerSchema.MedicalHistory)
            {
                var localizedEntry = localizedBiomarkerSchema.MedicalHistory.FirstOrDefault(x => x.Id == entry.Id);
                if (localizedEntry is null)
                {
                    throw new LocalizationMissingException(localizedEntry.Id);
                }
                entry.DisplayName = localizedEntry.DisplayName;
                entry.InfoText = localizedEntry.InfoText;
                entry.Category = !IsNullOrEmpty(localizedEntry.Category) ? localizedEntry.Category : entry.Category;
                if (localizedEntry.Unit?.Options is null || entry.Unit?.Options is null)
                {
                    continue;
                }
                var localizedOptions = localizedEntry.Unit.Options;
                foreach (var option in entry.Unit.Options)
                {
                    option.DisplayName = localizedOptions.FirstOrDefault(o => string.Equals(o.Value, option.Value, StringComparison.InvariantCultureIgnoreCase))?.DisplayName ?? option.Value;
                }
            }

            // Map translations 
            foreach (var entry in generalBiomarkerSchema.LabResults)
            {
                var localizedEntry = localizedBiomarkerSchema.LabResults.FirstOrDefault(x => x.Id == entry.Id);
                if (localizedEntry is null)
                {
                    throw new LocalizationMissingException(localizedEntry.Id);
                }
                entry.DisplayName = localizedEntry.DisplayName;
                entry.InfoText = localizedEntry.InfoText;
                entry.Category = !IsNullOrEmpty(localizedEntry.Category) ? localizedEntry.Category : entry.Category;
            }

            return generalBiomarkerSchema;
        }

        private static string TryGetLocalizedFilePath(string locale)
        {
            var path = Path.Combine(LocalizationConstants.TemplatesSubpath, Concat("BiomarkersSchema.info.", locale, ".json"));

            if (File.Exists(path))
                return path;

            path = Path.Combine(LocalizationConstants.TemplatesSubpath, Concat("BiomarkersSchema.info.", LocalizationConstants.DefaultLocale, ".json"));
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The requested file was not found. Requested File: {path}");
            }

            return path;
        }
    }

    internal class LocalizationMissingException : Exception
    {
        public LocalizationMissingException(string localizedEntryId) : base(FormatForBiomarker(localizedEntryId)) { }
        public LocalizationMissingException(string localizedEntryId, string property) : base(FormatForBiomarkerProperty(localizedEntryId, property)) { }

        static string FormatForBiomarker(string localizedEntryId)
        {
            return $"Biomarker {localizedEntryId} is missing a translation.";
        }
        static string FormatForBiomarkerProperty(string localizedEntryId, string property)
        {
            return $"Biomarker-property {localizedEntryId}.{property} is missing a translation.";
        }
    }
}