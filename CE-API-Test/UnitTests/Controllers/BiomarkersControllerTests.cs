using AutoMapper;
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
using System.Security.Claims;

namespace CE_API_Test.UnitTests.Controllers;

[TestFixture]
public class BiomarkersControllerTests
{
    private SchemasController _biomarkersController;
    private IBiomarkersTemplateService _biomarkersTemplateService;
    private IScoringTemplateService _scoringTemplateService;
    private IUserUOW _userUow;
    private IConfigurationRoot _configuration;
    private IUserInputTemplateService _userInputTemplateService;
    private UserHelper _userHelper;

    [OneTimeSetUp]
    public void Setup()
    {
        var extractorMock = new Mock<IUserInformationExtractor>();
        extractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new CE_API_V2.Models.Records.UserIdsRecord() { UserId = "TestUser", TenantId = "TestTenant" });
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();

        var inMemSettings = new Dictionary<string, string>
            {
                { "AiSubpath", Resources.TestResources.AiSubpath }
            };

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemSettings!).Build();

        _userHelper = new UserHelper(mapper, configuration);

        var testConfig = new Dictionary<string, string?>()
        {
            { "EditPeriodInDays", "1" },
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();

        var scoreSummaryUtility = new ScoreSummaryUtility(mapper, _configuration);
        _biomarkersTemplateService = new BiomarkersTemplateService(mapper);

        var userUowMock = new Mock<IUserUOW>();
        userUowMock.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(new UserModel(){ UserId = "123"});
        userUowMock.Setup(u => u.OrderTemplate(It.IsAny<CadRequestSchema>(), It.IsAny<string>())).Returns(_biomarkersTemplateService.GetTemplate().GetAwaiter().GetResult());

        var userInputTemplateService = new Mock<IUserInputTemplateService>();
        _userInputTemplateService = userInputTemplateService.Object;

        _userUow = userUowMock.Object;
        _scoringTemplateService = new ScoringTemplateService(mapper, _biomarkersTemplateService, scoreSummaryUtility, _userUow);
        _biomarkersController = new SchemasController(_biomarkersTemplateService, _scoringTemplateService, _userUow, extractorMock.Object, _userInputTemplateService, _userHelper);
    }

    [Test]
    public async Task GetInputFormTemplate_GivenCorrectLocale_ReturnsAllBiomarkers()
    {
        //Arrange
        var locale = "en-GB";
        var getTemplateTask = () => _biomarkersController.GetInputFormTemplate(locale);

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert 
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<CadRequestSchema>();

        var biomarkersTemplate = (CadRequestSchema)template!;
        biomarkersTemplate.AllMarkers.Count().Should().Be(33);
    }

    [Test]
    public async Task GetInputFormTemplate_GivenInvalidLocale_ReturnsAllBiomarkersWithDefaultLocale()
    {
        //Arrange
        var locale = "invalid";
        var getTemplateTask = () => _biomarkersController.GetInputFormTemplate(locale);

        //Act 
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert 
        result.Subject.Should().BeOfType<OkObjectResult>();

        var template = ((OkObjectResult)result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<CadRequestSchema>();

        var biomarkersTemplate = (CadRequestSchema)template!;
        biomarkersTemplate.AllMarkers.Count().Should().Be(33);
    }
}