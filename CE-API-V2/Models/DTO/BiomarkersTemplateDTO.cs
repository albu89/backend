namespace CE_API_V2.Models.DTO
{
    public class BiomarkersTemplateDTO
    {
        public IEnumerable<BiomarkerDTO> BiomarkerList { get; init; } = new List<BiomarkerDTO>();
    }
}
