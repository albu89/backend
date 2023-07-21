using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using Azure.Communication.Email;

namespace CE_API_V2.UnitOfWorks.Interfaces
{
    public interface IUserUOW
    {
        public Task StoreBiomarkerOrder(IEnumerable<BiomarkerOrderModel> biomarkerOrder);
        public Task StoreOrEditBiomarkerOrder(IEnumerable<BiomarkerOrderModel> biomarkerOrder, string userId);
        public void StoreBiomarkerOrderEntry(BiomarkerOrderModel biomarkerOrder);
        public void EditBiomarkerOrderEntry(BiomarkerOrderModel biomarkerOrder);
        public IEnumerable<BiomarkerSchemaDto> OrderTemplate(IEnumerable<BiomarkerSchemaDto> biomarkersSchema, string userId);
        public Task<EmailSendStatus> ProcessAccessRequest(AccessRequestDto userDto);
        public Task<User> StoreUser(User user);
        public User GetUser(string userId);
        IEnumerable<BiomarkerOrderModel> GetBiomarkerOrders(string userId);
    }
}
