using CE_API_Test.TestUtility;
using CE_API_V2.DTO;
using CE_API_V2.Models;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Utility;

[TestFixture]
internal class DataTransferUtilityTest
{
    private ScoringRequestDto _scoringRequestDto;

    [SetUp]
    public void Setup()
    {
        _scoringRequestDto = MockDataProvider.GetMockedScoringRequestDto();
    }

    [Test]
    public void CreateQueryString_GivenMockedAiDto_ReturnCorrectQueryString()
    {
        //Arrange
        var expectedQueryString = MockDataProvider.GetExpectedQueryString();
        var mockedDto = MockDataProvider.GetMockedAiDto();

        //Act
        var result = DataTransferUtility.CreateQueryString(mockedDto);

        //Assert
        result.Should().BeEquivalentTo(expectedQueryString);
    }

    [Test]
    public void ConvertBiomarkersToAiDto_GivenMockedBiomarkers_ReturnResult()
    {
        //Arrange

        //Act
        var result = DataTransferUtility.ConvertBiomarkersToAiDto(_scoringRequestDto);

        //Assert
        AssertCorrectMapping(result, _scoringRequestDto);
    }

    [Test]
    public void ConvertStringToScoringResponse_GivenMockedBiomarkers_ReturnResult()
    {
        //Arrange
        var responseAsJson = MockDataProvider.GetMockedSerializedResponse();
        var expectedScoringResponse = MockDataProvider.GetMockedScoringResponse();

        //Act
        var result = DataTransferUtility.ToScoringResponse(responseAsJson);

        //Assert
        result.Should().BeEquivalentTo(expectedScoringResponse);
    }


    [Test]
    public void FormatResponse_GivenMockedBiomarkers_ReturnResult()
    {
        //Arrange
        var responseAsJson = MockDataProvider.GetMockedSerializedResponse();
        var expectedTimeStamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        //Act
        var result = DataTransferUtility.FormatResponse(responseAsJson);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ScoringResponse));
        result.timestamp.Should().BeGreaterOrEqualTo(expectedTimeStamp);
    }

    private void AssertCorrectMapping(AiDto result, ScoringRequestDto scoringRequestDto)
    {
        result.Id.Should().BeEmpty();
        // result.Datum.Should().Be(scoringRequestDto.InputDate.Value);
        AssertFloatValue(result.Age, scoringRequestDto.Age.Value);
        AssertFloatValue(result.Sex_0_female_1male, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestDto.Sex.Value));
        AssertFloatValue(result.Gr_sse, scoringRequestDto.Height.Value);
        AssertFloatValue(result.Gewicht, scoringRequestDto.Weight.Value);
        AssertFloatValue(result.Thoraxschmerzen__0_keine_1_extr, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestDto.ChestPain.Value));
        AssertFloatValue(result.Nicotin_0_nein_1_St__N__2_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestDto.NicotineConsumption.Value));

        AssertFloatValue(result.Diabetes_0_no_1_NIDDM_2_IDDM, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestDto.Diabetes.Value));
        AssertFloatValue(result.Statin_od_Chol_senker, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.CholesterolLowering_Statin.Value));
        AssertFloatValue(result.Tc_Aggregation, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.TCAggregationInhibitor.Value));
        AssertFloatValue(result.ACE_od_ATII, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.ACEInhibitor.Value));
        AssertFloatValue(result.CA_Antagonist, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.CaAntagonist.Value));
        AssertFloatValue(result.Betablocker, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.Betablocker.Value));
        AssertFloatValue(result.Diureticum, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.Diuretic.Value));
        AssertFloatValue(result.Nitrat_od_Dancor, TypeToFloatConverter.MapBoolToFloat(scoringRequestDto.OrganicNitrate.Value));

        AssertFloatValue(result.BD_syst, scoringRequestDto.SystolicBloodPressure.Value);
        AssertFloatValue(result.BD_diast, scoringRequestDto.DiastolicBloodPressure.Value);
        AssertFloatValue(result.q_Zacken_0_nein_1_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestDto.RestingECG.Value));

        AssertFloatValue(result.Pankreas_Amylase, scoringRequestDto.PancreaticAmylase.Value);
        AssertFloatValue(result.Alk_Phase, scoringRequestDto.AlkalinePhosphatase.Value);
        AssertFloatValue(result.Troponin, scoringRequestDto.HsTroponinT.Value);
        AssertFloatValue(result.ALAT, scoringRequestDto.Alat.Value);

        AssertFloatValue(result.Glucose, scoringRequestDto.GlocuseFasting.Value);

        AssertFloatValue(result.Bilirubin, scoringRequestDto.Bilirubin.Value);
        AssertFloatValue(result.Harnstoff, scoringRequestDto.Urea.Value);
        AssertFloatValue(result.Harnsaure, scoringRequestDto.UricAcid.Value);

        AssertFloatValue(result.Cholesterin_gesamt, scoringRequestDto.Cholesterol.Value);
        AssertFloatValue(result.HDL, scoringRequestDto.Hdl.Value);
        AssertFloatValue(result.LDL, scoringRequestDto.Ldl.Value);

        AssertFloatValue(result.Total_Proteine, scoringRequestDto.Protein.Value);
        AssertFloatValue(result.Albumin, scoringRequestDto.Albumin.Value);

        AssertFloatValue(result.Leuko, scoringRequestDto.Leukocytes.Value);
        AssertFloatValue(result.MCHC__g_l_oder___, scoringRequestDto.Mchc.Value);

        //Todo - currently unused?
        //result.CustomToken = string.Empty;
        //result.ScroingRecordID = 0;
        //result.incomplete = false;
        //result.chosenOrgClient = string.Empty;
        //result.promocode = string.Empty;
        //result.classifier_type = string.Empty;
        //result.units = string.Empty;
        //result.overwrite = false;
        //result.PopulationRiskLevel = string.Empty;
        //result.PopulationRiskLevel = string.Empty; 
    }

    private void AssertFloatValue(float? actualValue, float expectedValue) =>
        actualValue.Should().BeApproximately(expectedValue, 0.001f);
}