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
        var request = _mapper.Map<ScoringRequestModel>(dto);
        request.Should().NotBeNull();
        request?.LatestBiomarkers?.Should().NotBeNull();
        request?.LatestBiomarkers?.Age.Should().Be(dto.Age.Value);
        request?.LatestBiomarkers?.Alat.Should().Be(dto.Alat.Value);
        request?.LatestBiomarkers?.Albumin.Should().Be(dto.Albumin.Value);
        request?.LatestBiomarkers?.Bilirubin.Should().Be(dto.Bilirubin.Value);
        request?.LatestBiomarkers?.Cholesterol.Should().Be(dto.Cholesterol.Value);
        request?.LatestBiomarkers?.Diabetes.Should().Be(dto.Diabetes.Value);
        request?.LatestBiomarkers?.Diuretic.Should().Be(dto.Diuretic.Value);
        request?.LatestBiomarkers?.Glucose.Should().Be(dto.GlucoseFasting.Value);
        request?.LatestBiomarkers?.Hdl.Should().Be(dto.Hdl.Value);
        request?.LatestBiomarkers?.Height.Should().Be(dto.Height.Value);
        request?.LatestBiomarkers?.Hstroponint.Should().Be(dto.HsTroponinT.Value);
        request?.LatestBiomarkers?.Ldl.Should().Be(dto.Ldl.Value);
        request?.LatestBiomarkers?.Leukocyte.Should().Be(dto.Leukocytes.Value);
        request?.LatestBiomarkers?.Mchc.Should().Be(dto.Mchc.Value);
        request?.LatestBiomarkers?.Nicotine.Should().Be(dto.NicotineConsumption.Value);
        request?.LatestBiomarkers?.Protein.Should().Be(dto.Protein.Value);
        request?.LatestBiomarkers?.Sex.Should().Be(dto.Sex.Value);
        request?.LatestBiomarkers?.Statin.Should().Be(dto.CholesterolLowering_Statin.Value);
        request?.LatestBiomarkers?.Systolicbp.Should().Be(dto.SystolicBloodPressure.Value);
        request?.LatestBiomarkers?.Urea.Should().Be(dto.Urea.Value);
        request?.LatestBiomarkers?.Albumin.Should().Be(dto.Albumin.Value);
        request?.LatestBiomarkers?.Aceinhibitor.Should().Be(dto.ACEInhibitor.Value);
        request?.LatestBiomarkers?.Alkaline.Should().Be(dto.AlkalinePhosphatase.Value); // TODO: Check naming
        request?.LatestBiomarkers?.Betablocker.Should().Be(dto.Betablocker.Value);
        request?.LatestBiomarkers?.Qwave.Should().Be(dto.RestingECG.Value);
        request?.LatestBiomarkers?.Calciumant.Should().Be(dto.CaAntagonist.Value);
        request?.LatestBiomarkers?.Chestpain.Should().Be(dto.ChestPain.Value);
        request?.LatestBiomarkers?.Amylasep.Should().Be(dto.PancreaticAmylase.Value);
        request?.LatestBiomarkers?.Tcagginhibitor.Should().Be(dto.TCAggregationInhibitor.Value);
        request?.LatestBiomarkers?.PriorCAD.Should().Be(dto.prior_CAD.Value);
    }

    [Test]
    public void CreateUserDtoToUser()
    {
        //Arrange
        var userDto = MockDataProvider.GetMockedCreateUserDto();

        //Act
        var user = _mapper.Map<UserModel>(userDto);

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
        user.Language.Should().Be("Mock");
        user.UnitLabValues.Should().Be("Mock");
        user.ClinicalSetting.Should().Be(ClinicalSetting.SecondaryCare);
    }
}