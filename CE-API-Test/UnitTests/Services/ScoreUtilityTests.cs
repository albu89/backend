using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using static CE_API_V2.Models.Enum.PatientDataEnums;

namespace CE_API_Test.UnitTests.Services;

[TestFixture]
public class ScoreUtilityTests
{
    private IScoringTemplateService _scoringTemplateService;
    private IBiomarkersTemplateService _biomarkerServiceTemplateService;
    private IScoreSummaryUtility _scoreSummaryUtility;
    private IConfigurationRoot _configuration;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var biomarkersTemplateMock = new CadRequestSchema();
        var biomarkersTemplateService = new Mock<IBiomarkersTemplateService>();
        biomarkersTemplateService.Setup(x => x.GetTemplate(It.IsAny<string>())).Returns(Task.FromResult(biomarkersTemplateMock));

        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();

        var userUowMock = new Mock<IUserUOW>();
        userUowMock.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(new UserModel() { UserId = "123" });
        userUowMock.Setup(u => u.OrderTemplate(It.IsAny<CadRequestSchema>(), It.IsAny<string>())).Returns(biomarkersTemplateService.Object.GetTemplate("").GetAwaiter().GetResult());

        
        var testConfig = new Dictionary<string, string?>()
        {
            { "EditPeriodInDays", "1" },
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();
        
        _scoreSummaryUtility = new ScoreSummaryUtility(mapper, _configuration); //No mock needed
        _biomarkerServiceTemplateService = biomarkersTemplateService.Object;
        _scoringTemplateService = new ScoringTemplateService(mapper, _biomarkerServiceTemplateService, _scoreSummaryUtility, userUowMock.Object);
    }

    [Test]
    [TestCase("en-GB", "No diagnostic testing mandated.")]
    [TestCase("de-DE", "Keine diagnostischen Tests vorgeschrieben.")]
    public void SetAdditionalScoringParams_GivenVariousLocales_ExpectedAddedMissingParameters(string locale, string expectedRecommendationSummary)
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
    [TestCase("invalid", "No diagnostic testing mandated.")]
    [TestCase("", "No diagnostic testing mandated.")]
    [TestCase(null, "No diagnostic testing mandated.")]
    public void SetAdditionalScoringParams_GivenInvalidLocale_ExpectedAddedMissingParametersWithDefaultLocale(string locale, string expectedRecommendationSummary)
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
    [TestCase(0.711993, "<5%", ClinicalSetting.PrimaryCare, false)]
    [TestCase(0.711994, "20%", ClinicalSetting.PrimaryCare, false)]
    [TestCase(0.8714438, "50%", ClinicalSetting.PrimaryCare, false)]
    [TestCase(0.9154612, ">75%", ClinicalSetting.PrimaryCare, false)]
    [TestCase(0.1199, "<5%", ClinicalSetting.PrimaryCare, true)] //ExpectedSecondary
    [TestCase(0.1203604, "20%", ClinicalSetting.PrimaryCare, true)] //ExpectedSecondary
    [TestCase(0.31999, "20%", ClinicalSetting.PrimaryCare, true)] //ExpectedSecondary
    [TestCase(0.32, "50%", ClinicalSetting.PrimaryCare, true)] //ExpectedSecondary
    [TestCase(0.72999, "50%", ClinicalSetting.PrimaryCare, true)] //ExpectedSecondary
    [TestCase(0.73, ">90%", ClinicalSetting.PrimaryCare, true)] //ExpectedSecondary
    [TestCase(0.1199, "<5%", ClinicalSetting.SecondaryCare, false)] //ExpectedSecondary
    [TestCase(0.1203604, "20%", ClinicalSetting.SecondaryCare, false)] //ExpectedSecondary
    [TestCase(0.3199, "20%", ClinicalSetting.SecondaryCare, false)] //ExpectedSecondary
    [TestCase(0.32, "50%", ClinicalSetting.SecondaryCare, false)] //ExpectedSecondary
    [TestCase(0.72988, "50%", ClinicalSetting.SecondaryCare, false)] //ExpectedSecondary
    [TestCase(0.73, ">90%", ClinicalSetting.SecondaryCare, false)] //ExpectedSecondary
    [TestCase(0.1199, "<5%", ClinicalSetting.SecondaryCare, true)] //ExpectedSecondary
    [TestCase(0.1203604, "20%", ClinicalSetting.SecondaryCare, true)] //ExpectedSecondary
    [TestCase(0.31, "20%", ClinicalSetting.SecondaryCare, true)] //ExpectedSecondary
    [TestCase(0.3112, "20%", ClinicalSetting.SecondaryCare, true)] //ExpectedSecondary
    [TestCase(0.72999, "50%", ClinicalSetting.SecondaryCare, true)] //ExpectedSecondary
    [TestCase(0.73000, ">90%", ClinicalSetting.SecondaryCare, true)] //ExpectedSecondary
    public void SetAdditionalScoringParams_GivenVariousScoresAndClinicalSettings_ExpectedAddedMissingParameters(double score, string expectedRiskValue, ClinicalSetting clinicalSetting, bool priorCad)
    {
        //Arrange
        var scoreSummary = MockDataProvider.GetScoringResponseSummaryMock();
        scoreSummary.RiskValue = null;
        scoreSummary.RecommendationLongText = null;
        scoreSummary.RecommendationSummary = null;
        scoreSummary.Warnings = null;
        scoreSummary.classifier_score = score;
        var indexPriorCad = Array.FindIndex(scoreSummary.Biomarkers.Values, values => values.Id == "prior_CAD");
        var indexClinicalSettingd = Array.FindIndex(scoreSummary.Biomarkers.Values, values => values.Id == "clinical_setting");

        scoreSummary.Biomarkers.Values[indexPriorCad] = new()
        {
            Id = "prior_CAD",
            Value = priorCad,
            DisplayValue = priorCad.ToString(),
            Unit = "SI"
        };

        scoreSummary.Biomarkers.Values[indexClinicalSettingd] = new()
        {
            Id = "clinical_setting",
            Value = clinicalSetting,
            DisplayValue = clinicalSetting.ToString(),
            Unit = "SI"
        };

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
    public async Task GetCategories_GivenCorrectLocalization_ReturnExpectedCategories(string locale)
    {
        //Arrange

        //Act
        var result = _scoreSummaryUtility.GetCategories(locale);

        //Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(4);
        result.Any(x => x.RiskValue.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.Id == 0).Should().BeFalse();
        result.Any(x => x.LongText.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.LowerLimit.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.ShortText.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.UpperLimit.IsNullOrEmpty()).Should().BeFalse();
    }

    [Test]
    [TestCase("invalid")]
    [TestCase("")]
    [TestCase(null)]
    public async Task GetCategories_GivenIncorrectLocalization_ReturnExpectedCategoriesWithFallBackLocale(string locale)
    {
        //Arrange

        //Act
        var result = _scoreSummaryUtility.GetCategories(locale);

        //Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(4);
        result.Any(x => x.RiskValue.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.Id == 0).Should().BeFalse();
        result.Any(x => x.LongText.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.LowerLimit.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.ShortText.IsNullOrEmpty()).Should().BeFalse();
        result.Any(x => x.UpperLimit.IsNullOrEmpty()).Should().BeFalse();
    }

    [Test]
    public async Task GetTemplate_GivenEnglishLocale_ReturnExpectedCorrectlyAssembledDefaultScoringSchemaList()
    {
        //Arrange
        var locale = "en-GB";

        var expectedAbbreviations = new Dictionary<string, string>
        {
            { "CTA", "computed tomography angiography" },
            { "FFR", "fractional flow reserve" },
            { "iwFR", "instantaneous wave-free ratio" }
        };

        //Act
        var getTemplateTask = () => _scoringTemplateService.GetTemplate("123", locale);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        var template = result.Subject;
        template.Should().NotBeNull();
        template.Should().BeOfType<ScoreSchema>();

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
            "In accordance to current medical guidelines, obstructive CAD is defined as a stenosis or blockage of 50% or more in the diameter of a major coronary artery or a branch with a diameter of 2 mm or more.");
        template.FootnoteHeader.Should().BeEquivalentTo("Footnote");
        template.Footnote.Should().BeEquivalentTo(
            "It's important to note that the management of patients with suspected CAD should be individualized based on the patient's specific risk factors and overall health status.");
        template.IntendedUseHeader.Should().BeEquivalentTo("Intended use");
        template.IntendedUse.Should().BeEquivalentTo(
            "The Cardio Explorer is intended for all patients who are suspected of having coronary artery disease (CAD) in the medical consultation.");
    }

    [Test]
    public async Task GetTemplate_GivenGermanLocale_ReturnExpectedCorrectlyAssembledDefaultScoringSchemaList() //Locale should not be the same as the default locale
    {
        //Arrange
        var locale = "de-DE";

        var expectedAbbreviations = new Dictionary<string, string>
        {
            { "CTA", "Computertomographie-Angiographie" },
            { "FFR", "Fraktionelle Flussreserve" },
            { "iwFR", "instantaneous wave-free ratio" }
        };

        //Act
        var getTemplateTask = () => _scoringTemplateService.GetTemplate("123", locale);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        var template = result.Subject;
        template.Should().NotBeNull();
        template.Should().BeOfType<ScoreSchema>();

        template.ScoreHeader.Should().Be("Score");
        template.Score.Should().Be("");
        template.RiskHeader.Should().Be("Risiko einer obstruktiven KHK");
        template.Risk.Should().Be("Risiko"); //Todo: is this correct? in English its empty
        template.RecommendationHeader.Should().BeEquivalentTo("Empfehlung");
        template.Recommendation.Should().BeEquivalentTo("");
        template.RecommendationExtended.Should().BeEquivalentTo("");
        template.RecommendationProbabilityHeader.Should().BeEquivalentTo("Wahrscheinlichkeit einer obstruktiven KHK");
        template.RecommendationTableHeader.Should().BeEquivalentTo("Empfehlung (ESC 2019 guidelines)");
        template.RecommendationScoreRangeHeader.Should().BeEquivalentTo("Score Skala");
        template.WarningHeader.Should().BeEquivalentTo("Warnungen");
        template.Warnings.Should().BeEmpty();
        template.InfoText.Should()
            .BeEquivalentTo(
                "Auswahl des Textes basierend auf klinischer Wahrscheinlichkeit, Patientenmerkmalen und -präferenzen, Verfügbarkeit sowie lokaler Expertise.");
        template.Abbreviations.Should().BeEquivalentTo(expectedAbbreviations); //TODO: "iwFR" is not properly localized in german!!! 
        template.CadDefinitionHeader.Should().BeEquivalentTo("KHK Definition"); 
        template.CadDefinition.Should().BeEquivalentTo(
            "Gemäß den aktuellen medizinischen Leitlinien ist eine obstruktive koronare Herzkrankheit (KHK) definiert als eine Stenose oder Blockade von 50 % oder mehr im Durchmesser einer großen Koronararterie oder eines Astes mit einem Durchmesser von 2 mm oder mehr.");
        template.FootnoteHeader.Should().BeEquivalentTo("Fußnote");
        template.Footnote.Should().BeEquivalentTo(
            "Es ist wichtig zu beachten, dass die Behandlung von Patienten mit Verdacht auf eine koronare Herzkrankheit individuell auf der Grundlage der spezifischen Risikofaktoren und des allgemeinen Gesundheitszustands des Patienten erfolgen sollte. Patienten sollten eng mit ihrem Gesundheitsdienstleister zusammenarbeiten, um einen personalisierten Behandlungsplan zu entwickeln, der ihren Bedürfnissen und Zielen entspricht.");
        template.IntendedUseHeader.Should().BeEquivalentTo("Verwendungszweck");
        template.IntendedUse.Should().BeEquivalentTo(
            "Der Cardio Explorer ist für alle Patienten gedacht, bei denen in der ärztlichen Konsultation der Verdacht auf eine koronare Herzkrankheit (KHK) besteht.");
    }

    [Test]
    public async Task GetTemplate_GiveIncorrectLocalization_ReturnExpectedCorrectlyAssembledDefaultScoringSchemaList()
    {
        //Arrange
        var locale = "wrongvalue";

        var expectedAbbreviations = new Dictionary<string, string>
        {
            { "CTA", "computed tomography angiography" },
            { "FFR", "fractional flow reserve" },
            { "iwFR", "instantaneous wave-free ratio" }
        };

        //ActPrior
        var getTemplateTask = () => _scoringTemplateService.GetTemplate("123", locale);
        var result = await getTemplateTask.Should().NotThrowAsync();

        //Assert
        var template = result.Subject;
        template.Should().NotBeNull();
        template.Should().BeOfType<ScoreSchema>();

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
            "In accordance to current medical guidelines, obstructive CAD is defined as a stenosis or blockage of 50% or more in the diameter of a major coronary artery or a branch with a diameter of 2 mm or more."); template.FootnoteHeader.Should().BeEquivalentTo("Footnote");
        template.Footnote.Should().BeEquivalentTo(
            "It's important to note that the management of patients with suspected CAD should be individualized based on the patient's specific risk factors and overall health status.");
        template.IntendedUseHeader.Should().BeEquivalentTo("Intended use");
        template.IntendedUse.Should().BeEquivalentTo(
            "The Cardio Explorer is intended for all patients who are suspected of having coronary artery disease (CAD) in the medical consultation.");
    }
}