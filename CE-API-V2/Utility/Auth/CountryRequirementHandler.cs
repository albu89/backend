using System.Security.Claims;
using CE_API_V2.Models.Enum;
using Microsoft.AspNetCore.Authorization;

namespace CE_API_V2.Utility.Auth
{
    public class CountryRequirementHandler : AuthorizationHandler<CountryRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CountryRequirement requirement)
        {
            var country = context.User.FindFirst(c => c.Type == "ctry")?.Value ?? "??";
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value ?? UserRole.User.ToString();

            if (userRole.ToLowerInvariant() == ("CE." + UserRole.SystemAdmin).ToLowerInvariant())
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            if (country.ToLowerInvariant() == requirement.RequiredCountry.ToLowerInvariant())
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var reason = new AuthorizationFailureReason(this, "Wrong Country. You are not allowed to access this country's API instance.");
            context.Fail(reason);
            return Task.CompletedTask;
        }
    }
}