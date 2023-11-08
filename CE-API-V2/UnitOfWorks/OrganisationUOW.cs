using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.UnitOfWorks.Interfaces;

namespace CE_API_V2.UnitOfWorks
{
    public class OrganisationUOW : IOrganisationUOW
    {
        private IGenericRepository<OrganizationModel>? _organisationRepository;
        public IGenericRepository<OrganizationModel> OrganisationRepository => _organisationRepository;

        public OrganisationUOW(CEContext context)
        {  
            _organisationRepository = new GenericRepository<OrganizationModel>(context);
        }

        public OrganizationModel? GetOrganisationWithTenantID(string tenantID) => OrganisationRepository.Get(x => x.TenantId == tenantID).FirstOrDefault();
    }
}
