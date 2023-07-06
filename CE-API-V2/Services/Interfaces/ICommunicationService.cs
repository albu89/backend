using Azure.Communication.Email;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces
{
    public interface ICommunicationService
    {
        public Task<EmailSendStatus> SendAccessRequest(AccessRequestDto userDto);
    }
}
