using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using System.Security.Claims;
using Microsoft.Identity.Web;

namespace CE_API_V2.Services
{
    public class UserInformationExtractor : IUserInformationExtractor
    {
        public UserIdsRecord GetUserIdInformation(ClaimsPrincipal? user)
        {
            var userId = GetClaimValue(user, ClaimTypes.NameIdentifier);
            var tenantId = GetClaimValue(user, ClaimConstants.TenantId); 

            return new UserIdsRecord()
            {
                UserId = userId,
                TenantId = tenantId,
            };
        }

        private string GetClaimValue(ClaimsPrincipal claimsPrincipal, string value)
        {
            var extractedValue = claimsPrincipal?.Claims?.Any() == true
                ? claimsPrincipal.FindFirstValue(value)
                : "anonymous";
            extractedValue ??= "anonymous";

            return extractedValue;
        }
    }
}
