using System.Globalization;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Validators;
using FluentValidation;
using FluentValidation.TestHelper;

namespace CE_API_Test.UnitTests.Validators;

public class ValidatorTest
{
    private ScoringRequestValidator _validator;
    private UserModel _testUser;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _validator = new ScoringRequestValidator();
        _testUser = MockDataProvider.GetMockedUserModel();
        _testUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;
    }
    
    [Test]
    public void Tests()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-GB");
        var invalidRequest = new ScoringRequest()
        {
            Age = new BiomarkerValue<int>() { UnitType = "SI", Value = 0 },
            AlkalinePhosphatase = new BiomarkerValue<float>() { UnitType = "SI", Value = 0 },
        };

        var invalidContext = new ValidationContext<ScoringRequest>(invalidRequest);
        invalidContext.RootContextData.Add("currentUser", _testUser);
        
        var valRes = _validator.TestValidate(invalidContext);
        valRes.ShouldHaveValidationErrorFor(x => x.Age.Value);
        
        
        var validRequest = MockDataProvider.CreateValidScoringRequestDto();
        var validContext = new ValidationContext<ScoringRequest>(validRequest);
        validContext.RootContextData.Add("currentUser", _testUser);
            
        var validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldNotHaveAnyValidationErrors();

        validRequest.Age.Value = 10;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Age.Value);
        validationResult.Errors.Clear();

        validRequest.SystolicBloodPressure.Value = 10;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.SystolicBloodPressure.Value);
        validationResult.Errors.Clear();

        validRequest.Height.Value = 10;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Height.Value);
        validationResult.Errors.Clear();

        validRequest.Weight.Value = 10;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Weight.Value);
        validationResult.Errors.Clear();

        validRequest.DiastolicBloodPressure.Value = 10;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.DiastolicBloodPressure.Value);
        validationResult.Errors.Clear();

        validRequest.PancreaticAmylase.Value = 0;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.PancreaticAmylase.Value);
        validationResult.Errors.Clear();

        validRequest.AlkalinePhosphatase.Value = 12312312;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.AlkalinePhosphatase.Value);
        validationResult.Errors.Clear();

        validRequest.HsTroponinT.Value = 0;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.HsTroponinT.Value);
        validationResult.Errors.Clear();

        validRequest.Alat.Value = 1333;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Alat.Value);
        validationResult.Errors.Clear();

        validRequest.GlucoseFasting.Value = 0.1f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Weight.Value);
        validationResult.Errors.Clear();

        validRequest.Bilirubin.Value = 1.6f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Bilirubin.Value);
        validationResult.Errors.Clear();

        validRequest.Urea.Value = 0.2f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Urea.Value);
        validationResult.Errors.Clear();

        validRequest.UricAcid.Value = 9.0f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.UricAcid.Value);
        validationResult.Errors.Clear();

        validRequest.Cholesterol.Value = 2000.0f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Cholesterol.Value);
        validationResult.Errors.Clear();
        
        validRequest.Cholesterol.Value = 2000.0f;
        validRequest.Cholesterol.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Cholesterol.Value);
        validationResult.Errors.Clear();
        
        validRequest.Cholesterol.Value = 3867.0f;
        validRequest.Cholesterol.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Cholesterol.Value);
        validationResult.Errors.Clear();

        validRequest.Hdl.Value = 200.0f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Hdl.Value);
        validationResult.Errors.Clear();

        validRequest.Ldl.Value = 0.01f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Ldl.Value);
        validationResult.Errors.Clear();
        
        validRequest.Ldl.Value = 0.5f;
        validRequest.Ldl.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Ldl.Value);
        validationResult.Errors.Clear();
        
        validRequest.Ldl.Value = 1.17f;
        validRequest.Ldl.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Ldl.Value);
        validationResult.Errors.Clear();

        validRequest.Protein.Value = 4.0f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Protein.Value);
        validationResult.Errors.Clear();

        validRequest.Albumin.Value = 4.0f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Albumin.Value);
        validationResult.Errors.Clear();

        validRequest.Leukocytes.Value = 0.01f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Leukocytes.Value);
        validationResult.Errors.Clear();

        validRequest.Mchc.Value = 1100.0f;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.Value);
        validationResult.Errors.Clear();

        validRequest.Mchc.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.Value);
        validationResult.Errors.Clear();

        validRequest.Mchc.Value = 99;
        validRequest.Mchc.UnitType = "Conventional";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.Mchc.Value);
        validationResult.Errors.Clear();

        validRequest.prior_CAD.Value = true;
        validRequest.RestingECG.Value = PatientDataEnums.RestingEcg.No;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.RestingECG.Value);
        validationResult.Errors.Clear();
        
        validRequest.prior_CAD.Value = false;
        _testUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
        validRequest.RestingECG.Value = PatientDataEnums.RestingEcg.No;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.RestingECG.Value);
        validationResult.Errors.Clear();
        
        validRequest.prior_CAD.Value = false;
        _testUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;
        validRequest.RestingECG.Value = PatientDataEnums.RestingEcg.No;
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldNotHaveValidationErrorFor(x => x.RestingECG.Value);
        validationResult.Errors.Clear();

        validRequest.Mchc.UnitType = "ABC";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.UnitType).WithErrorMessage("'{mchc}': The given unit type does not exist.");
        validationResult.Errors.Clear();

        CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
        validRequest.Mchc.UnitType = "ABC";
        validationResult = _validator.TestValidate(validContext);
        validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.UnitType).WithErrorMessage("'{mchc}': Die angegebene Einheit existiert nicht.");
        validationResult.Errors.Clear();
        
    }
}