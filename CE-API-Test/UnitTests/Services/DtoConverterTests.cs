
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Services;

[TestFixture]
internal class DtoConverterTests
{
    private ScoringRequestModel _scoringRequestModel;

    [SetUp]
    public void Setup()
    {
        _scoringRequestModel = MockDataProvider.GetMockedScoringRequest();
    }

    [Test]
    public void ConvertBiomarkersToAiDto_GivenMockedBiomarkers_ReturnResult()
    {
        //Arrange

        //Act
        var result = DtoConverter.ConvertToAiDto(_scoringRequestModel.LatestBiomarkers);

        //Assert
        AssertCorrectMapping(result, _scoringRequestModel);
    }

    [Test]
    public void ConvertBiomarkersToAiDto_GivenInvalidBiomarkers_ReturnResult()
    {
        //Arrange
        var scoringRequestModel = new ScoringRequestModel();

        //Act
        var convertToAiDtoTask = () => DtoConverter.ConvertToAiDto(scoringRequestModel.LatestBiomarkers);

        //Assert
        convertToAiDtoTask.Should().Throw<Exception>();
    }

    private void AssertCorrectMapping(AiDto result, ScoringRequestModel scoringRequestModel)
    {
        result.Id.Should().BeEmpty();
        // result.Datum.Should().Be(scoringRequestDto.InputDate.Value);
        AssertFloatValue(result.Age, scoringRequestModel.LatestBiomarkers.Age);
        AssertFloatValue(result.Sex_0_female_1male, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.LatestBiomarkers.Sex));
        AssertFloatValue(result.Gr_sse, scoringRequestModel.LatestBiomarkers.Height);
        AssertFloatValue(result.Gewicht, scoringRequestModel.LatestBiomarkers.Weight);
        AssertFloatValue(result.Thoraxschmerzen__0_keine_1_extr, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.LatestBiomarkers.Chestpain));
        AssertFloatValue(result.Nicotin_0_nein_1_St__N__2_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.LatestBiomarkers.Nicotine));

        AssertFloatValue(result.Diabetes_0_no_1_NIDDM_2_IDDM, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.LatestBiomarkers.Diabetes));
        AssertFloatValue(result.Statin_od_Chol_senker, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Statin));
        AssertFloatValue(result.Tc_Aggregation, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Tcagginhibitor));
        AssertFloatValue(result.ACE_od_ATII, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Aceinhibitor));
        AssertFloatValue(result.CA_Antagonist, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Calciumant));
        AssertFloatValue(result.Betablocker, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Betablocker));
        AssertFloatValue(result.Diureticum, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Diuretic));
        AssertFloatValue(result.Nitrat_od_Dancor, TypeToFloatConverter.MapBoolToFloat(scoringRequestModel.LatestBiomarkers.Nitrate));

        AssertFloatValue(result.BD_syst, scoringRequestModel.LatestBiomarkers.Systolicbp);
        AssertFloatValue(result.BD_diast, scoringRequestModel.LatestBiomarkers.Diastolicbp);
        AssertFloatValue(result.q_Zacken_0_nein_1_ja, TypeToFloatConverter.MapEnumValueToFloat(scoringRequestModel.LatestBiomarkers.Qwave));

        AssertFloatValue(result.Pankreas_Amylase, scoringRequestModel.LatestBiomarkers.Amylasep);
        AssertFloatValue(result.Alk_Phase, scoringRequestModel.LatestBiomarkers.Alkaline);
        AssertFloatValue(result.Troponin, scoringRequestModel.LatestBiomarkers.Hstroponint);
        AssertFloatValue(result.ALAT, scoringRequestModel.LatestBiomarkers.Alat);

        AssertFloatValue(result.Glucose, scoringRequestModel.LatestBiomarkers.Glucose);

        AssertFloatValue(result.Bilirubin, scoringRequestModel.LatestBiomarkers.Bilirubin);
        AssertFloatValue(result.Harnstoff, scoringRequestModel.LatestBiomarkers.Urea);
        AssertFloatValue(result.Harnsaure, scoringRequestModel.LatestBiomarkers.Uricacid);

        AssertFloatValue(result.Cholesterin_gesamt, scoringRequestModel.LatestBiomarkers.Cholesterol);
        AssertFloatValue(result.HDL, scoringRequestModel.LatestBiomarkers.Hdl);
        AssertFloatValue(result.LDL, scoringRequestModel.LatestBiomarkers.Ldl);

        AssertFloatValue(result.Total_Proteine, scoringRequestModel.LatestBiomarkers.Protein);
        AssertFloatValue(result.Albumin, scoringRequestModel.LatestBiomarkers.Albumin);

        AssertFloatValue(result.Leuko, scoringRequestModel.LatestBiomarkers.Leukocyte);
        AssertFloatValue(result.MCHC__g_l_oder___, scoringRequestModel.LatestBiomarkers.Mchc);

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

