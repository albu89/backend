using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;

namespace CE_API_Test.UnitTests.Services;

[TestFixture]
public class BiomarkersTemplateServiceTests
{
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
    [TestCase("de-DE", "Prior CAD", "DEDEDEDEDE")]
    public async Task TestGetTemplate_GivenCorrectLocalization_ExpectedCorrectlyAssembledBiomarkerSchemaDtoList(string languageTag, string localizedFieldName, string localizedInfoText)
    {
        //Arrange

        //Act
        var getTemplateTask = () => _biomarkersTemplateService.GetTemplate(languageTag);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        result.Subject.Should().BeOfType<List<BiomarkerSchema>>();
        result.Subject.Should().NotBeNull();
        result.Subject.Count().Should().Be(33);
        result.Subject.Any(x => string.IsNullOrEmpty(x.Id)).Should().BeFalse();
        result.Subject.Any(x => x.Units is null).Should().BeFalse();
        result.Subject.Any(x => x.Units.Length == 0).Should().BeFalse();
        result.Subject.All(x => string.IsNullOrEmpty(x.Category)).Should().BeFalse("we expect at least one Item to have a category");
        result.Subject.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse("we expect at least one Item to have an InfoText");

        result.Subject.Any(x => string.IsNullOrEmpty(x.Fieldname)).Should().BeFalse();
        var elementToTest = result.Subject.FirstOrDefault(x => x.Id == "prior_CAD");
        elementToTest.Fieldname.Should().Be(localizedFieldName);
        result.Subject.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse();
        elementToTest.InfoText.Should().Be(localizedInfoText);
        elementToTest.Units.Should().NotBeNullOrEmpty();
        elementToTest.Category.Should().NotBeNullOrEmpty();
        elementToTest.Id.Should().NotBeNullOrEmpty();
    }    
    
    [Test]
    public async Task TestGetTemplate_GivenIncorrectCorrectLocalization_ExpectedCorrectlyAssembledDefaultBiomarkerSchemaDtoList()
    {
        //Arrange
        var locale = "wrongvalue"; 

        var expectedFieldName = "Prior CAD";
        var expectedInfoText =
            "Prior CAD refers to a history of coronary artery disease (CAD) in a patient. Prior CAD increases the likelihood of patients with cardiovascular disease.";

        //ActPrior
        var getTemplateTask = () => _biomarkersTemplateService.GetTemplate(locale);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        result.Subject.Should().BeOfType<List<BiomarkerSchema>>();
        result.Subject.Should().NotBeNull();
        result.Subject.Count().Should().Be(33);
        result.Subject.Any(x => string.IsNullOrEmpty(x.Id)).Should().BeFalse();
        result.Subject.Any(x => x.Units is null).Should().BeFalse();
        result.Subject.Any(x => x.Units.Length == 0).Should().BeFalse();
        result.Subject.All(x => string.IsNullOrEmpty(x.Category)).Should().BeFalse("we expect at least one Item to have a category");
        result.Subject.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse("we expect at least one Item to have an InfoText");

        result.Subject.Any(x => string.IsNullOrEmpty(x.Fieldname)).Should().BeFalse();
        var elementToTest = result.Subject.FirstOrDefault(x => x.Id == "prior_CAD");
        elementToTest.Fieldname.Should().Be(expectedFieldName);
        result.Subject.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse();
        elementToTest.InfoText.Should().Be(expectedInfoText);
    }
}