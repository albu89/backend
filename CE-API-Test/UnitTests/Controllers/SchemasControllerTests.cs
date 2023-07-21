using System.Collections;
using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Controllers;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
namespace CE_API_Test.UnitTests.Controllers;

[TestFixture]
public class SchemasControllerTests
{
    private SchemasController _schemasController;
    private IBiomarkersTemplateService _biomarkersTemplateService;
    private IScoringTemplateService _scoringTemplateService;
    private IScoreSummaryUtility _scoreSummaryUtility;
    private IUserUOW _userUow;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();
        _biomarkersTemplateService = new BiomarkersTemplateService(mapper);
        _scoreSummaryUtility = new ScoreSummaryUtility(mapper);
        

        var userUow = new Mock<IUserUOW>();
        userUow.Setup(x => x.GetBiomarkerOrders(It.IsAny<string>())).Returns(MockDataProvider.GetMockedOrderModels());
        userUow.Setup(x => x.GetUser(It.IsAny<string>())).Returns(MockDataProvider.GetMockedUser);
        var template = await _biomarkersTemplateService.GetTemplate();
        _scoringTemplateService = new ScoringTemplateService(mapper, _biomarkersTemplateService, _scoreSummaryUtility, userUow.Object);
        userUow.Setup(x => x.OrderTemplate(It.IsAny<IEnumerable<BiomarkerSchemaDto>>(), It.IsAny<string>())).Returns(template);
        _userUow = userUow.Object;
        
        _schemasController = new SchemasController(_biomarkersTemplateService, _scoringTemplateService, _userUow, new UserInformationExtractor());
    }

    [Test]
    public async Task TestSchemaEndpoint()
    {
        var getTemplateTask = () => _schemasController.GetInputFormTemplate("en-GB");
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<OkObjectResult>();
        var template = ((OkObjectResult) result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<List<BiomarkerSchemaDto>>();
        var biomarkersTemplate = (IEnumerable<BiomarkerSchemaDto>) template;
        biomarkersTemplate.Count().Should().Be(33);
    }

    [Test]
    public async Task GetScoringSchema_WithoutParameter_ReturnOkRequestResult()
    {
        //Arrange
        
        //Act
        var result = await _schemasController.GetScoringSchema();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));

        var okObjectResult = result as OkObjectResult;
        okObjectResult.Value.Should().BeOfType<ScoreSummary>();
        okObjectResult?.StatusCode.Should().Be(200);
    }
}