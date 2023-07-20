using AutoMapper;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Utility;
using Moq;
using NSubstitute;

namespace CE_API_Test.UnitTests.Services;

[TestFixture]
public class ScoringTemplateServiceTests
{
    private IScoringTemplateService _scoringTemplateService;
    private IBiomarkersTemplateService _biomarkerServiceTemplateService;
    private IScoreSummaryUtility _scoreSummaryUtility;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var biomarkersTemplateMock = new List<BiomarkerSchemaDto> { new() } as IEnumerable<BiomarkerSchemaDto>;
        var biomarkersTemplateService = new Mock<IBiomarkersTemplateService>();
        biomarkersTemplateService.Setup(x => x.GetTemplate(It.IsAny<string>())).Returns(Task.FromResult(biomarkersTemplateMock));

        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();

        _scoreSummaryUtility = new ScoreSummaryUtility(mapper); //No mock needed
        _biomarkerServiceTemplateService = biomarkersTemplateService.Object;
        _scoringTemplateService = new ScoringTemplateService(mapper, _biomarkerServiceTemplateService, _scoreSummaryUtility);
    }

    [Test]
    [TestCase("en-GB", "Score", "Choice of the text based on clinical likelihood, patient characteristics and preference, availability, as well as local expertise.")]
    [TestCase("en-FR", "Score", "Choice of the text based on clinical likelihood, patient characteristics and preference, availability, as well as local expertise.")] // default will be used
    [TestCase("de-DE", "DEDE", "DEDE")]
    public async Task TestGetTemplate_GivenCorrectLocalization_ExpectedCorrectlyAssembledScoreSummary(string locale, string scoreHeader, string localizedInfoText)
    {
        //Arrange

        //Act
        var getTemplateTask = () => _scoringTemplateService.GetTemplate(locale);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        result.Subject.Should().NotBeNull();
        result.Subject.Should().BeOfType<ScoreSummary>();

        result.Subject.ScoreHeader.Should().Be(scoreHeader);
        result.Subject.InfoText.Should().BeEquivalentTo(localizedInfoText);
}

    [Test]
    public async Task GetTemplate_GivenIncorrectCorrectLocalization_ExpectedCorrectlyAssembledDefaultScoreSummary()
    {
        //Arrange
        var locale = "wrongvalue";

        var expectedAbbreviations = new Dictionary<string, string>
        {
            {"CTA", "computed tomography angiography"},
            {"FFR", "fractional flow reserve"},
            {"iwFR", "instantaneous wave-free ratio"}
        };

        //ActPrior
        var getTemplateTask = () => _scoringTemplateService.GetTemplate(locale);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        result.Subject.Should().NotBeNull();
        result.Subject.Should().BeOfType<ScoreSummary>();

        result.Subject.ScoreHeader.Should().Be("Score");
        result.Subject.Score.Should().Be("");
        result.Subject.RiskHeader.Should().Be("Risk of obstructive CAD");
        result.Subject.Risk.Should().Be("");
        result.Subject.RecommendationHeader.Should().BeEquivalentTo("Recommendation");
        result.Subject.Recommendation.Should().BeEquivalentTo("");
        result.Subject.RecommendationExtended.Should().BeEquivalentTo("");
        result.Subject.RecommendationProbabilityHeader.Should().BeEquivalentTo("Probability of obstructive CAD");
        result.Subject.RecommendationScoreRangeHeader.Should().BeEquivalentTo("Score Range");
        result.Subject.RecommendationTableHeader.Should().BeEquivalentTo("Recommendation (ESC 2019 guidelines)");
        result.Subject.WarningHeader.Should().BeEquivalentTo("Warnings");
        result.Subject.Warnings.Should().BeEmpty();
        result.Subject.InfoText.Should()
            .BeEquivalentTo(
                "Choice of the text based on clinical likelihood, patient characteristics and preference, availability, as well as local expertise.");
        result.Subject.Abbreviations.Should().BeEquivalentTo(expectedAbbreviations);
        result.Subject.CadDefinitionHeader.Should().BeEquivalentTo("CAD Definition");
        result.Subject.CadDefinition.Should().BeEquivalentTo(
            "In accordance to current medical guidelines, obstructive coronary artery disease (CAD) is defined as a stenosis or blockage of 50% or more in the diameter of major coronary artery or a branch with a diameter of 2mm or more.");
        result.Subject.FootnoteHeader.Should().BeEquivalentTo("Footnote");
        result.Subject.Footnote.Should().BeEquivalentTo(
            "It's important to note that the management of patients with suspected CAD should be individualized based in the patient's specific risk factors and overall health status. Patients should work closely with their healthcare provider to develop a personalized management plan that meets their needs and goals.");
        result.Subject.IntendedUseHeader.Should().BeEquivalentTo("Intended use");
        result.Subject.IntendedUse.Should().BeEquivalentTo(
            "The Cardio Explorer is intended for all patients who are suspected of having coronary artery disease (CAD) in the medical consultation.");
    }
}