using CE_API_V2.Models;

namespace CE_API_V2.UnitOfWorks.Interfaces;

/// <summary>
/// Class to handle database interactions.
/// </summary>
public interface IAdministrativeEntitiesUOW
{
    /// <summary>
    /// Gets a list of the countries from the database.
    /// </summary>
    /// <returns>IEnumerable of country.</returns>
    public IEnumerable<CountryModel> GetCountries();

    /// <summary>
    /// Gets a list of the organizations from the database.
    /// </summary>
    /// <returns>IEnumerable of organization model.</returns>
    public IEnumerable<OrganizationModel> GetOrganizations();

    /// <summary>
    /// Adds a country to the database.
    /// </summary>
    /// <param name="country">The country to be added.</param>
    /// <returns>The added country model if the operation was successful.</returns>
    public CountryModel? AddCountry(CountryModel country);

    /// <summary>
    /// Adds a organization to the database.
    /// </summary>
    /// <param name="organization">The country to be added.</param>
    /// <returns>The added organization model if the operation was successful.</returns>
    public OrganizationModel? AddOrganizations(OrganizationModel organization);

    /// <summary>
    /// Updates a country if it is present in the database.
    /// </summary>
    /// <param name="updatedCountry">The updated country.</param>
    /// <returns>The updated country model.</returns>
    public CountryModel? UpdateCountry(CountryModel updatedCountry);

    /// <summary>
    /// Updates an organization if it is present in the database.
    /// </summary>
    /// <param name="organization">The updated organization.</param>
    /// <returns>The updated organization model.</returns>
    public OrganizationModel? UpdateOrganization(OrganizationModel organization);

    /// <summary>
    /// Removes a country from the database.
    /// </summary>
    /// <param name="id">The id of the country.</param>
    /// <returns>The removed country model.</returns>
    public CountryModel? DeleteCountry(Guid id);

    /// <summary>
    /// Removes an organization from the database.
    /// </summary>
    /// <param name="id">The id of the organization.</param>
    /// <returns>The removed organization model.</returns>
    public OrganizationModel? DeleteOrganization(Guid id);

    /// <summary>
    /// Gets a country by its name.
    /// </summary>
    /// <param name="name">The name of the country.</param>
    /// <returns>The country model.</returns>
    public CountryModel? GetCountryByName(string name);

    /// <summary>
    /// Gets an organization by its name.
    /// </summary>
    /// <param name="name">The name of the organization</param>
    /// <returns>The organization model.</returns>
    public OrganizationModel? GetOrganizationByName(string name);
}