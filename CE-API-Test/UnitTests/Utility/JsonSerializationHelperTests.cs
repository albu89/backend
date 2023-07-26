using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Utility;

[TestFixture]
internal class JsonSerializationHelperTests
{

    [Test]
    public void DeserializeObject_GivenScoringResponse_ReturnDeserializedObject()
    {
        //Arrange
        var scoringResponseJson = MockDataProvider.GetMockedSerializedResponse();

        //Act
        var deserializedScoringResponse =
            JsonSerializationHelper.DeserializeObject<ScoringResponseModel>(scoringResponseJson);

        //Assert
        AssertObjectAndPropertiesNotNull(deserializedScoringResponse);
    }

    [Test]
    public async Task DeserializeObject_GivenScoringRequestDto_ReturnDeserializedObject()
    {
        //Arrange
        var mockedSerializedScoringRequest = MockDataProvider.GetMockedSerializedScoringRequestDto();

        //Act
        var deserializedscoringRequest =
            JsonSerializationHelper.DeserializeObject<ScoringRequest>(mockedSerializedScoringRequest);

        //Assert
        AssertObjectAndPropertiesNotNull(deserializedscoringRequest);
    }

    [Test]
    public void DeserializeObject_GivenIncorrectTypeArgument_ReturnDeserializedObject()
    {
        //Arrange
        var scoringRequestJson = MockDataProvider.GetMockedSerializedScoringRequestDto();

        //Act
        var deserializedscoringRequest =
            JsonSerializationHelper.DeserializeObject<ScoringResponseModel>(scoringRequestJson);

        //Assert
        deserializedscoringRequest.Should().NotBeNull();
    }

    [Test]
    public void DeserializeObject_GivenNull_ReturnDeserializedObject()
    {
        //Arrange
        string scoringRequestJson = null;

        //Act
        var deserializedscoringRequest =
            JsonSerializationHelper.DeserializeObject<ScoringResponseModel>(scoringRequestJson);

        //Assert
        deserializedscoringRequest.Should().BeNull();
    }

    private void AssertObjectAndPropertiesNotNull(object obj)
    {
        obj.Should().NotBeNull();

        var properties = obj.GetType().GetProperties().Where(x => x.Name.Contains("ScoringRequest"));

        foreach (var property in properties)
        {

            var value = property.GetValue(obj);
            value.Should().NotBeNull();
        }
    }

    private void AssertObjectAndPropertiesNotNull(ScoringRequest scoringRequest)
    {
        AssertNotNull(scoringRequest.Age);
        AssertNotNull(scoringRequest.ACEInhibitor);
        AssertNotNull(scoringRequest.Alat);
        AssertNotNull(scoringRequest.Albumin);
        AssertNotNull(scoringRequest.AlkalinePhosphatase);
        AssertNotNull(scoringRequest.Betablocker);
        AssertNotNull(scoringRequest.Bilirubin);
        AssertNotNull(scoringRequest.CaAntagonist);
        AssertNotNull(scoringRequest.Cholesterol);
        AssertNotNull(scoringRequest.CholesterolLowering_Statin);
        AssertNotNull(scoringRequest.Diabetes);
        AssertNotNull(scoringRequest.DiastolicBloodPressure);
        AssertNotNull(scoringRequest.Diuretic);
        AssertNotNull(scoringRequest.GlucoseFasting);
        AssertNotNull(scoringRequest.Hdl);
        AssertNotNull(scoringRequest.Height);
        AssertNotNull(scoringRequest.HsTroponinT);
        AssertNotNull(scoringRequest.Ldl);
        AssertNotNull(scoringRequest.Leukocytes);
        AssertNotNull(scoringRequest.Mchc);
        AssertNotNull(scoringRequest.NicotineConsumption);
        AssertNotNull(scoringRequest.OrganicNitrate);
        AssertNotNull(scoringRequest.PancreaticAmylase);
        AssertNotNull(scoringRequest.Protein);
        AssertNotNull(scoringRequest.RestingECG);
        AssertNotNull(scoringRequest.Sex);
        AssertNotNull(scoringRequest.SystolicBloodPressure);
        AssertNotNull(scoringRequest.TCAggregationInhibitor);
        AssertNotNull(scoringRequest.ChestPain);
        AssertNotNull(scoringRequest.Urea);
        AssertNotNull(scoringRequest.UricAcid);
        AssertNotNull(scoringRequest.Weight);
        AssertNotNull(scoringRequest.clinical_setting);
        AssertNotNull(scoringRequest.prior_CAD);
        
    }

    private void AssertNotNull(object obj)
    {
        obj.Should().NotBeNull();
    }
}