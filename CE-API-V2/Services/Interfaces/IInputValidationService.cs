﻿using CE_API_V2.Models.DTO;
using FluentValidation.Results;
namespace CE_API_V2.Services.Interfaces;

public interface IInputValidationService
{
    public ValidationResult ScoringRequestIsValid(ScoringRequest request);
    public bool ValidateUser(CreateUser user);
    public bool ValidateAccessRequest(AccessRequest user);
}