using CE_API_V2.Controllers;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Mvc;
namespace CE_API_Test.UnitTests.Controllers;

[TestFixture]
public class BiomarkersControllerTests
{
    private BiomarkersController _biomarkersController;
    private IBiomarkersTemplateService _templateService;

    [OneTimeSetUp]
    public void Setup()
    {
        _templateService = new BiomarkersTemplateService();
        _biomarkersController = new BiomarkersController(_templateService);
    }

    [Test]
    public async Task TestSchemaEndpoint()
    {
        var getTemplateTask = () => _biomarkersController.GetInputFormTemplate();
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<OkObjectResult>();
        var template = ((OkObjectResult) result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<BiomarkersTemplateDTO>();
        var biomarkersTemplate = (BiomarkersTemplateDTO) template;
        biomarkersTemplate.BiomarkerList.Count().Should().Be(34);
    }
}