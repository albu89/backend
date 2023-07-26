using CE_API_V2.Models;
using CE_API_Test.TestUtilities;
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
        public async Task? PostPatientData_GivenIncorrectHttpConfiguration_ThrowsException()
        {
            //Arrange
            Func<Task> requestFunc = async () => await _sut.RequestScore(_scoringRequestModel);

            //Act

            //Assert
            requestFunc.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task PostPatientData_GivenNull_ReturnNull()
        {
            //Arrange

            //Act
            var result = await _sut.RequestScore(null);

            //Assert
            //Todo -> was liefert die Ai wenn das gesendete Objekt fehlerhaft ist ?
            result.Should().BeNull();
        }
    }
}
