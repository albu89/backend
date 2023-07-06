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
        public ValidationResult ScoringRequestIsValid(ScoringRequestDto value)
        {
            return _scoringRequestValidator.Validate(value);
        }
        public bool ValidateUser(CreateUserDto user)
        {
            return true;
        }

        public bool ValidateAccessRequest(AccessRequestDto user)
        {
            return true;
        }
    }
}
