using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Utility;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NSubstitute;

namespace CE_API_Test.UnitTests.Services;

[TestFixture]
public class ScoreSummaryUtilityTests
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
    [TestCase("en-GB", "No diagnostic testing mandated.")]
    [TestCase("de-DE", "DEDE")]
    public async Task SetAdditionalScoringParams_GivenVariousLocales_ExpectedAddedMissingParameters(string locale, string expectedRecommendationSummary)
    {
        //Arrange
        var scoreSummary = MockDataProvider.GetScoringResponseSummaryMock();
        scoreSummary.RiskValue = null;
        scoreSummary.RecommendationLongText = null;
        scoreSummary.RecommendationSummary = null;
        scoreSummary.Warnings = null;
        scoreSummary.Warnings = null;

        //Act
        var result = _scoreSummaryUtility.SetAdditionalScoringParams(scoreSummary, locale);

        //Assert
        result.Should().NotBeNull();
        result.RiskValue.Should().NotBeNull();
        result.RecommendationLongText.Should().NotBeNull();
        result.RecommendationSummary.Should().NotBeNull();
        result.RecommendationSummary.Should().Be(expectedRecommendationSummary);
        result.Warnings.Should().NotBeNull();
    }

    [Test]
    [TestCase(0.10, "<5%")]
    [TestCase(0.30, "20%")]
    [TestCase(0.70, "50%")]
    [TestCase(0.74, ">75%")]
    public async Task SetAdditionalScoringParams_GivenVariousScores_ExpectedAddedMissingParameters(double score, string expectedRiskValue)
    {
        //Arrange
        var scoreSummary = MockDataProvider.GetScoringResponseSummaryMock();
        scoreSummary.RiskValue = null;
        scoreSummary.RecommendationLongText = null;
        scoreSummary.RecommendationSummary = null;
        scoreSummary.Warnings = null;
        scoreSummary.classifier_score = score;

        //Act
        var result = _scoreSummaryUtility.SetAdditionalScoringParams(scoreSummary, "en-GB");

        //Assert
        result.Should().NotBeNull();
        result.RiskValue.Should().NotBeNull();
        result.RiskValue.Should().Be(expectedRiskValue);
        result.RecommendationLongText.Should().NotBeNull();
        result.RecommendationSummary.Should().NotBeNull();
        result.Warnings.Should().NotBeNull();
    }

    [Test]
    [TestCase("en-GB")]
    [TestCase("de-DE")]
    [TestCase("fr-FR")]

    public async Task GetCategories_GivenCorrectLocalization_ExpectedCategories(string locale)
    {
        //Arrange

        //Act
        var result = _scoreSummaryUtility.GetCategories(locale);

        //Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(4);
        result.Any(x => x.RiskValue.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.Id ==0).Should().BeFalse();
        result.Any(x => x.LongText.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.LowerLimit.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.ShortText.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.UpperLimit.IsNullOrEmpty()).Should().BeFalse();
    }

    [Test]
    public async Task GetTemplate_GivenCorrectLocalization_ExpectedCorrectlyAssembledDefaultScoringSchemaList()
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
        var template = result.Subject;
        template.Should().NotBeNull();
        template.Should().BeOfType<ScoreSummary>();

        template.ScoreHeader.Should().Be("Score");
        template.Score.Should().Be("");
        template.RiskHeader.Should().Be("Risk of obstructive CAD");
        template.Risk.Should().Be("");
        template.RecommendationHeader.Should().BeEquivalentTo("Recommendation");
        template.Recommendation.Should().BeEquivalentTo("");
        template.RecommendationExtended.Should().BeEquivalentTo("");
        template.RecommendationProbabilityHeader.Should().BeEquivalentTo("Probability of obstructive CAD");
        template.RecommendationTableHeader.Should().BeEquivalentTo("Recommendation (ESC 2019 guidelines)");
        template.RecommendationScoreRangeHeader.Should().BeEquivalentTo("Score Range");
        template.WarningHeader.Should().BeEquivalentTo("Warnings");
        template.Warnings.Should().BeEmpty();
        template.InfoText.Should()
            .BeEquivalentTo(
                "Choice of the text based on clinical likelihood, patient characteristics and preference, availability, as well as local expertise.");
        template.Abbreviations.Should().BeEquivalentTo(expectedAbbreviations);
        template.CadDefinitionHeader.Should().BeEquivalentTo("CAD Definition");
        template.CadDefinition.Should().BeEquivalentTo(
            "In accordance to current medical guidelines, obstructive coronary artery disease (CAD) is defined as a stenosis or blockage of 50% or more in the diameter of major coronary artery or a branch with a diameter of 2mm or more.");
        template.FootnoteHeader.Should().BeEquivalentTo("Footnote");
        template.Footnote.Should().BeEquivalentTo(
            "It's important to note that the management of patients with suspected CAD should be individualized based in the patient's specific risk factors and overall health status. Patients should work closely with their healthcare provider to develop a personalized management plan that meets their needs and goals.");
        template.IntendedUseHeader.Should().BeEquivalentTo("Intended use");
        template.IntendedUse.Should().BeEquivalentTo(
            "The Cardio Explorer is intended for all patients who are suspected of having coronary artery disease (CAD) in the medical consultation.");
    }
}