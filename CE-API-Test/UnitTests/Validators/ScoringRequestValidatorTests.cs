using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using System.Globalization;
namespace CE_API_Test.UnitTests.Validators;

public class ScoringRequestValidatorTests
{
    private ScoringRequestValidator _validator;
    private UserModel _testUser;
    private ScoringRequest _validRequest;
    private ValidationContext<ScoringRequest> _validContext;
    private TestValidationResult<ScoringRequest> _validationResult;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _validator = new ScoringRequestValidator();
        _testUser = MockDataProvider.GetMockedUserModel();
        _testUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;

        CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

        _validRequest = MockDataProvider.CreateValidScoringRequestDto();
        _validContext = new ValidationContext<ScoringRequest>(_validRequest);
        _validContext.RootContextData.Add("currentUser", _testUser);

        _validationResult = _validator.TestValidate(_validContext);
    }

    [Test]
    public void TestValidate_GivenInvalidRequest_ReturnsPresentErrors()
    {
        //Arrange
        var invalidRequest = new ScoringRequest()
        {
            Age = new BiomarkerValue<int>() { UnitType = "SI", Value = 0 },
            AlkalinePhosphatase = new BiomarkerValue<float>() { UnitType = "SI", Value = 0 },
        };

        var invalidContext = new ValidationContext<ScoringRequest>(invalidRequest);
        invalidContext.RootContextData.Add("currentUser", _testUser);

        //Act
        var valRes = _validator.TestValidate(invalidContext);

        //Assert
        valRes.ShouldHaveValidationErrorFor(x => x.Age.Value);
    }

    [Test]
    public void GeneralTest_TestValidate_GivenValidRequest_ReturnsValidResult()
    {
        //Arrange

        //Act

        //Assert
        _validationResult.ShouldNotHaveAnyValidationErrors();
    }


    [Test]
    public void TestValidate_GivenInvalidAge_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Age.Value = 10;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Age.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidSystolicBloodPressure_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.SystolicBloodPressure.Value = 10;
        
        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.SystolicBloodPressure.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenHeight_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Height.Value = 10;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Height.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenWeight_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Weight.Value = 10;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Weight.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenDiastolicBloodPressure_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.DiastolicBloodPressure.Value = 10;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.DiastolicBloodPressure.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenPancreaticAmylase_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.PancreaticAmylase.Value = 0;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.PancreaticAmylase.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenAlkalinePhosphatase_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.AlkalinePhosphatase.Value = 12312312;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.AlkalinePhosphatase.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidHsTroponinT_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.HsTroponinT.Value = 0;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.HsTroponinT.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidAlat_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Alat.Value = 1333;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Alat.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidGlucoseFasting_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.GlucoseFasting.Value = 0.1f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.GlucoseFasting.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidBilirubin_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Bilirubin.Value = 1.6f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Bilirubin.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidUrea_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Urea.Value = 0.2f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Urea.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidUricAcid_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.UricAcid.Value = 9.0f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.UricAcid.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidCholesterol_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Cholesterol.Value = 2000.0f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Cholesterol.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenValidCholesterolWithConventionalUnitType_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Cholesterol.Value = 2000.0f;
        _validRequest.Cholesterol.UnitType = "Conventional";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldNotHaveValidationErrorFor(x => x.Cholesterol.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidCholesterolWithConventionalUnitType_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Cholesterol.Value = 3867.0f;
        _validRequest.Cholesterol.UnitType = "Conventional";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Cholesterol.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidHdl_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Hdl.Value = 200.0f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Hdl.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidLdl_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Ldl.Value = 0.01f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Ldl.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidLdlWithConventionalUnitType_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Ldl.Value = 0.5f;
        _validRequest.Ldl.UnitType = "Conventional";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Ldl.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenValidLdlWithConventionalUnitType_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Ldl.Value = 1.17f;
        _validRequest.Ldl.UnitType = "Conventional";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldNotHaveValidationErrorFor(x => x.Ldl.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidProtein_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Protein.Value = 4.0f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Protein.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidAlbumin_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Albumin.Value = 4.0f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Albumin.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidLeukocytes_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Leukocytes.Value = 0.01f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Leukocytes.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidMchc_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Mchc.Value = 1100.0f;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidMchcWithConventionalUnitType_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Mchc.Value = 1100.0f;
        _validRequest.Mchc.UnitType = "Conventional";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenValidMchcWithConventionalUnitType_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Mchc.Value = 99;
        _validRequest.Mchc.UnitType = "Conventional";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldNotHaveValidationErrorFor(x => x.Mchc.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidPrior_CAD_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.prior_CAD.Value = true;
        _validRequest.RestingECG.Value = PatientDataEnums.RestingEcg.No;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.RestingECG.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidPrior_CADWithSecondaryCareNoRestingEcg_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.prior_CAD.Value = false;
        _testUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
        _validRequest.RestingECG.Value = PatientDataEnums.RestingEcg.No;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.RestingECG.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidPrior_CADWithPrimaryCareNoRestingEcg_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.prior_CAD.Value = false;
        _testUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;
        _validRequest.RestingECG.Value = PatientDataEnums.RestingEcg.No;

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldNotHaveValidationErrorFor(x => x.RestingECG.Value);
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidMchcWithDefaultCultureInfo_ReturnsCorrespondingError()
    {
        //Arrange
        _validRequest.Mchc.UnitType = "ABC";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.UnitType)
            .WithErrorMessage("'{mchc}': The given unit type does not exist.");
        _validationResult.Errors.Clear();
    }

    [Test]
    public void TestValidate_GivenInvalidMchcWithGermanCultureInfo_ReturnsCorrespondingError()
    {
        //Arrange
        CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
        _validRequest.Mchc.UnitType = "ABC";

        //Act
        _validationResult = _validator.TestValidate(_validContext);

        //Assert
        _validationResult.ShouldHaveValidationErrorFor(x => x.Mchc.UnitType)
            .WithErrorMessage("'{mchc}': Die angegebene Einheit existiert nicht.");
        _validationResult.Errors.Clear();
    }
}
