using System.Data.SqlTypes;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
namespace CE_API_V2.Services.Mocks;

public class MockedInputValidationService : IInputValidationService
{
    public bool BiomarkersAreValid(object value) => true;
    public bool ScoringRequestIsValid(ScoringRequestDto value) => true;
    public bool ValidateUser(CreateUserDto user) => true;
}