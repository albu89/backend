using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Validators;
using FluentValidation;
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
        public ValidationResult ScoringRequestIsValid(ScoringRequest request, UserModel currentUser)
        {
            var ctx = new ValidationContext<ScoringRequest>(request);
            ctx.RootContextData.Add("currentUser", currentUser);
            return _scoringRequestValidator.Validate(ctx);
        }
        public bool ValidateUser(CreateUser user)
        {
            return true;
        }

        public bool ValidateAccessRequest(AccessRequest user)
        {
            return true;
        }

        public bool ValidateOrganization(CreateOrganization organisation)
        {
            return true;
        }
        
        public bool ValidateCountry(CreateCountry createCountry)
        {
            return true;
        }
    }
}
