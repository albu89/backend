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
        private readonly IMapper _mapper;
        private ICommunicationService _comunicationService;

        public UserUOW(CEContext context, IMapper mapper, ICommunicationService comunicationService)
        {
            _context = context;
            _userRepository = new GenericRepository<User>(_context);
            _mapper = mapper;
            _comunicationService = comunicationService;
        }

        public IGenericRepository<User> UserRepository => _userRepository;

        public async Task<User> StoreUser(CreateUserDto userDto, UserIdsRecord userInformation)
        {
            var user = MapToUserModel(userDto, userInformation);
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

            storedUser = UserRepository.GetById(userInformation.UserId);

            return storedUser;
        }
        
        public async Task<EmailSendStatus> ProcessAccessRequest(AccessRequestDto accessDto)
            => await _comunicationService.SendAccessRequest(accessDto);

        private User MapToUserModel(CreateUserDto userDto, UserIdsRecord userInformation)
        {
            var user = _mapper.Map<CreateUserDto, User>(userDto); 
            user.Role = UserRole.MedicalDoctor; 
            user.UserId = userInformation.UserId;
            user.TenantID = userInformation.TenantId;

            return user;
        }
    }
}
