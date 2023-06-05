using CE_API_V2.Models;
using CE_API_V2.Services;
using CE_API_V2.DTO;
using CE_API_Test.TestUtility;
using Moq;
using System.Net;
using Microsoft.Extensions.Configuration;
using Moq.Protected;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class AiRequestServiceTests

    {
        private IAiRequestService _sut;
        private ScoringRequestDto _scoringRequestDto;

        [SetUp]
        public void SetUp()
        {
            var inMemSettings = new Dictionary<string, string>
            {
                {"AiSubpath", "/api/AiMock?"}
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemSettings).Build();
            _scoringRequestDto = MockDataProvider.GetMockedScoringRequestDto();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var content = MockDataProvider.GetMockedSerializedResponse();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content),
            };

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("https://testproject");
            _sut = new AiRequestService(httpClient, config);
        }

        [Test]
        public async Task PostPatientData_GivenMockedDto_ReturnOkResult()
        {
            //Arrange

            //Act
            var result = await _sut.RequestScore(_scoringRequestDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ScoringResponse));
        }

        [Test]
        public async Task? PostPatientData_GivenIncorrectHttpConfiguration_ThrowsException()
        {
            //Arrange
            Func<Task> requestFunc = async () => await _sut.RequestScore(_scoringRequestDto);

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
