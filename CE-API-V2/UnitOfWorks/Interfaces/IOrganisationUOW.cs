using CE_API_V2.Models;

namespace CE_API_V2.UnitOfWorks.Interfaces
{
    public interface IOrganisationUOW
    {
        public OrganizationModel? GetOrganisationWithTenantID(string tenantID);
    }
}
