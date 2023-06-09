using CE_API_V2.Services.Interfaces;
namespace CE_API_V2.Services.Mocks;

public class MockedInputValidationService : IInputValidationService
{
    public bool BiomarkersAreValid(object value) => true;
}