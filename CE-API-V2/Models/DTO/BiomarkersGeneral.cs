namespace CE_API_V2.Models.DTO
{
    public class BiomarkersGeneral
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public BiomarkerSchemaUnitDto[] Units { get; set; }
        public int OrderNumber { get; set; }
        public string PreferredUnit { get; set; }
    }
}
