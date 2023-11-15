using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Services.Interfaces;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class AiRequestServiceTests
    {
        private IAiRequestService _sut;
        private ScoringRequestModel _scoringRequestModel;

        [SetUp]
        public void SetUp()
        {
            _sut = MockServiceProvider.GenerateAiRequestService();
            _scoringRequestModel = MockDataProvider.GetMockedScoringRequest();
        }
    
        [Test]
        public async Task RequestScore_GivenMockedScoringRequest_ReturnsScoringResponse()
        {
            //Arrange
            _scoringRequestModel = MockDataProvider.GetMockedScoringRequest();
            
            //Act
            var result = await _sut!.RequestScore(_scoringRequestModel);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ScoringResponseModel));
        }

        [Test]
        public async Task RequestScore_GivenNull_ReturnsScoringResponse()
        {
            //Arrange
            _scoringRequestModel = MockDataProvider.GetMockedScoringRequest();
            ScoringRequestModel? scoringRequestModel = null;

            //Act
            var result = await _sut!.RequestScore(scoringRequestModel);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task? RequestScore_GivenScoringRequestModel_ReturnsNull()
        {
            //Arrange
            var sut = MockServiceProvider.GenerateAiRequestServiceWithInvalidResponseContent();
   
            //Act
             var result = await sut.RequestScore(_scoringRequestModel);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task RequestScore_GivenIncorrectHttpConfiguration_ReturnsNull()
        {
            //Arrange
            var sut = MockServiceProvider.GenerateAiRequestServiceWithInvalidConfigurationAndReturnValues();

            //Act
            ScoringResponseModel? result = await sut.RequestScore(_scoringRequestModel);

            //Assert
            result.Should().BeNull();
        }
    }
}
