using System.Security.Claims;
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
        userModel.Role = UserRole.User; 
        userModel.UserId = userInformation.UserId;
        userModel.TenantID = userInformation.TenantId;

        return userModel;
    }
    
    
    public static string GetUserId(ClaimsPrincipal user)
    {
        var userId = user?.Claims?.Any() == true ? user.FindFirstValue(ClaimTypes.NameIdentifier) : "anonymous";
        userId ??= "anonymous";
        return userId;
    }
}