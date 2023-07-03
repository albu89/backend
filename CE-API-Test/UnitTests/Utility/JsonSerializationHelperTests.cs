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
            JsonSerializationHelper.DeserializeObject<ScoringResponse>(scoringResponseJson);

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
            JsonSerializationHelper.DeserializeObject<ScoringRequestDto>(mockedSerializedScoringRequest);

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
            JsonSerializationHelper.DeserializeObject<ScoringResponse>(scoringRequestJson);

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
            JsonSerializationHelper.DeserializeObject<ScoringResponse>(scoringRequestJson);

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

    private void AssertObjectAndPropertiesNotNull(ScoringRequestDto scoringRequestDto)
    {
        AssertNotNull(scoringRequestDto.Age);
        AssertNotNull(scoringRequestDto.ACEInhibitor);
        AssertNotNull(scoringRequestDto.Alat);
        AssertNotNull(scoringRequestDto.Albumin);
        AssertNotNull(scoringRequestDto.AlkalinePhosphatase);
        AssertNotNull(scoringRequestDto.Betablocker);
        AssertNotNull(scoringRequestDto.Bilirubin);
        AssertNotNull(scoringRequestDto.CaAntagonist);
        AssertNotNull(scoringRequestDto.Cholesterol);
        AssertNotNull(scoringRequestDto.CholesterolLowering_Statin);
        AssertNotNull(scoringRequestDto.Diabetes);
        AssertNotNull(scoringRequestDto.DiastolicBloodPressure);
        AssertNotNull(scoringRequestDto.Diuretic);
        AssertNotNull(scoringRequestDto.GlucoseFasting);
        AssertNotNull(scoringRequestDto.Hdl);
        AssertNotNull(scoringRequestDto.Height);
        AssertNotNull(scoringRequestDto.HsTroponinT);
        AssertNotNull(scoringRequestDto.Ldl);
        AssertNotNull(scoringRequestDto.Leukocytes);
        AssertNotNull(scoringRequestDto.Mchc);
        AssertNotNull(scoringRequestDto.NicotineConsumption);
        AssertNotNull(scoringRequestDto.OrganicNitrate);
        AssertNotNull(scoringRequestDto.PancreaticAmylase);
        AssertNotNull(scoringRequestDto.Protein);
        AssertNotNull(scoringRequestDto.RestingECG);
        AssertNotNull(scoringRequestDto.Sex);
        AssertNotNull(scoringRequestDto.SystolicBloodPressure);
        AssertNotNull(scoringRequestDto.TCAggregationInhibitor);
        AssertNotNull(scoringRequestDto.ChestPain);
        AssertNotNull(scoringRequestDto.Urea);
        AssertNotNull(scoringRequestDto.UricAcid);
        AssertNotNull(scoringRequestDto.Weight);
        AssertNotNull(scoringRequestDto.clinical_setting);
        AssertNotNull(scoringRequestDto.prior_CAD);
        
    }

    private void AssertNotNull(object obj)
    {
        obj.Should().NotBeNull();
    }
}