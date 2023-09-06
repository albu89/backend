using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using FluentValidation.Results;

namespace CE_API_V2.Services.Mocks;

public class MockedInputValidationService : IInputValidationService
{
    public ValidationResult ScoringRequestIsValid(ScoringRequest request, UserModel user) => new ();
    public bool ValidateUser(CreateUser user) => true;
    public bool BiomarkersAreValid(ScoringRequest value) => true;
    public bool ValidateAccessRequest(AccessRequest user) => true;
    public bool ValidateOrganization(CreateOrganization organization) => true;
}