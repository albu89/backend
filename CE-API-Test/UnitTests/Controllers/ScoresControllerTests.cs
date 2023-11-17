using System.Net;
using System.Security.Claims;
using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_Test.TestUtilities.Test;
using CE_API_V2.Controllers;
using CE_API_V2.Data;
using CE_API_V2.Hasher;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    public class ScoresControllerTests
    {
        private IScoringUOW _scoringUow;
        private IPatientIdHashingUOW _patientHashingUow;
        private IInputValidationService _inputValidationService;
        private Mock<IUserInformationExtractor> _userInfoExtractorMock;
        private IConfiguration _configuration;
        private UserHelper _userHelper;
        private IMapper _mapper;
        private IUserUOW _userUow;
        private IScoreSummaryUtility _scoreSummaryUtility;
        private readonly ILogger<ScoresController> _logger = NullLogger<ScoresController>.Instance;

        private readonly Guid OldGuid = Guid.NewGuid();
        private readonly Guid NewGuid = Guid.NewGuid();
        private IUserInformationExtractor _userInformationExtractor;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();

            var testConfig = new Dictionary<string, string?>()
            {
                { "EditPeriodInDays", "1" },
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfig)
                .Build();

            _userHelper = new UserHelper(_mapper, _configuration);

            _scoreSummaryUtility = new ScoreSummaryUtility(_mapper, _configuration);
            var requestServiceMock = new Mock<IAiRequestService>();
            var mockedScoringRequest = MockDataProvider.GetMockedScoringRequest();
            var valueConversionUow = new Mock<IValueConversionUOW>();
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var scoringTemplateService = new Mock<IScoringTemplateService>();
            var userUow = new Mock<IUserUOW>();
            var hashingUowMock = new Mock<IPatientIdHashingUOW>();

            var mockedResponseTask = Task.FromResult(MockDataProvider.GetScoringResponseSummaryMock());
            var mockedRequestTask = Task.FromResult(MockDataProvider.GetMockedScoringRequest());
            var mockedAiResponseTask = Task.FromResult(MockDataProvider.GetMockedScoringResponse());

            SetupMockedScoringUOW(mockedResponseTask, mockedRequestTask);
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ScoringRequestIsValid(It.IsAny<ScoringRequest>(), It.IsAny<UserModel>())).Returns(new ValidationResult());
            requestServiceMock.Setup(x => x.RequestScore(It.IsAny<ScoringRequestModel>())).Returns(mockedAiResponseTask);
            valueConversionUow.Setup(x => x.ConvertToScoringRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PatientDataEnums.ClinicalSetting>())).Returns((mockedScoringRequest, mockedScoringRequest.LatestBiomarkers));
            scoringTemplateService.Setup(x => x.GetTemplate(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new ScoreSchema()));
            userUow.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(MockDataProvider.GetMockedUserModel);
            userUow.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(MockDataProvider.GetMockedUserModel);
            hashingUowMock.Setup(x => x.HashPatientId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(It.IsAny<string>);
            
            _patientHashingUow = hashingUowMock.Object;
            _inputValidationService = new InputValidationService(new CE_API_V2.Validators.ScoringRequestValidator());
            _userUow = userUow.Object;
            _userInfoExtractorMock = new Mock<IUserInformationExtractor>();

            var userIdRecord = new UserIdsRecord()
            {
                TenantId = "MockedTentantId",
                UserId = "MockedUserId",
            };

            _userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userIdRecord);
            _userInformationExtractor = new UserInformationExtractor();
        }

        [Test]
        public async Task PostScoringRequest_GivenValidData_ReturnOkObjectResult()
        {
            //Arrange
            var locale = "en-GB";
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PostScoringRequest(mockedBiomarkers, locale);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
        }

        [Test]
        public async Task PostScoringRequest_GivenEmptyData_ReturnBadRequestResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = await sut.PostScoringRequest(null, null);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));

            var badRequest = result as BadRequestResult;
            badRequest?.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task PostScoringRequest_UserUowReturnsNoUser_ReturnBadRequestResult()
        {
            //Arrange
            UserModel? returnedUser = null;
            var localUserUow = new Mock<IUserUOW>();
            localUserUow.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(returnedUser);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();
            
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, localUserUow.Object, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = async () => await sut.PostScoringRequest(mockedBiomarkers, null);

            //Assert
            result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task PostScoringRequest_GivenInvalidValues_ReturnBadRequestResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkersNV = MockDataProvider.CreateInvalidScoringRequestDto();

            //Act
            var postTask = async () => await sut.PostScoringRequest(mockedBiomarkersNV, "de-DE");

            //Assert
            await postTask.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task PutScoringRequest_GivenScoringRequestInTimeframe_ReturnOkResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PutScoringRequest(mockedBiomarkers, NewGuid);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
        }

        [Test]
        public async Task PutScoringRequest_GivenScoringRequestOutsideOfTimeframe_ReturnBadRequestResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PutScoringRequest(mockedBiomarkers, OldGuid);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));

            var okResult = result as BadRequestObjectResult;
            okResult?.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task PutScoringRequest_ScoringUowThrowsException_ReturnBadRequestResult()
        {
            //Arrange
            var scoringUowMock =  new Mock<IScoringUOW>();
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(It.IsAny<Guid>(), It.IsAny<string>())).Throws<Exception>();

            var sut = new ScoresController(scoringUowMock.Object, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PutScoringRequest(mockedBiomarkers, OldGuid);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));

            var okResult = result as BadRequestObjectResult;
            okResult?.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task GetScoringRequest_GivenMatchingPatientDataAndRequestId_ReturnsOkResultWithScoringResponse()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            string userid = "mockedUserId";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();

            var scoringResponse = new ScoringResponseModel();
            scoringResponse.Request = new ScoringRequestModel()
            {
                Id = requestId,
                Responses = new List<ScoringResponseModel>() { scoringResponse },
                Biomarkers = new List<Biomarkers>() { new Biomarkers() }
            };

            var scoringSummary = new ScoringResponse()
            {
                Biomarkers = new StoredBiomarkers(),
                RecommendationLongText = "MockedLongText",
                RecommendationSummary = "MockedRecommendationSummary",
                RequestId = requestId,
                RiskValue = "MockedRiskValue",
                Warnings = new string[] { },
                classifier_score = 0.0,
            };

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new SimpleScore()
            {
                RequestId = requestId,
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f,
                Risk = ">75%",
                RiskClass = 4
            };

            scoringHistoryMock.Add(mockedHistory);
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);
            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse.Request);
            scoringUowMock.Setup(x => x.GetScoringResponse(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>(), It.IsAny<Guid>())).Returns(scoringSummary);

            var scoringUow = scoringUowMock.Object;

            var sut = new ScoresController(scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
            okResult?.Value?.Should().BeEquivalentTo(scoringSummary);
        }

        [Test]
        public async Task GetScoringRequest_LatestBiomarkerContainScoringResponse_ReturnsOkResultWithScoringResponse()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            string userid = "mockedUserId";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();

            var latestBiomarkers = new Biomarkers();
            latestBiomarkers.Response = new ScoringResponseModel();

            var scoringResponse = new ScoringResponseModel();
            scoringResponse.Request = new ScoringRequestModel()
            {
                Id = requestId,
                Responses = new List<ScoringResponseModel>() { scoringResponse },
                Biomarkers = new List<Biomarkers>() { latestBiomarkers }
            };

            var scoringSummary = new ScoringResponse()
            {
                Biomarkers = new StoredBiomarkers(),
                RecommendationLongText = "MockedLongText",
                RecommendationSummary = "MockedRecommendationSummary",
                RequestId = requestId,
                RiskValue = "MockedRiskValue",
                Warnings = new string[] { },
                classifier_score = 0.0,
            };

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new SimpleScore()
            {
                RequestId = requestId,
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f,
                Risk = ">75%",
                RiskClass = 4
            };

            scoringHistoryMock.Add(mockedHistory);
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);
            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse.Request);
            scoringUowMock.Setup(x => x.GetScoringResponse(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>(), It.IsAny<Guid>())).Returns(scoringSummary);
            
            var scoringUow = scoringUowMock.Object;

            var scoreSummaryUtilityMock = new Mock<IScoreSummaryUtility>();
            scoreSummaryUtilityMock.Setup(x => x.CalculateIfUpdatePossible(It.IsAny<ScoringRequestModel>()))
                .Returns(false);

            var sut = new ScoresController(scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, scoreSummaryUtilityMock.Object, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
            okResult?.Value?.Should().BeEquivalentTo(scoringSummary);
            ((ScoringResponse)okResult?.Value).CanEdit.Should().BeFalse();
        }

        [Test]
        public async Task GetScoringRequest_LatestBiomarkerDoesNotContainScoringResponse_ReturnsOkResultWithScoringResponse()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            string userid = "mockedUserId";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();

            var latestBiomarkers = new Biomarkers();
            latestBiomarkers.Response = null;

            var scoringResponse = new ScoringResponseModel();
            scoringResponse.Request = new ScoringRequestModel()
            {
                Id = requestId,
                Responses = new List<ScoringResponseModel>() { scoringResponse },
                Biomarkers = new List<Biomarkers>() { latestBiomarkers }
            };

            var scoringSummary = new ScoringResponse()
            {
                Biomarkers = new StoredBiomarkers(),
                RecommendationLongText = "MockedLongText",
                RecommendationSummary = "MockedRecommendationSummary",
                RequestId = requestId,
                RiskValue = "MockedRiskValue",
                Warnings = new string[] { },
                classifier_score = 0.0,
            };

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new SimpleScore()
            {
                RequestId = requestId,
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f,
                Risk = ">75%",
                RiskClass = 4
            };

            scoringHistoryMock.Add(mockedHistory);
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);
            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse.Request);
            scoringUowMock.Setup(x => x.GetScoringResponse(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>(), It.IsAny<Guid>())).Returns(scoringSummary);

            var scoringUow = scoringUowMock.Object;

            var scoreSummaryUtilityMock = new Mock<IScoreSummaryUtility>();
            scoreSummaryUtilityMock.Setup(x => x.CalculateIfUpdatePossible(It.IsAny<ScoringRequestModel>()))
                .Returns(false);

            var sut = new ScoresController(scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, scoreSummaryUtilityMock.Object, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
            okResult?.Value?.Should().BeEquivalentTo(scoringSummary);
            ((ScoringResponse)okResult?.Value).CanEdit.Should().BeFalse();
        }

        [Test]
        public async Task GetScoringRequest_ScoreSummaryUtilitySetsCanEditToTrue_ReturnsOkResultWithScoringResponse()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            string userid = "mockedUserId";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid requestId = Guid.NewGuid();

            var latestBiomarkers = new Biomarkers();
            latestBiomarkers.Response = new ScoringResponseModel();

            var scoringResponse = new ScoringResponseModel();
            scoringResponse.Request = new ScoringRequestModel()
            {
                Id = requestId,
                Responses = new List<ScoringResponseModel>() { scoringResponse },
                Biomarkers = new List<Biomarkers>() { latestBiomarkers }
            };

            var scoringSummary = new ScoringResponse()
            {
                Biomarkers = new StoredBiomarkers(),
                RecommendationLongText = "MockedLongText",
                RecommendationSummary = "MockedRecommendationSummary",
                RequestId = requestId,
                RiskValue = "MockedRiskValue",
                Warnings = new string[] { },
                classifier_score = 0.0,
            };

            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();
            var mockedHistory = new SimpleScore()
            {
                RequestId = requestId,
                RequestTimeStamp = DateTimeOffset.Now,
                Score = 1.0f,
                Risk = ">75%",
                RiskClass = 4
            };

            scoringHistoryMock.Add(mockedHistory);
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);
            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringResponse.Request);
            scoringUowMock.Setup(x => x.GetScoringResponse(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>(), It.IsAny<Guid>())).Returns(scoringSummary);

            _scoringUow = scoringUowMock.Object;

            var scoreSummaryUtilityMock = new Mock<IScoreSummaryUtility>();
            scoreSummaryUtilityMock.Setup(x => x.CalculateIfUpdatePossible(It.IsAny<ScoringRequestModel>()))
                .Returns(true);

            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, scoreSummaryUtilityMock.Object, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType(typeof(ScoringResponse));
            okResult.Value.Should().BeEquivalentTo(scoringSummary);
            ((ScoringResponse)okResult.Value!).CanEdit.Should().BeTrue();
        }

        [Test]
        public async Task GetScoringRequest_GivenIncorrectRequestId_ReturnBadRequestResult()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            Guid testGuid = Guid.NewGuid();
            var scoringResponseMock = MockDataProvider.GetMockedScoringResponse();
            var scoringRequestMock = MockDataProvider.GetMockedScoringRequest();

            scoringResponseMock.Request = scoringRequestMock;
            scoringResponseMock.RequestId = scoringRequestMock.Id;

            var scoringUowMock = new Mock<IScoringUOW>();
            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(scoringResponseMock);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(It.IsAny<Guid>(), It.IsAny<string>())).Returns(scoringRequestMock);
            _scoringUow = scoringUowMock.Object;

            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            //Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, testGuid);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));

            var badRequestResult = result as BadRequestResult;
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Test]
        public void GetScoringRequests_ScoringUowReturnsEmptyScoringHistoryForUserList_ReturnOkRequestResult()
        {
            // Arrange
            var scoringUowMock = new Mock<IScoringUOW>();
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForUser(It.IsAny<string>()))
                .Returns(new List<SimpleScore>());
            var scoringUow = scoringUowMock.Object;

            var sut = new ScoresController(scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            // Act
            var result = sut.GetScoringRequests();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult?.StatusCode.Should().Be(200);
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType<List<SimpleScore>>();
        }

        [Test]
        public void GetScoringRequests_ScoringUowReturnsEmptyScoringHistoryForPatientList_ReturnOkRequestResult()
        {
            // Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new(2000, 1, 1);

            var scoringUowMock = new Mock<IScoringUOW>();
            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForUser(It.IsAny<string>()))
                .Returns(new List<SimpleScore>());
            var scoringUow = scoringUowMock.Object;

            var sut = new ScoresController(scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            // Act
            var result = sut.GetScoringRequests(name, lastname, dateOfBirth);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult!.StatusCode.Should().Be(200);
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType<SimpleScore[]>();
        }

        [Test]
        public void GetScoringRequests_WithPatientData_ReturnOkRequestResult()
        {
            // Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new(2000, 1, 1);

            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);

            // Act
            var result = sut.GetScoringRequests(name, lastname, dateOfBirth);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task PostScoringDraft_WithValidInputs_ReturnsOkObjectResult()
        {
            // Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();
            
            // Act
            var result = await sut.PostScoringDraft(mockedBiomarkers);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            (result as OkObjectResult).Value.Should().BeOfType<Guid>();
        }

        [Test]
        public async Task PostScoringDraft_WithInvalidBiomarkers_ReturnsOkObjectResult()
        {
            // Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateInvalidScoringRequestDto();
            
            // Act
            var result = await sut.PostScoringDraft(mockedBiomarkers);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            (result as OkObjectResult).Value.Should().BeOfType<Guid>();
        }
        
        [Test]
        public async Task PostScoringDraft_WithInvalidPatientFirstName_ReturnsBadRequestResult()
        {
            //Arrange
            
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();
            mockedBiomarkers.FirstName = null;
            
            // Act
            var result = await sut.PostScoringDraft(mockedBiomarkers);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));
        }
        
        [Test]
        public async Task PostScoringDraft_WithInvalidPatientLastName_ReturnsBadRequestResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();
            mockedBiomarkers.LastName = string.Empty;
            
            // Act
            var result = await sut.PostScoringDraft(mockedBiomarkers);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));
        }
        
        [Test]
        public async Task PostScoringDraft_WithInvalidPatientDateOfBirth_ReturnsBadRequestResult()
        {
            //Arrange
            
            var sut = new ScoresController(_scoringUow, _patientHashingUow, _inputValidationService, _configuration, _userUow, _scoreSummaryUtility, _userInformationExtractor, _logger, _userHelper);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();
            mockedBiomarkers.DateOfBirth = null;
            
            // Act
            var result = await sut.PostScoringDraft(mockedBiomarkers);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));
        }

        [Test]
        public async Task ScoringControllerConcurrencyTest()
        {
            var app = new CardioExplorerServer
            {
                Environment = Constants.TestEnvironment
            };
            var client = app.CreateClient();

            var response = new List<HttpResponseMessage>();

            for (int i = 0; i < 15; i++)
            {
                client.DefaultRequestHeaders.Add(TestAuthHandler.UserId, $"{i}");
                response.Add((await client.PostAsync("/api/scores/request", null)));
            }

            for (int i = 0; i < 15; i++)
            {
                var expectedCode = i < 15 ? HttpStatusCode.BadRequest : HttpStatusCode.TooManyRequests;
                response[i].StatusCode.Should().Be(expectedCode);
            }
        }

        private void SetupMockedScoringUOW(Task<ScoringResponse> mockedResponseTask, Task<ScoringRequestModel> mockedRequestTask)
        {
            var scoringUowMock = new Mock<IScoringUOW>();
            var scoringHistoryMock = MockDataProvider.GetMockedScoringRequestHistory().ToList();

            scoringUowMock.Setup(x => x.RetrieveScoringHistoryForPatient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(scoringHistoryMock);
            scoringUowMock
                .Setup(x => x.ProcessScoringRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PatientDataEnums.ClinicalSetting>(), It.IsAny<Guid?>()))
                .Returns(mockedResponseTask);
            
            scoringUowMock
                .Setup(x => x.StoreDraftRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PatientDataEnums.ClinicalSetting>()))
                .Returns(mockedRequestTask);

            scoringUowMock.Setup(x => x.GetScoringResponse(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>(), It.IsAny<Guid>())).Returns(new ScoringResponse());

            var oldMock = MockDataProvider.GetMockedScoringRequest(CreatedOn: DateTimeOffset.Now.Subtract(TimeSpan.FromDays(3)));
            var newMock = MockDataProvider.GetMockedScoringRequest(CreatedOn: DateTimeOffset.Now.Subtract(TimeSpan.FromDays(1)));

            scoringUowMock.Setup(x => x.RetrieveScoringRequest(OldGuid, It.IsAny<string>())).Returns(oldMock);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(NewGuid, It.IsAny<string>())).Returns(newMock);
            _scoringUow = scoringUowMock.Object;
        }
    }
}