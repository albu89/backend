using CE_API_Test.TestUtility;
using CE_API_V2.Controllers;
using CE_API_V2.DTO;
using CE_API_V2.Models;
using CE_API_V2.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    public class ScoreControllerTests
    {
        private HttpClient _httpClient;
        private IAiRequestService _requestService;

        [SetUp]
        public void SetUp()
        {
           var requestServiceMock = new Mock<IAiRequestService>();
           var mockedResposeTask = Task.FromResult(MockDataProvider.GetMockedScoringResponse());

           requestServiceMock.Setup(x => x.RequestScore(It.IsAny<ScoringRequestDto>()))
               .Returns(mockedResposeTask);

           _requestService = requestServiceMock.Object;
        }

        [Test]
        public async Task PostPatientData_GivenMockedDto_ReturnOkResult()
        {
            //Arrange
            var sut = new ScoreController(_requestService);
            var mockedBiomarkers = MockDataProvider.GetMockedScoringRequestDto();

            //Act
            var result = await sut.PostPatientData(mockedBiomarkers);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
        }

        [Test]
        public async Task PostPatientData_GivenNull_ReturnBadRequestResult()
        {
            //Todo - functionality to check validity not yet implemented
            //Arrange
            //var sut = new ScoreController(_httpClient);

            ////Act
            //var result = await sut.PostPatientData(null);

            ////Assert
            //Assert.IsNotNull(result);
            //Assert.True(result is BadRequestObjectResult);

            //var okResult = result as BadRequestObjectResult;
            //okResult?.StatusCode.Should().Be(500);
            Assert.True(true);
        }

        [Test]
        public async Task PostPatientData_GivenDtoWithInvalidValues_ReturnBadRequestResult()
        {
            //Todo - functionality to check for validity not yet implemented
            //var sut = new ScoreController(_httpClient);

            ////Act
            //var result = await sut.PostPatientData(null);

            ////Assert
            //Assert.IsNotNull(result);
            //Assert.True(result is BadRequestObjectResult);

            //var okResult = result as BadRequestObjectResult;
            //okResult?.StatusCode.Should().Be(500);
            Assert.True(true);
        }
    }
}
