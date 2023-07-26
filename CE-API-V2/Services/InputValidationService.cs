using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Validators;
using FluentValidation.Results;

namespace CE_API_V2.Services
{
    public class InputValidationService : IInputValidationService
    {
        private readonly ScoringRequestValidator _scoringRequestValidator;

        public InputValidationService(ScoringRequestValidator scoringRequestValidator)
        {
            _scoringRequestValidator = scoringRequestValidator;
        }
        public ValidationResult ScoringRequestIsValid(ScoringRequest request)
        {
            return _scoringRequestValidator.Validate(request);
        }
        public bool ValidateUser(CreateUser user)
        {
            return true;
        }

        public bool ValidateAccessRequest(AccessRequest user)
        {
            return true;
        }
    }
}
