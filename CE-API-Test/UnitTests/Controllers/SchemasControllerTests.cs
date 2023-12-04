using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Controllers;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NSubstitute;

namespace CE_API_Test.UnitTests.Controllers;

[TestFixture]
public class SchemasControllerTests
{
    private SchemasController _schemasController;
    private IBiomarkersTemplateService _biomarkersTemplateService;
    private IScoringTemplateService _scoringTemplateService;
    private UserHelper _userHelper;
    private IScoreSummaryUtility _scoreSummaryUtility;
    private IUserUOW _userUow;
    private IConfigurationRoot _configuration;
    private IUserInputTemplateService _userInputTemplateService;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();

        var testConfig = new Dictionary<string, string?>()
        {
            { "EditPeriodInDays", "1" },
        };

        _biomarkersTemplateService = new BiomarkersTemplateService(mapper);
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();

        var inMemSettings = new Dictionary<string, string>
            {
                { "AiSubpath", Resources.TestResources.AiSubpath }
            };

        var configuration1 = new ConfigurationBuilder().AddInMemoryCollection(inMemSettings!).Build();
        var template = await _biomarkersTemplateService.GetTemplate();

        var userUow = new Mock<IUserUOW>();
        userUow.Setup(x => x.GetBiomarkerOrders(It.IsAny<string>())).Returns(MockDataProvider.GetMockedOrderModels());
        userUow.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(MockDataProvider.GetMockedUserModel);
        userUow.Setup(x => x.OrderTemplate(It.IsAny<CadRequestSchema>(), It.IsAny<string>())).Returns(template);

        var userInputTemplateService = new Mock<IUserInputTemplateService>();
        userInputTemplateService.Setup(x => x.GetTemplate(It.IsAny<UserIdsRecord>(), It.IsAny<string>()))
            .Returns(Task.FromResult(new UserInputFormSchema { City = "mockedcity"}));

        _userInputTemplateService = userInputTemplateService.Object;
        _userHelper = new UserHelper(mapper, configuration1);
        _scoreSummaryUtility = new ScoreSummaryUtility(mapper, _configuration);
        _userUow = userUow.Object;
        _scoringTemplateService = new ScoringTemplateService(mapper, _biomarkersTemplateService, _scoreSummaryUtility, userUow.Object);
        _schemasController = new SchemasController(_biomarkersTemplateService, _scoringTemplateService, _userUow, new UserInformationExtractor(), _userInputTemplateService, _userHelper);
    }

    [Test]
    public async Task GetInputFormTemplate_GivenCorrectLocale_ExpectedOkObjectResult() 
    {
        //Arrange
        var locale = "en-GB";
        var getTemplateTask = () => _schemasController.GetInputFormTemplate(locale);

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult) result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<CadRequestSchema>();
        var biomarkersTemplate = (CadRequestSchema) template;
        biomarkersTemplate.AllMarkers.Count().Should().Be(33);
    }

    [Test]
    public async Task GetInputFormTemplate_GivenInvalidLocale_ReturnsOkObjectResultContainingCadRequestSchema()
    {
        //Arrange
        var locale = "invalid";
        var exampleBiomarker = "amylase_p";
        var expectedInfoText =
            "Amylase is an enzyme, or special protein, that helps in the digestion of carbohydrates. Tests for amylase in blood are mainly used to diagnose problems with pancreas, including pancreatitis. It is also used to monitor chronic (long-term) pancreatitis.\r\n\r\nAcute pancreatitis can be associated with electrical changes mimicking acute coronary syndrome with normal coronary arteries.";
        var getTemplateTask = () => _schemasController.GetInputFormTemplate(locale);

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert 
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<CadRequestSchema>();

        var biomarkersTemplate = (CadRequestSchema)template!;
        biomarkersTemplate.AllMarkers.Count().Should().Be(33);
        var firstElement = biomarkersTemplate.AllMarkers.FirstOrDefault(x=> x.Id == exampleBiomarker);
        firstElement.Should().NotBeNull();
        firstElement.InfoText.Should().BeEquivalentTo(expectedInfoText);
    }

    [Test]
    public async Task GetInputFormTemplate_GivenNoParameter_ReturnsAllBiomarkersWithDefaultLocale()
    {
        //Arrange
        var exampleBiomarker = "amylase_p";
        var expectedInfoText =
            "Amylase is an enzyme, or special protein, that helps in the digestion of carbohydrates. Tests for amylase in blood are mainly used to diagnose problems with pancreas, including pancreatitis. It is also used to monitor chronic (long-term) pancreatitis.\r\n\r\nAcute pancreatitis can be associated with electrical changes mimicking acute coronary syndrome with normal coronary arteries.";

        var getTemplateTask = () => _schemasController.GetInputFormTemplate();

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert 
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<CadRequestSchema>();

        var biomarkersTemplate = (CadRequestSchema)template!;
        biomarkersTemplate.AllMarkers.Count().Should().Be(33);
        var firstElement = biomarkersTemplate.AllMarkers.FirstOrDefault(x => x.Id == exampleBiomarker);
        firstElement.Should().NotBeNull();
        firstElement.InfoText.Should().BeEquivalentTo(expectedInfoText);
    }

    [Test]
    public async Task GetScoringSchema_WithValidParameter_ReturnOkRequestResult()
    {
        //Arrange
        var expectedScoreHeader =
            "Gemäß den aktuellen medizinischen Leitlinien ist eine obstruktive koronare Herzkrankheit (KHK) definiert als eine Stenose oder Blockade von 50 % oder mehr im Durchmesser einer großen Koronararterie oder eines Astes mit einem Durchmesser von 2 mm oder mehr.";
        var locale = "de-DE";

        //Act
        var result = await _schemasController.GetScoringSchema(locale);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));

        var okObjectResult = result as OkObjectResult;
        okObjectResult.Value.Should().BeOfType<ScoreSchema>();
        var scoreSchema = okObjectResult.Value as ScoreSchema;
        scoreSchema.CadDefinition.Should().Be(expectedScoreHeader);
    }

    [Test]
    public async Task GetScoringSchema_WithoutParameter_ReturnOkRequestResult()
    {
        //Arrange
        var expectedScoreHeader =
            "In accordance to current medical guidelines, obstructive CAD is defined as a stenosis or blockage of 50% or more in the diameter of a major coronary artery or a branch with a diameter of 2 mm or more.";
        //Act
        var result = await _schemasController.GetScoringSchema();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));

        var okObjectResult = result as OkObjectResult;
        okObjectResult.Value.Should().BeOfType<ScoreSchema>();
        okObjectResult?.StatusCode.Should().Be(200);
        var scoreSchema = okObjectResult.Value as ScoreSchema;
        scoreSchema.CadDefinition.Should().Be(expectedScoreHeader);
    }

    [Test]
    public async Task GetScoringSchema_WithoutInvalid_ReturnOkRequestResult()
    {
        //Arrange
        var expectedScoreHeader =
            "In accordance to current medical guidelines, obstructive CAD is defined as a stenosis or blockage of 50% or more in the diameter of a major coronary artery or a branch with a diameter of 2 mm or more.";
        var invalidLocale = "invalid";

        //Act
        var result = await _schemasController.GetScoringSchema(invalidLocale);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
        

        var okObjectResult = result as OkObjectResult;
        okObjectResult.Value.Should().BeOfType<ScoreSchema>();
        okObjectResult?.StatusCode.Should().Be(200);

        var scoreSchema = okObjectResult.Value as ScoreSchema;
        scoreSchema.CadDefinition.Should().Be(expectedScoreHeader);
    }

    [Test]
    public async Task GetUserInputFormTemplate_GivenCorrectLocale_ExpectedOkObjectResult()
    {
        //Arrange
        var locale = "en-GB";
        var getTemplateTask = () => _schemasController.GetUserInputFormTemplate(locale);

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<UserInputFormSchema>();
        ((UserInputFormSchema)template!).City.Should().Be("mockedcity");
    }

    [Test]
    public async Task GetUserInputFormTemplate_GivenInvalidLocale_ReturnsOkObjectResult()
    {
        //Arrange
        var locale = "invalid";
        var getTemplateTask = () => _schemasController.GetUserInputFormTemplate(locale);

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert 
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<UserInputFormSchema>();
        ((UserInputFormSchema)template!).City.Should().Be("mockedcity");
    }

    [Test]
    public async Task GetUserInputFormTemplate_GivenNoParameter_ReturnsOkObjectResultWithDefaultLocale()
    {
        //Arrange
        var getTemplateTask = () => _schemasController.GetUserInputFormTemplate();

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert 
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<UserInputFormSchema>();
        ((UserInputFormSchema)template!).City.Should().Be("mockedcity");

    }
}