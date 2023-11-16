using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Azure.Communication.Email;
using CE_API_V2.Data.Extensions;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Records;
using CE_API_V2.Utility;

namespace CE_API_V2.UnitOfWorks
{
    public class UserUOW : IUserUOW
    {
        private readonly CEContext _context;
        private IGenericRepository<UserModel> _userRepository;
        private IGenericRepository<BiomarkerOrderModel>? biomarkerOrderRepository;
        private readonly ICommunicationService _communicationService;
        private readonly IOrganisationUOW _organisationUOW;

        public UserUOW(CEContext context, ICommunicationService communicationService, IOrganisationUOW organisationUOW)
        {
            _context = context;
            _userRepository = new GenericRepository<UserModel>(_context);
            _communicationService = communicationService;
            _organisationUOW = organisationUOW;
        }

        public IGenericRepository<UserModel> UserRepository
        {
            get => _userRepository;
            internal set => _userRepository = value;
        }

        public IGenericRepository<BiomarkerOrderModel> BiomarkerOrderRepository
        {
            get
            {
                if (biomarkerOrderRepository == null)
                    this.biomarkerOrderRepository = new GenericRepository<BiomarkerOrderModel>(_context);
                return biomarkerOrderRepository;
            }
        }
        
        /// <summary>
        /// Order the schema of CAD Scoring Requests
        /// </summary>
        /// <remarks>
        /// Loads a users order and preferred units if provided, default ordering otherwise. Orders the MedicalHistory and LabResults in place.
        /// </remarks>
        /// <param name="biomarkersSchemas"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CadRequestSchema OrderTemplate(CadRequestSchema biomarkersSchemas, string userId = "")
        {
            // If userId is provided, load users ordering
            if (!string.IsNullOrEmpty(userId))
            {
                List<BiomarkerOrderModel> orders = BiomarkerOrderRepository.Get(e => e.UserId == userId).OrderBy(x => x.OrderNumber).ToList();
            
                // Set indices of MedicalHistory
                foreach (var entry in biomarkersSchemas.MedicalHistory)
                {
                    var orderEntry = orders.FirstOrDefault(x => x.BiomarkerId == entry.Id);
                    if (orderEntry is null)
                    {
                        continue;
                    }
                    entry.OrderIndex = orderEntry.OrderNumber;
                }
            
                // Set indices of LabResults
                foreach (var entry in biomarkersSchemas.LabResults)
                {
                    var orderEntry = orders.FirstOrDefault(x => x.BiomarkerId == entry.Id);
                    if (orderEntry is null)
                    {
                        continue;
                    }
                    entry.OrderIndex = orderEntry.OrderNumber;
                    entry.PreferredUnit = orderEntry.PreferredUnit;
                }    
            }

            biomarkersSchemas.MedicalHistory = biomarkersSchemas.MedicalHistory.OrderBy(x => x.OrderIndex).ToArray();
            biomarkersSchemas.LabResults = biomarkersSchemas.LabResults.OrderBy(x => x.OrderIndex).ToArray();
            
            return biomarkersSchemas;
        }

        public async Task StoreBiomarkerOrder(IEnumerable<BiomarkerOrderModel> biomarkerOrder)
        {
            foreach (var entry in biomarkerOrder)
            {
                StoreBiomarkerOrderEntry(entry);
            }
            await _context.SaveChangesAsync();
        }

        public void StoreBiomarkerOrderEntry(BiomarkerOrderModel biomarkerOrderEntry)
        {
            if (string.IsNullOrEmpty(biomarkerOrderEntry.BiomarkerId) || string.IsNullOrEmpty(biomarkerOrderEntry.PreferredUnit) || _context.IsOrderAttached(biomarkerOrderEntry))
                return;
            BiomarkerOrderRepository.Insert(biomarkerOrderEntry);
        }

        public async Task StoreOrEditBiomarkerOrder(IEnumerable<BiomarkerOrderModel> biomarkerOrders, string userId)
        {
            var allOrders = BiomarkerOrderRepository.Get(x => x.UserId == userId).Select(x => x.BiomarkerId).ToList();
            foreach (var entry in biomarkerOrders)
            {
                if (allOrders.Contains(entry.BiomarkerId))
                {

                    EditBiomarkerOrderEntry(entry);
                }
                else
                {
                    StoreBiomarkerOrderEntry(entry);
                }
            }

            await _context.SaveChangesAsync();
        }

        public void EditBiomarkerOrderEntry(BiomarkerOrderModel biomarkerOrderEntry)
        {
            try
            {
                if (!_context.IsOrderAttached(biomarkerOrderEntry))
                {
                    BiomarkerOrderRepository.Update(biomarkerOrderEntry);
                }
                else
                {
                    var marker = BiomarkerOrderRepository.Get(x => x.UserId == biomarkerOrderEntry.UserId && x.BiomarkerId == biomarkerOrderEntry.BiomarkerId).FirstOrDefault();
                    if (marker != null)
                    {
                        marker.OrderNumber = biomarkerOrderEntry.OrderNumber;
                        marker.PreferredUnit = biomarkerOrderEntry.PreferredUnit;
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw new NotImplementedException("Something went wrong while editing the biomarker ordering.");
            }
        }

        public bool CheckIfIsActiveStateIsModifiable(UserIdsRecord userInfo)
        {
            var orga = _organisationUOW.GetOrganisationWithTenantID(userInfo.TenantId);
            var countOfActiveUserInOrga = _userRepository.Get(x => x.TenantID == userInfo.TenantId && x.IsActive == true)
                                                                .Count();

            if (orga != null && orga.Userquota > countOfActiveUserInOrga)
                return true;
            else 
                return false;
        }

        public async Task<UserModel> StoreUser(UserModel userModel)
        {
            var storedUser = _userRepository.GetById(userModel.UserId);
            if (storedUser != null)
            {
                return storedUser;
            }

            try
            {
                UserRepository.Insert(userModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException("Something went wrong while storing the user data.");
            }

            storedUser = UserRepository.GetById(userModel.UserId);

            return storedUser;
        }

        public UserModel? GetUser(string userId, UserIdsRecord userInfo)
        {
            var user = UserRepository.GetById(userId);

            var isSystemAdmin = userInfo.Role is UserRole.SystemAdmin;
            var isTenantAdmin = userInfo.Role is UserRole.Admin && userInfo.TenantId == user.TenantID;
            var isSelf = userInfo.UserId == user?.UserId;

            if (!isSelf && !isTenantAdmin && !isSystemAdmin)
            {
                return null;
            }

            return user;
        }

        public async Task<EmailSendStatus> ProcessAccessRequest(AccessRequest access)
            => await _communicationService.SendAccessRequest(access);

        public async Task<EmailSendStatus> ProcessInactiveUserCreation(UserModel user)
            => await _communicationService.SendActivationRequest(user);

        public IEnumerable<BiomarkerOrderModel> GetBiomarkerOrders(string userId)
        {
            return BiomarkerOrderRepository.Get(x => x.UserId == userId);
        }

        public async Task<UserModel> UpdateUser(string userId, UserModel updatedUser, UserIdsRecord userInfo)
        {
            var storedUser = _userRepository.GetById(userId) ?? throw new KeyNotFoundException();

            // Modification is allowed if:
            // User modifies themselves
            // User is privileged and modifies a User of their own tenant
            var modifiesSelf = storedUser.UserId == userInfo.UserId;
            var isTenantAdmin = userInfo.Role is UserRole.Admin && storedUser.TenantID == userInfo.TenantId;
            var isSystemAdmin = userInfo.Role is UserRole.SystemAdmin;
            if (!modifiesSelf && !isTenantAdmin && !isSystemAdmin)
            {
                throw new Exception("You are not allowed to modify this user.");
            }
            
            var updatedUserModel = UserModelUpdater.UpdateUserModel(updatedUser, storedUser, out _);
            if (userInfo.Role is (UserRole.Admin or UserRole.SystemAdmin))
            {
                updatedUserModel = UserModelUpdater.UpdatePrivilegedData(updatedUser, storedUser, out _);
            }

            try
            {
                UserRepository.Update(updatedUserModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while updating the user data.");
            }

            return UserRepository.GetById(storedUser.UserId);
        }

        /// <summary>
        /// Returns all users belonging to the same administrative group as the provided user
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public IEnumerable<UserModel> GetUsersForAdmin(UserIdsRecord userInfo)
        {
            if (userInfo.Role is not (UserRole.Admin or UserRole.SystemAdmin))
            {
                return Enumerable.Empty<UserModel>();
            }

            if (userInfo.Role is UserRole.SystemAdmin)
            {
                return _userRepository.Get();
            }
            
            return _userRepository.Get(u => u.TenantID == userInfo.TenantId);
        }
    }
}
