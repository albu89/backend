using Azure.Communication.Email;
using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Records;

namespace CE_API_V2.UnitOfWorks.Interfaces;

public interface IUserUOW
{
    public IGenericRepository<User> UserRepository { get; }
    public Task<EmailSendStatus> ProcessAccessRequest(AccessRequestDto userDto);
    public Task<User> StoreUser(CreateUserDto user, UserIdsRecord userIds);
}