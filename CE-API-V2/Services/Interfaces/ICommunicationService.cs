using Azure.Communication.Email;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces
{
    /// <summary>
    /// Service carrying out the communication.
    /// </summary>
    public interface ICommunicationService
    {
        /// <summary>
        /// Sends the access request to the person in charge
        /// </summary>
        /// <param name="user">User model containing the information required for the access request.</param>
        /// <returns></returns>
        public Task<EmailSendStatus> SendAccessRequest(AccessRequest user);

        /// <summary>
        /// Sends the activation request to the person in charge
        /// </summary>
        /// <param name="user">User model containing the information required for the activation request.</param>
        /// <returns></returns>
        public Task<EmailSendStatus> SendActivationRequest(UserModel user);
    }
}
