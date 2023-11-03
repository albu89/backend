using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CE_API_V2.UnitOfWorks;

///<inheritdoc/>
public class AdministrativeEntitiesUOW : IAdministrativeEntitiesUOW
{
    private readonly CEContext _context;
    private readonly IGenericRepository<CountryModel> _countryRepository;
    private readonly IGenericRepository<OrganizationModel> _organizationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdministrativeEntitiesUOW" />
    /// </summary>
    /// <param name="context">The context.</param>
    public AdministrativeEntitiesUOW(CEContext context)
    {
        _context = context;
        _countryRepository = new GenericRepository<CountryModel>(_context);
        _organizationRepository = new GenericRepository<OrganizationModel>(_context);
    }

    /// <summary>
    /// The country repository.
    /// </summary>
    public IGenericRepository<CountryModel> CountryRepository => _countryRepository;

    ///<inheritdoc/>
    public IEnumerable<CountryModel> GetCountries() => _countryRepository.Get();

    ///<inheritdoc/>
    public IEnumerable<OrganizationModel> GetOrganizations() => _organizationRepository.Get();

    ///<inheritdoc/>
    public CountryModel? AddCountry(CountryModel country)
    {
        if (_countryRepository.Get().Any(o => o.Name == country.Name))
        {
            throw new Exception("Country is already present");
        }

        _countryRepository.Insert(country);
        _context.SaveChanges();

        return _countryRepository.Get().FirstOrDefault(o => o.Name == country.Name);
    }

    ///<inheritdoc/>
    public OrganizationModel? AddOrganizations(OrganizationModel organization)
    {
        if (_organizationRepository.Get().Any(o => o.Name == organization.Name))
        {
            throw new Exception("Organization already present");
        }

        _organizationRepository.Insert(organization);
        _context.SaveChanges();

        return _organizationRepository.Get().FirstOrDefault(o => o.Name == organization.Name);
    }

    ///<inheritdoc/>
    public CountryModel? UpdateCountry(CountryModel updatedCountry)
    {
        CountryModel? updatedEntity = null;

        try
        {
            updatedEntity = _countryRepository.Update(updatedCountry);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw new NotImplementedException();
        }

        return updatedEntity;
    }

    ///<inheritdoc/>
    public OrganizationModel? UpdateOrganization(OrganizationModel organization)
    {
        OrganizationModel? updatedEntity = null;

        try
        {
            updatedEntity = _organizationRepository.Update(organization);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw new NotImplementedException();
        }
        
        return updatedEntity;
    }

    ///<inheritdoc/>
    public CountryModel? DeleteCountry(Guid id)
    {
        var country = _countryRepository.Get().FirstOrDefault(o => o.Id == id);

        if (country == null)
        {
            throw new NotImplementedException();
        }

        try
        {
            var deletedOrganization = _countryRepository.Delete(country);
            _context.SaveChangesAsync();

            return deletedOrganization;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new NotImplementedException("Error deleting an Organization");
        }
    }

    ///<inheritdoc/>
    public OrganizationModel? DeleteOrganization(Guid id)
    {
        var organization = _organizationRepository.Get().FirstOrDefault(o => o.Id == id) ?? throw new NotImplementedException();
        try
        {
            var deletedOrganization = _organizationRepository.Delete(organization);
            _context.SaveChangesAsync();

            return deletedOrganization;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new NotImplementedException("Error deleting an Organization");
        }
    }

    ///<inheritdoc/>
    public CountryModel? GetCountryByName(string name)
    {
        var country = _countryRepository.Get().FirstOrDefault(o => o.Name == name);
        
        return country;
    }

    ///<inheritdoc/>
    public OrganizationModel? GetOrganizationByName(string name)
    {
        var organization = _organizationRepository.Get().FirstOrDefault(o => o.Name == name);

        return organization;
    }
}