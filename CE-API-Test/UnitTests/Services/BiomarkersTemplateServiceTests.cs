using CE_API_V2.Models.DTO;
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
        _biomarkersTemplateService = new BiomarkersTemplateService();
    }

    [Test]
    public async Task GetTemplateTest()
    {
        var getTemplateTask = () => _biomarkersTemplateService.GetTemplate();
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<List<BiomarkerSchemaDto>>();
        result.Subject.Should().NotBeNull();
        result.Subject.Should().NotBeNull();
        result.Subject.Count().Should().Be(35);
        result.Subject.Any(x => string.IsNullOrEmpty(x.Id)).Should().BeFalse();
        result.Subject.Any(x => string.IsNullOrEmpty(x.Fieldname)).Should().BeFalse();
        result.Subject.Any(x => x.Units is null).Should().BeFalse();
        result.Subject.Any(x => x.Units.Length == 0).Should().BeFalse();
        result.Subject.All(x => string.IsNullOrEmpty(x.Category)).Should().BeFalse("we expect at least one Item to have a category");
        result.Subject.All(x => string.IsNullOrEmpty(x.InfoText)).Should().BeFalse("we expect at least one Item to have an InfoText");
    }
}