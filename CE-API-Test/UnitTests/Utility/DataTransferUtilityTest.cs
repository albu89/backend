using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Utility;

[TestFixture]
internal class DataTransferUtilityTest
{
    private ScoringRequestModel _scoringRequestModel;

    [SetUp]
    public void Setup()
    {
        _scoringRequestModel = MockDataProvider.GetMockedScoringRequest();
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
        var result = DtoConverter.ConvertToAiDto(_scoringRequestModel.Biomarkers);

        //Assert
        AssertCorrectMapping(result, _scoringRequestModel);
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
        result.Should().BeOfType(typeof(ScoringResponseModel));
    }

    private void AssertCorrectMapping(AiDto result, ScoringRequestModel scoringRequestModel)
    {
        result.Id.Should().BeEmpty();
        // result.Datum.Should().Be(scoringRequestDto.InputDate.Value);
        AssertFloatValue(result.Age, scoringRequestModel.Biomarkers.Age);
        AssertFloatValue(result.Sex_0_female_1male, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.Biomarkers.Sex));
        AssertFloatValue(result.Gr_sse, scoringRequestModel.Biomarkers.Height);
        AssertFloatValue(result.Gewicht, scoringRequestModel.Biomarkers.Weight);
        AssertFloatValue(result.Thoraxschmerzen__0_keine_1_extr, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.Biomarkers.ChestPain));
        AssertFloatValue(result.Nicotin_0_nein_1_St__N__2_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.Biomarkers.Nicotine));

        AssertFloatValue(result.Diabetes_0_no_1_NIDDM_2_IDDM, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.Biomarkers.Diabetes));
        AssertFloatValue(result.Statin_od_Chol_senker, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.Statin));
        AssertFloatValue(result.Tc_Aggregation, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.TcAggInhibitor));
        AssertFloatValue(result.ACE_od_ATII, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.AceInhibitor));
        AssertFloatValue(result.CA_Antagonist, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.CaAntagonist));
        AssertFloatValue(result.Betablocker, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.Betablocker));
        AssertFloatValue(result.Diureticum, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.Diuretic));
        AssertFloatValue(result.Nitrat_od_Dancor, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.Biomarkers.OganicNitrate));

        AssertFloatValue(result.BD_syst, scoringRequestModel.Biomarkers.SystolicBloodPressure);
        AssertFloatValue(result.BD_diast, scoringRequestModel.Biomarkers.DiastolicBloodPressure);
        AssertFloatValue(result.q_Zacken_0_nein_1_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.Biomarkers.RestingECG));

        AssertFloatValue(result.Pankreas_Amylase, scoringRequestModel.Biomarkers.PancreaticAmylase);
        AssertFloatValue(result.Alk_Phase, scoringRequestModel.Biomarkers.AlkalinePhosphate);
        AssertFloatValue(result.Troponin, scoringRequestModel.Biomarkers.HsTroponinT);
        AssertFloatValue(result.ALAT, scoringRequestModel.Biomarkers.Alat);

        AssertFloatValue(result.Glucose, scoringRequestModel.Biomarkers.Glucose);

        AssertFloatValue(result.Bilirubin, scoringRequestModel.Biomarkers.Bilirubin);
        AssertFloatValue(result.Harnstoff, scoringRequestModel.Biomarkers.Urea);
        AssertFloatValue(result.Harnsaure, scoringRequestModel.Biomarkers.UricAcid);

        AssertFloatValue(result.Cholesterin_gesamt, scoringRequestModel.Biomarkers.Cholesterol);
        AssertFloatValue(result.HDL, scoringRequestModel.Biomarkers.Hdl);
        AssertFloatValue(result.LDL, scoringRequestModel.Biomarkers.Ldl);

        AssertFloatValue(result.Total_Proteine, scoringRequestModel.Biomarkers.Protein);
        AssertFloatValue(result.Albumin, scoringRequestModel.Biomarkers.Albumin);

        AssertFloatValue(result.Leuko, scoringRequestModel.Biomarkers.Leukocytes);
        AssertFloatValue(result.MCHC__g_l_oder___, scoringRequestModel.Biomarkers.Mchc);

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