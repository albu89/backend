using System.Text.Json.Serialization;

namespace CE_API_V2.Models
{
    public class BiomarkerOrderModel
    {
        public string UserId { get; set; }
        public string BiomarkerId { get; set; }
        public int OrderNumber { get; set; }
        public string PreferredUnit { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }
    }
}
