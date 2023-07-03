using CE_API_Test.TestUtilities;
using CE_API_V2.Validators;
using FluentValidation.TestHelper;
namespace CE_API_Test.UnitTests.Validators;

public class ValidatorTest
{
    private ScoringRequestValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _validator = new ScoringRequestValidator();
    }
    
    [Test]
    public void Tests()
    {
        var validRequest = MockDataProvider.CreateValidScoringRequestDto();
        var validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldNotHaveAnyValidationErrors();

        validRequest.Age.Value = 10;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Age.Value);

        validRequest.SystolicBloodPressure.Value = 10;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.SystolicBloodPressure.Value);

        validRequest.Height.Value = 10;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Height.Value);

        validRequest.Weight.Value = 10;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Weight.Value);

        validRequest.DiastolicBloodPressure.Value = 10;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.DiastolicBloodPressure.Value);

        validRequest.PancreaticAmylase.Value = 0;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.PancreaticAmylase.Value);

        validRequest.AlkalinePhosphatase.Value = 12312312;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.AlkalinePhosphatase.Value);

        validRequest.HsTroponinT.Value = 0;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.HsTroponinT.Value);

        validRequest.Alat.Value = 1333;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Alat.Value);

        validRequest.GlucoseFasting.Value = 0.1f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Weight.Value);

        validRequest.Bilirubin.Value = 1.6f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Bilirubin.Value);

        validRequest.Urea.Value = 0.2f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Urea.Value);

        validRequest.UricAcid.Value = 9.0f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.UricAcid.Value);

        validRequest.Cholesterol.Value = 2000.0f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Cholesterol.Value);

        validRequest.Hdl.Value = 200.0f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Hdl.Value);

        validRequest.Ldl.Value = 0.01f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Ldl.Value);

        validRequest.Protein.Value = 4.0f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Protein.Value);

        validRequest.Albumin.Value = 4.0f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Albumin.Value);

        validRequest.Leukocytes.Value = 0.01f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Leukocytes.Value);

        validRequest.Mchc.Value = 1100.0f;
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.Value);

        validRequest.Mchc.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validRequest);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Mchc.Value);
    }
}