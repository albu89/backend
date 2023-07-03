using CE_API_V2.Models.DTO;

namespace CE_API_V2.Services.Interfaces;

public interface IInputValidationService
{
    public bool BiomarkersAreValid(object value);
    public bool ScoringRequestIsValid(ScoringRequestDto value);
    public bool ValidateUser(CreateUserDto user);
}