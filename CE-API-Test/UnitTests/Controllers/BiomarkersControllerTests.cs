using AutoMapper;
using CE_API_V2.Controllers;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Controllers;

[TestFixture]
public class BiomarkersControllerTests
{
    private SchemasController _biomarkersController;
    private IBiomarkersTemplateService _biomarkersTemplateService;
    private IScoringTemplateService _scoringTemplateService;
    private IUserUOW _userUow;

    [OneTimeSetUp]
    public void Setup()
    {
        var extractorMock = new Mock<IUserInformationExtractor>();
        extractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new CE_API_V2.Models.Records.UserIdsRecord(){UserId = "TestUser", TenantId = "TestTenant"});
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();

        var scoreSummaryUtility = new ScoreSummaryUtility(mapper);
        _biomarkersTemplateService = new BiomarkersTemplateService(mapper);
        
        var userUowMock = new Mock<IUserUOW>();
        userUowMock.Setup(u => u.GetUser(It.IsAny<string>())).Returns(new User(){ UserId = "123"});
        userUowMock.Setup(u => u.OrderTemplate(It.IsAny<IEnumerable<BiomarkerSchemaDto>>(), It.IsAny<string>())).Returns(_biomarkersTemplateService.GetTemplate().GetAwaiter().GetResult());
        _userUow = userUowMock.Object;
        _scoringTemplateService = new ScoringTemplateService(mapper, _biomarkersTemplateService, scoreSummaryUtility, _userUow);
        _biomarkersController = new SchemasController(_biomarkersTemplateService, _scoringTemplateService, _userUow, extractorMock.Object);
    }

    [Test]
    public async Task TestSchemaEndpoint()
    {
        var getTemplateTask = () => _biomarkersController.GetInputFormTemplate("en-GB");
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<OkObjectResult>();
        var template = ((OkObjectResult) result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<List<BiomarkerSchemaDto>>();
        var biomarkersTemplate = (IEnumerable<BiomarkerSchemaDto>) template!;
        biomarkersTemplate.Count().Should().Be(33);
    }
}