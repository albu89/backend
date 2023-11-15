using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;

namespace CE_API_Test.UnitTests.Services;

[TestFixture]
public class BiomarkersTemplateServiceTests
{
    private const string ExpectedId = "prior_CAD";
    private const string CategoryText = "we expect at least one Item to have a category";
    private const string InfoText = "we expect at least one Item to have an InfoText";

    private IBiomarkersTemplateService _biomarkersTemplateService;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();
        _biomarkersTemplateService = new BiomarkersTemplateService(mapper);
    }

    [Test]
    [TestCase("en-GB", "Prior CAD", "Prior CAD refers to a history of coronary artery disease (CAD) in a patient. Prior CAD increases the likelihood of patients with cardiovascular disease.")]
    [TestCase("en-FR", "Prior CAD", "Prior CAD refers to a history of coronary artery disease (CAD) in a patient. Prior CAD increases the likelihood of patients with cardiovascular disease.")] // default will be used
    [TestCase("de-DE", "Vorherige KHK", "Vorherige koronare Herzkrankheit bezieht sich auf eine Vorgeschichte einer koronaren Herzkrankheit (KHK) bei einem Patienten. Eine Vorherige koronare Herzkrankheit erhöht die Wahrscheinlichkeit, dass Patienten an Herz-Kreislauf-Erkrankungen leiden.")]
    public async Task GetTemplate_GivenCorrectLocalization_ExpectedCorrectlyAssembledCadRequestSchemaDtoList(string languageTag, string localizedFieldName, string localizedInfoText)
    {
        //Arrange

        //Act
        var getTemplateTask = () => _biomarkersTemplateService.GetTemplate(languageTag);

        //Assert
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<CadRequestSchema>();
        result.Subject.Should().NotBeNull();
        result.Subject.AllMarkers.Count().Should().Be(33);
        result.Subject.AllMarkers.Any(x => string.IsNullOrEmpty(x.Id)).Should().BeFalse();
        result.Subject.AllMarkers.All(x => string.IsNullOrEmpty(x.CategoryId)).Should().BeFalse(CategoryText);
        result.Subject.AllMarkers.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse(InfoText);
        result.Subject.AllMarkers.Any(x => string.IsNullOrEmpty(x.DisplayName)).Should().BeFalse();
        result.Subject.LabResults.Any(x => x.Units.Length == 0).Should().BeFalse(); 

        var medicalHistoryElementToTest = result.Subject.MedicalHistory.FirstOrDefault(x => x.Id == ExpectedId);
        medicalHistoryElementToTest.Should().NotBeNull();
        medicalHistoryElementToTest!.DisplayName.Should().Be(localizedFieldName);
        medicalHistoryElementToTest.InfoText.Should().Be(localizedInfoText);
        medicalHistoryElementToTest.Unit.Should().NotBeNull();
        medicalHistoryElementToTest.CategoryId.Should().NotBeNullOrEmpty();
        medicalHistoryElementToTest.Id.Should().NotBeNullOrEmpty();
    }    
    
    [Test]
    public async Task GetTemplate_GivenIncorrectCorrectLocalization_ExpectedCorrectlyAssembledDefaultCadRequestSchemaDtoList()
    {
        //Arrange
        var locale = "wronglocale"; 
        var expectedFieldName = "Prior CAD";
        var expectedInfoText = "Prior CAD refers to a history of coronary artery disease (CAD) in a patient. Prior CAD increases the likelihood of patients with cardiovascular disease.";

        //Act
        var getTemplateTask = () => _biomarkersTemplateService.GetTemplate(locale);

        //Assert
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<CadRequestSchema>();
        result.Subject.Should().NotBeNull();
        result.Subject.AllMarkers.Count().Should().Be(33);
        result.Subject.AllMarkers.Any(x => string.IsNullOrEmpty(x.Id)).Should().BeFalse();
        result.Subject.AllMarkers.All(x => string.IsNullOrEmpty(x.CategoryId)).Should().BeFalse(CategoryText);
        result.Subject.AllMarkers.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse(InfoText);
        result.Subject.AllMarkers.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse();
        result.Subject.AllMarkers.Any(x => string.IsNullOrEmpty(x.DisplayName)).Should().BeFalse();
        result.Subject.LabResults.Any(x => x.Units.Length == 0).Should().BeFalse();

        var elementToTest = result.Subject.AllMarkers.FirstOrDefault(x => x.Id == ExpectedId);
        elementToTest.Should().NotBeNull();
        elementToTest!.DisplayName.Should().Be(expectedFieldName);
        elementToTest.InfoText.Should().Be(expectedInfoText);
    }
}