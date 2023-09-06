using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using System.Security.Claims;
using System.Text.RegularExpressions;
using CE_API_V2.Models.Enum;
using Microsoft.Identity.Web;

namespace CE_API_V2.Services
{
    public partial class UserInformationExtractor : IUserInformationExtractor
    {
        public UserIdsRecord GetUserIdInformation(ClaimsPrincipal user)
        {
            var userId = GetClaimValue(user, ClaimTypes.NameIdentifier);
            var tenantId = GetClaimValue(user, ClaimConstants.TenantId);
            var homeTenantId = ExtractTenantIdFromIdentityProvider(GetClaimValue(user, "http://schemas.microsoft.com/identity/claims/identityprovider"));
            var roleClaim = GetClaimValue(user, ClaimTypes.Role);
            roleClaim = roleClaim.Replace("CE.", "");
            var roleParsed = Enum.TryParse<UserRole>(roleClaim, true, out var role);

            return new UserIdsRecord()
            {
                UserId = userId,
                TenantId = string.IsNullOrEmpty(homeTenantId) ? tenantId : homeTenantId,
                Role = roleParsed ? role : UserRole.User
            };
        }
        private static string ExtractTenantIdFromIdentityProvider(string identityProvider)
        {
            Regex guidRegex = GuidRegex();
            var match = guidRegex.Match(identityProvider);
            return !match.Success ? "" : match.Value;
        }

        private static string GetClaimValue(ClaimsPrincipal claimsPrincipal, string value)
        {
            var extractedValue = claimsPrincipal?.Claims?.Any() == true
                ? claimsPrincipal.FindFirstValue(value)
                : "anonymous";
            extractedValue ??= "anonymous";

            return extractedValue;
        }

        [GeneratedRegex("[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?")]
        private static partial Regex GuidRegex();
    }
}