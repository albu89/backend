using System.Text.Json.Serialization;
namespace CE_API_V2.Models.DTO
{
    public class CreateCountry 
    {
        public string Name { get; set; }
        public string ContactEmail { get; set; }
    }
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }    
}
