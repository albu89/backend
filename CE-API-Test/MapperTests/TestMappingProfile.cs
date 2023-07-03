using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.Mapping;
using static CE_API_V2.Models.Enum.PatientDataEnums;

namespace CE_API_Test.MapperTests;

public class TestMappingProfile
{
    private IMapper _mapper;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

        _mapper = mapperConfig.CreateMapper();
    }

    [Test]
    public void RequestDtoToRequest()
    {
        var dto = MockDataProvider.CreateScoringRequestDto();
        var request = _mapper.Map<ScoringRequest>(dto);
        request.Id.Should().NotBeEmpty();
        request.Biomarkers.Should().NotBeNull();
        request.Biomarkers.Age.Should().Be(dto.Age.Value);
        request.Biomarkers.Alat.Should().Be(dto.Alat.Value);
        request.Biomarkers.Albumin.Should().Be(dto.Albumin.Value);
        request.Biomarkers.Bilirubin.Should().Be(dto.Bilirubin.Value);
        request.Biomarkers.Cholesterol.Should().Be(dto.Cholesterol.Value);
        request.Biomarkers.Diabetes.Should().Be(dto.Diabetes.Value);
        request.Biomarkers.Diuretic.Should().Be(dto.Diuretic.Value);
        request.Biomarkers.Glucose.Should().Be(dto.GlucoseFasting.Value);
        request.Biomarkers.Hdl.Should().Be(dto.Hdl.Value);
        request.Biomarkers.Height.Should().Be(dto.Height.Value);
        request.Biomarkers.HsTroponinT.Should().Be(dto.HsTroponinT.Value);
        request.Biomarkers.Ldl.Should().Be(dto.Ldl.Value);
        request.Biomarkers.Leukocytes.Should().Be(dto.Leukocytes.Value);
        request.Biomarkers.Mchc.Should().Be(dto.Mchc.Value);
        request.Biomarkers.Nicotine.Should().Be(dto.NicotineConsumption.Value);
        request.Biomarkers.Protein.Should().Be(dto.Protein.Value);
        request.Biomarkers.Sex.Should().Be(dto.Sex.Value);
        request.Biomarkers.Statin.Should().Be(dto.CholesterolLowering_Statin.Value);
        request.Biomarkers.SystolicBloodPressure.Should().Be(dto.SystolicBloodPressure.Value);
        request.Biomarkers.Urea.Should().Be(dto.Urea.Value);
        request.Biomarkers.Albumin.Should().Be(dto.Albumin.Value);
        request.Biomarkers.AceInhibitor.Should().Be(dto.ACEInhibitor.Value);
        request.Biomarkers.AlkalinePhosphate.Should().Be(dto.AlkalinePhosphatase.Value); // TODO: Check naming
        request.Biomarkers.Betablocker.Should().Be(dto.Betablocker.Value);
        request.Biomarkers.RestingECG.Should().Be(dto.RestingECG.Value);
        request.Biomarkers.CaAntagonist.Should().Be(dto.CaAntagonist.Value);
        request.Biomarkers.ChestPain.Should().Be(dto.ChestPain.Value);
        request.Biomarkers.PancreaticAmylase.Should().Be(dto.PancreaticAmylase.Value);
        request.Biomarkers.TcAggInhibitor.Should().Be(dto.TCAggregationInhibitor.Value);
        request.Biomarkers.PriorCAD.Should().Be(dto.prior_CAD.Value);
    }

    [Test]
    public void CreateUserDtoToUser()
    {
        //Arrange
        var userDto = MockDataProvider.GetMockedCreateUserDto();

        //Act
        var user = _mapper.Map<User>(userDto);

        //Assert
        user.Should().NotBeNull();
        user.UserId.Should().Be(string.Empty);
        user.TenantID.Should().Be(string.Empty);
        user.Address.Should().Be("Mock");
        user.Salutation.Should().Be("Mock");
        user.Surname.Should().Be("Mock");
        user.FirstName.Should().Be("Mock");
        user.ProfessionalSpecialisation.Should().Be("Mock");
        user.Department.Should().Be("Mock");
        user.PreferredLab.Should().Be("Mock");
        user.Address.Should().Be("Mock");
        user.City.Should().Be("Mock");
        user.CountryCode.Should().Be("Mock");
        user.Country.Should().Be("Mock");
        user.TelephoneNumber.Should().Be("Mock");
        user.EMailAdress.Should().Be("Mock");
        user.Language.Should().Be("Mock");
        user.UnitLabValues.Should().Be("Mock");
        user.ClinicalSetting.Should().Be(ClinicalSetting.SecondaryCare);
    }
}