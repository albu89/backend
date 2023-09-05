using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Records;

namespace CE_API_V2.Services.Interfaces;

/// <summary>
/// Helper class to set up the email configuration.
/// </summary>
public interface IEmailBuilder
{
    /// <summary>
    /// Gets the email configuration for the access request.
    /// </summary>
    /// <param name="user">The user model containing the required information for the access request.</param>
    /// <returns>THe email configuration.</returns>
    public EMailConfiguration GetRequestAccessEmailConfiguration(AccessRequest user);

    /// <summary>
    /// Gets the email configuration for the activate user request.
    /// </summary>
    /// <param name="user">The user model containing the required information for the activate user request.</param>
    /// <returns>THe email configuration.</returns>
    public EMailConfiguration GetActivateUserEmailConfiguration(UserModel user);
}
