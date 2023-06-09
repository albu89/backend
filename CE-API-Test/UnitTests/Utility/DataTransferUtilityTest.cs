using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Utility;

[TestFixture]
internal class DataTransferUtilityTest
{
    private ScoringRequest _scoringRequest;

    [SetUp]
    public void Setup()
    {
        _scoringRequest = MockDataProvider.GetMockedScoringRequest();
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
        var result = DtoConverter.ConvertToAiDto(_scoringRequest.Biomarkers);

        //Assert
        AssertCorrectMapping(result, _scoringRequest);
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

        //Act
        var result = DataTransferUtility.FormatResponse(responseAsJson);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ScoringResponse));
    }

    private void AssertCorrectMapping(AiDto result, ScoringRequest scoringRequest)
    {
        result.Id.Should().BeEmpty();
        // result.Datum.Should().Be(scoringRequestDto.InputDate.Value);
        AssertFloatValue(result.Age, scoringRequest.Biomarkers.Age);
        AssertFloatValue(result.Sex_0_female_1male, TypeToFloatConverter.MapEnumValueToFloat(scoringRequest.Biomarkers.Sex));
        AssertFloatValue(result.Gr_sse, scoringRequest.Biomarkers.Height);
        AssertFloatValue(result.Gewicht, scoringRequest.Biomarkers.Weight);
        AssertFloatValue(result.Thoraxschmerzen__0_keine_1_extr, TypeToFloatConverter.MapEnumValueToFloat(scoringRequest.Biomarkers.ChestPain));
        AssertFloatValue(result.Nicotin_0_nein_1_St__N__2_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequest.Biomarkers.Nicotine));

        AssertFloatValue(result.Diabetes_0_no_1_NIDDM_2_IDDM, TypeToFloatConverter.MapEnumValueToFloat(scoringRequest.Biomarkers.Diabetes));
        AssertFloatValue(result.Statin_od_Chol_senker, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.Statin));
        AssertFloatValue(result.Tc_Aggregation, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.TcAggInhibitor));
        AssertFloatValue(result.ACE_od_ATII, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.AceInhibitor));
        AssertFloatValue(result.CA_Antagonist, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.CaAntagonist));
        AssertFloatValue(result.Betablocker, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.Betablocker));
        AssertFloatValue(result.Diureticum, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.Diuretic));
        AssertFloatValue(result.Nitrat_od_Dancor, TypeToFloatConverter.MapBoolToFloat(scoringRequest.Biomarkers.OganicNitrate));

        AssertFloatValue(result.BD_syst, scoringRequest.Biomarkers.SystolicBloodPressure);
        AssertFloatValue(result.BD_diast, scoringRequest.Biomarkers.DiastolicBloodPressure);
        AssertFloatValue(result.q_Zacken_0_nein_1_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequest.Biomarkers.RestingECG));

        AssertFloatValue(result.Pankreas_Amylase, scoringRequest.Biomarkers.PancreaticAmylase);
        AssertFloatValue(result.Alk_Phase, scoringRequest.Biomarkers.AlkalinePhosphate);
        AssertFloatValue(result.Troponin, scoringRequest.Biomarkers.HsTroponinT);
        AssertFloatValue(result.ALAT, scoringRequest.Biomarkers.Alat);

        AssertFloatValue(result.Glucose, scoringRequest.Biomarkers.Glucose);

        AssertFloatValue(result.Bilirubin, scoringRequest.Biomarkers.Bilirubin);
        AssertFloatValue(result.Harnstoff, scoringRequest.Biomarkers.Urea);
        AssertFloatValue(result.Harnsaure, scoringRequest.Biomarkers.UricAcid);

        AssertFloatValue(result.Cholesterin_gesamt, scoringRequest.Biomarkers.Cholesterol);
        AssertFloatValue(result.HDL, scoringRequest.Biomarkers.Hdl);
        AssertFloatValue(result.LDL, scoringRequest.Biomarkers.Ldl);

        AssertFloatValue(result.Total_Proteine, scoringRequest.Biomarkers.Protein);
        AssertFloatValue(result.Albumin, scoringRequest.Biomarkers.Albumin);

        AssertFloatValue(result.Leuko, scoringRequest.Biomarkers.Leukocytes);
        AssertFloatValue(result.MCHC__g_l_oder___, scoringRequest.Biomarkers.Mchc);

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