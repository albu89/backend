using Microsoft.AspNetCore.Authorization;

namespace CE_API_V2.Utility.Auth
{
    public class CountryRequirement : IAuthorizationRequirement
    {
        public CountryRequirement(string country) => RequiredCountry  = country;

        public string RequiredCountry { get; set;}
    }
}