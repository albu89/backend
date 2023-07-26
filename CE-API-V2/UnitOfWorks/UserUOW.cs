using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Azure.Communication.Email;
using CE_API_V2.Data.Extensions;

namespace CE_API_V2.UnitOfWorks
{
    public class UserUOW : IUserUOW
    {
        private readonly CEContext _context;
        private readonly IGenericRepository<UserModel> _userRepository;
        private IGenericRepository<BiomarkerOrderModel>? biomarkerOrderRepository;
        private readonly ICommunicationService _communicationService;

        public UserUOW(CEContext context, ICommunicationService communicationService)
        {
            _context = context;
            _userRepository = new GenericRepository<UserModel>(_context);
            _communicationService = communicationService;
        }

        public IGenericRepository<UserModel> UserRepository => _userRepository;

        public IGenericRepository<BiomarkerOrderModel> BiomarkerOrderRepository
        {
            get
            {
                if (biomarkerOrderRepository == null)
                    this.biomarkerOrderRepository = new GenericRepository<BiomarkerOrderModel>(_context);
                return biomarkerOrderRepository;
            }
        }

        public IEnumerable<BiomarkerSchema> OrderTemplate(IEnumerable<BiomarkerSchema> biomarkersSchemas, string userId)
        {
            List<BiomarkerOrderModel> orders = BiomarkerOrderRepository.Get(e => e.UserId == userId).OrderBy(x => x.OrderNumber).ToList();

            var sortedList = new List<BiomarkerSchema>();
            foreach (var entry in orders)
            {
                var schemaEntry = biomarkersSchemas.First(x => x.Id == entry.BiomarkerId);
                schemaEntry.OrderNumber = entry.OrderNumber;
                schemaEntry.PreferredUnit = entry.PreferredUnit;
                sortedList.Add(schemaEntry);
            }

            var exceptList = biomarkersSchemas.Except(sortedList).OrderBy(x => x.OrderNumber).ToList();
            sortedList.AddRange(exceptList);

            var orderNumber = 0;

            foreach (var entry in sortedList)
            {
                entry.OrderNumber = orderNumber;
                orderNumber++;
            }

            return biomarkersSchemas.OrderBy(x => x.OrderNumber);
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
                throw new NotImplementedException();
            }
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
                throw new NotImplementedException();
            }

            storedUser = UserRepository.GetById(userModel.UserId);

            return storedUser;
        }

        public UserModel GetUser(string userId)
        {
            var user = UserRepository.GetById(userId);

            return user;
        }

        public async Task<EmailSendStatus> ProcessAccessRequest(AccessRequest access)
            => await _communicationService.SendAccessRequest(access);

        public IEnumerable<BiomarkerOrderModel> GetBiomarkerOrders(string userId)
        {
            return BiomarkerOrderRepository.Get(x => x.UserId == userId);
        }
    }
}
