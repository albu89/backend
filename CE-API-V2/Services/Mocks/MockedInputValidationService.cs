using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using FluentValidation.Results;

namespace CE_API_V2.Services.Mocks;

public class MockedInputValidationService : IInputValidationService
{
    public ValidationResult ScoringRequestIsValid(ScoringRequestDto value) => new ();
    public bool ValidateUser(CreateUserDto user) => true;
    public bool BiomarkersAreValid(ScoringRequestDto value) => true;
    public bool ValidateAccessRequest(AccessRequestDto user) => true;
}