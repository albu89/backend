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

    public UserModel MapToUserModel(CreateUser user, UserIdsRecord userInformation)
    {
        var userModel = _mapper.Map<CreateUser, UserModel>(user); 
        userModel.Role = UserRole.MedicalDoctor; 
        userModel.UserId = userInformation.UserId;
        userModel.TenantID = userInformation.TenantId;

        return userModel;
    }
}