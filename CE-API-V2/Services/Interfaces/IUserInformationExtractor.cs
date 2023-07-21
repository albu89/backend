using System.Security.Claims;
using CE_API_V2.Models.Records;

namespace CE_API_V2.Services.Interfaces;

public interface IUserInformationExtractor
{
    public UserIdsRecord GetUserIdInformation(ClaimsPrincipal user);
}