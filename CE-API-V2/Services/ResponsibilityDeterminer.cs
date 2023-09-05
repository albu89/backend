using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CE_API_V2.Services;

/// <inheritdoc/>
public class ResponsibilityDeterminer : IResponsibilityDeterminer
{
    private readonly IAdministrativeEntitiesUOW _administrativeEntitiesUow;
    private readonly IConfiguration _configuration;
    private readonly IEmailValidator _emailValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponsibilityDeterminer"/> class.
    /// </summary>
    /// <param name="administrativeEntitiesUow">Unit of work handling country related tasks.</param>
    /// <param name="configuration">The Configuration</param>
    public ResponsibilityDeterminer(IAdministrativeEntitiesUOW administrativeEntitiesUow, IConfiguration configuration, IEmailValidator emailValidator)
    {
        _administrativeEntitiesUow = administrativeEntitiesUow;
        _configuration = configuration;
        _emailValidator = emailValidator;
    }

    /// <inheritdoc/>
    public string GetEmailAddress(string country, string organizationName, bool wasCreatedExternally)
    {
        if (!wasCreatedExternally && TryGetOrganizationRelatedContact(organizationName, out var contact))
        {
            return contact;
        }

        if (TryGetCountryRelatedContact(country, out contact))
        {
            return contact;
        }

        var defaultEMailAddress = _configuration.GetValue<string>("ExplorisContactEMail");

        return defaultEMailAddress ?? string.Empty;
    }

    private bool TryGetOrganizationRelatedContact(string organizationName, out string contact)
    {
        contact = string.Empty;

        if (organizationName.IsNullOrEmpty())
        {
            return false;
        }

        var organization = _administrativeEntitiesUow.GetOrganizationByName(organizationName);
        
        if (organization == null || !_emailValidator.EmailAddressIsValid(organization?.ContactEmail))
        {
            return false;
        }

        contact = organization!.ContactEmail;

        return true;
    }

    private bool TryGetCountryRelatedContact(string countryName, out string contact)
    {
        contact = string.Empty;

        if (countryName.IsNullOrEmpty())
        {
            return false;
        }

        var country = _administrativeEntitiesUow.GetCountryByName(countryName);

        if (country == null || !_emailValidator.EmailAddressIsValid(country.ContactEmail))
        {
            return false;
        }

        contact = country.ContactEmail;

        return true;
    }
}