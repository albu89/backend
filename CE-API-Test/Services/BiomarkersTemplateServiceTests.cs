using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Utility;
namespace CE_API_Test.Services;

[TestFixture]
public class BiomarkersTemplateServiceTests
{
    private IBiomarkersTemplateService _biomarkersTemplateService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _biomarkersTemplateService = new BiomarkersTemplateService();
    }

    [Test]
    public async Task GetTemplateTest()
    {
        var getTemplateTask = () => _biomarkersTemplateService.GetTemplate();
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<BiomarkersTemplateDTO>();
        result.Subject.Should().NotBeNull();
        result.Subject.BiomarkerList.Should().NotBeNull();
        result.Subject.BiomarkerList.Count().Should().Be(34);
        result.Subject.BiomarkerList.Any(x => string.IsNullOrEmpty(x.Id)).Should().BeFalse();
        result.Subject.BiomarkerList.Any(x => string.IsNullOrEmpty(x.Fieldname)).Should().BeFalse();
        result.Subject.BiomarkerList.Any(x => x.Units is null).Should().BeFalse();
        result.Subject.BiomarkerList.Any(x => x.Units.Length == 0).Should().BeFalse();
        result.Subject.BiomarkerList.All(x => string.IsNullOrEmpty(x.Category)).Should().BeFalse("we expect at least one Item to have a category");
        result.Subject.BiomarkerList.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse("we expect at least one Item to have an InfoText");
    }
}