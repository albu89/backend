using CE_API_V2.Models.DTO;
using FluentValidation.Results;
namespace CE_API_V2.Services.Interfaces;

public interface IInputValidationService
{
    public ValidationResult ScoringRequestIsValid(ScoringRequestDto value);
    public bool ValidateUser(CreateUserDto user);
}