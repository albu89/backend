using AutoMapper;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Records;
namespace CE_API_V2.Utility;

public class UserHelper
{
    private readonly IMapper _mapper;

    public UserHelper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public User MapToUserModel(CreateUserDto userDto, UserIdsRecord userInformation)
    {
        var user = _mapper.Map<CreateUserDto, User>(userDto); 
        user.Role = UserRole.MedicalDoctor; 
        user.UserId = userInformation.UserId;
        user.TenantID = userInformation.TenantId;

        return user;
    }
}