using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Utility;
using System.Text.Json;

namespace CE_API_Test.UnitTests.Utility;

[TestFixture]
internal class DataTransferUtilityTest
{
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
    public void ConvertStringToScoringResponse_GivenMockedRequestResponse_ReturnResult()
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
    public void ConvertStringToScoringResponse_GivenInvalidRequestResponseString_ReturnNull()
    {
        //Arrange
        var responseAsJson = MockDataProvider.GetMockedSerializedResponse();
        var invalidResponseAsJson = responseAsJson.Remove(1, 3); //invalidate serialized string
        
        //Act
        var result = DataTransferUtility.ToScoringResponse(invalidResponseAsJson);

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public void ConvertStringToScoringResponse_GivenInvalidRequestResponse_ReturnResult()
    {
        //Arrange
        var invalidResponse = new ScoringResponseModel();
        var invalidResponseAsJson = JsonSerializer.Serialize(invalidResponse);

        //Act
        var result = DataTransferUtility.ToScoringResponse(invalidResponseAsJson);

        //Assert
        result.Should().BeOfType<ScoringResponseModel>();
    }

    [Test]
    public void FormatResponse_GivenMockedBiomarkers_ReturnResult()
    {
        //Arrange
        var responseAsJson = MockDataProvider.GetMockedSerializedResponse();

        //Act
        var result = DataTransferUtility.ToScoringResponse(responseAsJson);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ScoringResponseModel));
    }
}