using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Records;

namespace CE_API_V2.Services.Interfaces;

public interface IEmailBuilder
{
    public EMailConfiguration GetEmailConfiguration(AccessRequest user);
}
