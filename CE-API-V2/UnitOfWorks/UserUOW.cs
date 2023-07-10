using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Models;
using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Azure.Communication.Email;

namespace CE_API_V2.UnitOfWorks
{
    public class UserUOW : IUserUOW
    {
        private readonly CEContext _context;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ICommunicationService _communicationService;

        public UserUOW(CEContext context, ICommunicationService communicationService)
        {
            _context = context;
            _userRepository = new GenericRepository<User>(_context);
            _communicationService = communicationService;
        }

        public IGenericRepository<User> UserRepository => _userRepository;

        public async Task<User> StoreUser(User user)
        {
            var storedUser = _userRepository.GetById(user.UserId);
            if (storedUser != null)
            {
                return storedUser;
            }
            
            try
            {
                UserRepository.Insert(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }

            storedUser = UserRepository.GetById(user.UserId);

            return storedUser;
        }

        public User GetUser(string userId)
        {
            var user = UserRepository.GetById(userId);

            return user;
        }

        public async Task<EmailSendStatus> ProcessAccessRequest(AccessRequestDto accessDto)
            => await _communicationService.SendAccessRequest(accessDto);
    }
}
