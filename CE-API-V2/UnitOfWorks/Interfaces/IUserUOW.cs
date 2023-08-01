using Azure.Communication.Email;
using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.UnitOfWorks.Interfaces
{
    public interface IUserUOW
    {
        public Task StoreBiomarkerOrder(IEnumerable<BiomarkerOrderModel> biomarkerOrder);
        public Task StoreOrEditBiomarkerOrder(IEnumerable<BiomarkerOrderModel> biomarkerOrder, string userId);
        public void StoreBiomarkerOrderEntry(BiomarkerOrderModel biomarkerOrder);
        public void EditBiomarkerOrderEntry(BiomarkerOrderModel biomarkerOrder);
        public IEnumerable<BiomarkerSchema> OrderTemplate(IEnumerable<BiomarkerSchema> biomarkersSchema, string userId);
        public UserModel GetUser(string userId);
        IEnumerable<BiomarkerOrderModel> GetBiomarkerOrders(string userId);
        public IGenericRepository<UserModel> UserRepository { get; }
        public Task<EmailSendStatus> ProcessAccessRequest(AccessRequest user);
        public Task<UserModel> StoreUser(UserModel userModel);
        public Task<UserModel> UpdateUser(string userId, UserModel userModel);
    }
}