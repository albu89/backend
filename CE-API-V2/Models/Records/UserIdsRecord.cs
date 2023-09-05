using CE_API_V2.Models.Enum;
namespace CE_API_V2.Models.Records
{
    public class UserIdsRecord
    {
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public UserRole Role { get; set; }
    }
}
