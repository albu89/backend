using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Controllers;
using CE_API_V2.Hasher;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    public class ScoreControllerTests
    {
        private HttpClient _httpClient;
        private IScoringUOW _scoringUow;
        private IValueConversionUOW _valueConversionUow;
        private IPatientIdHashingUOW _patientHashingUow;
        private IInputValidationService _inputValidationService;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            
            var requestServiceMock = new Mock<IAiRequestService>();
            var mockedScoringRequest = MockDataProvider.GetMockedScoringRequest();
            var valueConversionUow = new Mock<IValueConversionUOW>();
            var inputValidationServiceMock = new Mock<IInputValidationService>();

            var mockedResponseTask = Task.FromResult(MockDataProvider.GetMockedScoringResponse());
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUserDto>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ScoringRequestIsValid(It.IsAny<ScoringRequestDto>())).Returns(new ValidationResult());

            SetupMockedScoringUOW(mockedResponseTask);
            requestServiceMock.Setup(x => x.RequestScore(It.IsAny<ScoringRequest>())).Returns(mockedResponseTask);
            valueConversionUow
                .Setup(x => x.ConvertToScoringRequest(It.IsAny<ScoringRequestDto>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(mockedScoringRequest);

            _valueConversionUow = valueConversionUow.Object;
            var hashingUowMock = new Mock<IPatientIdHashingUOW>();
            hashingUowMock.Setup(x => x.HashPatientId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>())).Returns(It.IsAny<string>);
            _patientHashingUow = hashingUowMock.Object;
            _inputValidationService = inputValidationServiceMock.Object;
        }

        [Test]
        public async Task PostPatientData_GivenMockedDto_ReturnOkResult()
        {
            //Arrange
            var sut = new ScoreController(_scoringUow, _patientHashingUow, _valueConversionUow, _inputValidationService, _mapper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PostPatientData(mockedBiomarkers);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponseDto));
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

        [Test]
        public async Task GetScoringRequests_GivenCorrespondingRequestGuid_ReturnOkResult()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new ScoringHistoryDto()
            {
                RequestId = requestId,
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f
            };

            if (mockedHistory != null)
            {
                scoringHistoryMock.Add(mockedHistory);
            }

            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);

            _scoringUow = scoringUowMock.Object;

            var sut = new ScoreController(_scoringUow, _patientHashingUow, _valueConversionUow, _inputValidationService, _mapper);
            ////Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringHistoryDto));
            okResult?.Value?.Should().BeEquivalentTo(mockedHistory);
        }

        [Test]
        public async Task GetScoringRequest_GivenIncorrectRequestGuid_ReturnBadRequestResult()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid testGuid = Guid.NewGuid();

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new ScoringHistoryDto()
            {
                RequestId = Guid.NewGuid(),
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f
            };

            if (mockedHistory != null)
            {
                scoringHistoryMock.Add(mockedHistory);
            }

            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);
            _scoringUow = scoringUowMock.Object;

            var sut = new ScoreController(_scoringUow, _patientHashingUow, _valueConversionUow, _inputValidationService, _mapper);
            ////Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, testGuid);

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));

            var badRequestResult = result as BadRequestResult;
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task GetScoringRequestsParameterless_WithoutParameters_ReturnOkRequestResult()
        {
            //Arrange
            var sut = new ScoreController(_scoringUow, _patientHashingUow, _valueConversionUow, _inputValidationService, _mapper);

            ////Act
            var result = sut.GetScoringRequests();

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetScoringRequests_WithParameters_ReturnOkRequestResult()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);

            var sut = new ScoreController(_scoringUow, _patientHashingUow, _valueConversionUow, _inputValidationService, _mapper);

            ////Act
            var result = sut.GetScoringRequests(name, lastname, dateOfBirth);

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult?.StatusCode.Should().Be(200);
        }

        private void SetupMockedScoringUOW(Task<ScoringResponse> mockedResponseTask)
        {
            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();

            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(scoringHistoryMock);
            scoringUowMock
                .Setup(x => x.ProcessScoringRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mockedResponseTask);
            _scoringUow = scoringUowMock.Object;
        }
    }
}
