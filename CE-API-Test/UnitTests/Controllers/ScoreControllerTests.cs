using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Controllers;
using CE_API_V2.Hasher;
using CE_API_V2.Localization.JsonStringFactroy;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Validators;
using FluentValidation;
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
        private IScoringTemplateService _scoringTemplateService;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var scoringUow = new Mock<IScoringUOW>();

            _mapper = mapperConfig.CreateMapper();

            var requestServiceMock = new Mock<IAiRequestService>();
            var mockedScoringRequest = MockDataProvider.GetMockedScoringRequest();
            var valueConversionUow = new Mock<IValueConversionUOW>();
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var scoringTemplateService = new Mock<IScoringTemplateService>();
            
            var mockedResponseTask = Task.FromResult(MockDataProvider.GetMockedScoringResponse());
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUserDto>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ScoringRequestIsValid(It.IsAny<ScoringRequestDto>())).Returns(new ValidationResult());

            SetupMockedScoringUOW(mockedResponseTask);
            requestServiceMock.Setup(x => x.RequestScore(It.IsAny<ScoringRequest>())).Returns(mockedResponseTask);
            valueConversionUow
                .Setup(x => x.ConvertToScoringRequest(It.IsAny<ScoringRequestDto>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(mockedScoringRequest);
            scoringTemplateService.Setup(x => x.GetTemplate(It.IsAny<string>())).Returns(Task.FromResult(new ScoreSummary()));

            _valueConversionUow = valueConversionUow.Object;
            var hashingUowMock = new Mock<IPatientIdHashingUOW>();
            hashingUowMock.Setup(x => x.HashPatientId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>())).Returns(It.IsAny<string>);
            _patientHashingUow = hashingUowMock.Object;
            _inputValidationService = new InputValidationService(new CE_API_V2.Validators.ScoringRequestValidator());
            _scoringTemplateService = scoringTemplateService.Object;
        }

        [Test]
        public async Task PostPatientData_GivenMockedDto_ReturnOkResult()
        {
            //Arrange
            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PostPatientData(mockedBiomarkers, "en-GB");

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponseSummary));
        }


        [Test]
        public async Task PostPatientData_GivenNull_ReturnBadRequestResult()
        {
            //Arrange
            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);

            //Act
            var result = await sut.PostPatientData(null, null);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));

            var badRequest = result as BadRequestResult;
            badRequest?.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task PostPatientData_GivenDtoWithInvalidValues_ReturnBadRequestResult()
        {
            //Arrange
            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);
            var mockedBiomarkersNV = MockDataProvider.CreateNotValidScoringRequestDto();

            //Act
            var postTask = async () => await sut.PostPatientData(mockedBiomarkersNV, "de-DE");

            //Assert
            await postTask.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task GetScoringRequest_GivenCorrespondingRequestGuid_ReturnsOkResultWithSocringSummary()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            string userid = "mockedUserId";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();

            var scoringResponse = new ScoringResponse();
            scoringResponse.Request = new ScoringRequest()
            {
                Id = requestId
            };

            var scoringSummary = new ScoringResponseSummary()
            {
                Biomarkers = new Biomarkers(),
                RecommendationLongText = "MockedLongText",
                RecommendationSummary = "MockedRecommendationSummary",
                RequestId = requestId,
                RiskValue = "MockedRiskValue",
                Warnings = new string[] { },
                classifier_class = 0,
                classifier_score = 0.0,
                classifier_sign = 0
            };

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new ScoringHistoryDto()
            {
                RequestId = requestId,
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f
            };

            scoringHistoryMock.Add(mockedHistory);
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(scoringHistoryMock);
            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse);
            scoringUowMock.Setup(x => x.GetScoreSummary(It.IsAny<ScoringResponse>())).Returns(scoringSummary);

            _scoringUow = scoringUowMock.Object;

            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);
            ////Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponseSummary));
            okResult?.Value?.Should().BeEquivalentTo(scoringSummary);
        }

        [Test]
        public async Task GetScoringRequest_GivenIncorrectRequestGuid_ReturnBadRequestResult()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();
            Guid testGuid = Guid.NewGuid();
            var scoringResponse = new ScoringResponse();
            var scoringSummary = new ScoringResponseSummary()
            {
                Biomarkers = new Biomarkers(),
                RecommendationLongText = "MockedLongText",
                RecommendationSummary = "MockedRecommendationSummary",
                RequestId = requestId,
                RiskValue = "MockedRiskValue",
                Warnings = new string[] { },
                classifier_class = 0,
                classifier_score = 0.0,
                classifier_sign = 0
            };

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

            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);
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
            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);

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

            var sut = new ScoreController(_scoringUow, _patientHashingUow,  _inputValidationService, _scoringTemplateService, _mapper);

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
                .Setup(x => x.ProcessScoringRequest(It.IsAny<ScoringRequestDto>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mockedResponseTask);

            scoringUowMock.Setup(x => x.GetScoreSummary(It.IsAny<ScoringResponse>())).Returns(new ScoringResponseSummary());
            _scoringUow = scoringUowMock.Object;
        }
    }
}
