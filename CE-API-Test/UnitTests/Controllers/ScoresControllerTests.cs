using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Controllers;
using CE_API_V2.Hasher;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using Moq;
using NSubstitute;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    public class ScoresControllerTests
    {
        private IScoringUOW _scoringUow;
        private IPatientIdHashingUOW _patientHashingUow;
        private IInputValidationService _inputValidationService;
        private IConfiguration _configuration;
        private IMapper _mapper;
        private IUserUOW _userUow;

        private Guid OldGuid = Guid.NewGuid();
        private Guid NewGuid = Guid.NewGuid();

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
            var requestServiceMock = new Mock<IAiRequestService>();
            var mockedScoringRequest = MockDataProvider.GetMockedScoringRequest();
            var valueConversionUow = new Mock<IValueConversionUOW>();
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var scoringTemplateService = new Mock<IScoringTemplateService>();
            var userUow = new Mock<IUserUOW>();

            userUow.Setup(u => u.GetUser(It.IsAny<string>())).Returns(MockDataProvider.GetMockedUser);
            
            var mockedResponseTask = Task.FromResult(MockDataProvider.GetScoringResponseSummaryMock());
            var mockedAiResponseTask = Task.FromResult(MockDataProvider.GetMockedScoringResponse());
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ScoringRequestIsValid(It.IsAny<ScoringRequest>(), It.IsAny<UserModel>())).Returns(new ValidationResult());

            SetupMockedScoringUOW(mockedResponseTask);
            requestServiceMock.Setup(x => x.RequestScore(It.IsAny<ScoringRequestModel>())).Returns(mockedAiResponseTask);
            valueConversionUow
                .Setup(x => x.ConvertToScoringRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns((mockedScoringRequest, mockedScoringRequest.LatestBiomarkers));
            scoringTemplateService.Setup(x => x.GetTemplate(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new ScoreSchema()));

            var hashingUowMock = new Mock<IPatientIdHashingUOW>();
            hashingUowMock.Setup(x => x.HashPatientId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(It.IsAny<string>);
            _patientHashingUow = hashingUowMock.Object;
            _inputValidationService = new InputValidationService(new CE_API_V2.Validators.ScoringRequestValidator());
            _userUow = userUow.Object;
        }

        [Test]
        public async Task PostPatientData_GivenMockedDto_ReturnOkResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PostPatientData(mockedBiomarkers, "en-GB");

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
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);

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
            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);
            var mockedBiomarkersNV = MockDataProvider.CreateNotValidScoringRequestDto();

            //Act
            var postTask = async () => await sut.PostPatientData(mockedBiomarkersNV, "de-DE");

            //Assert
            await postTask.Should().ThrowAsync<ValidationException>();
        }
        
        [Test]
        public async Task PutPatientData_GivenGuidWithCreatedOnInTimeframe_ReturnOkResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();
        
            //Act
            var result = await sut.PutPatientData(mockedBiomarkers, NewGuid);
        
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
        }
        
        [Test]
        public async Task PutPatientData_GivenGuidWithCreatedOnOutsideOfTimeFrame()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);
            var mockedBiomarkers = MockDataProvider.CreateValidScoringRequestDto();

            //Act
            var result = await sut.PutPatientData(mockedBiomarkers, OldGuid);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));

            var okResult = result as BadRequestObjectResult;
            okResult?.StatusCode.Should().Be(400);
        }
        

        [Test]
        public async Task GetScoringRequest_GivenCorrespondingRequestGuid_ReturnsOkResultWithScoringSummary()
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
                Id = requestId
            };

            var scoringSummary = new ScoringResponse()
            {
                Biomarkers = new Biomarkers(),
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
            scoringUowMock.Setup(x => x.GetScoreSummary(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>())).Returns(scoringSummary);

            _scoringUow = scoringUowMock.Object;

            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);
            ////Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, requestId);

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(ScoringResponse));
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
            var scoringResponseMock = MockDataProvider.GetMockedScoringResponse();
            var scoringRequestMock = MockDataProvider.GetMockedScoringRequest();
            scoringResponseMock.Request = scoringRequestMock;
            scoringResponseMock.RequestId = scoringRequestMock.Id;
            

            var scoringUowMock = new Mock<IScoringUOW>();

            scoringUowMock.Setup(x => x.RetrieveScoringResponse(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(scoringResponseMock);
            _scoringUow = scoringUowMock.Object;

            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);
            
            ////Act
            var result = sut.GetScoringRequest(name, lastname, dateOfBirth, testGuid);

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));

            var badRequestResult = result as BadRequestResult;
            badRequestResult?.StatusCode.Should().Be(400);
        }
        [Test]
        public void GetScoringRequestsParameterless_WithoutParameters_ReturnOkRequestResult()
        {
            //Arrange
            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);

            ////Act
            var result = sut.GetScoringRequests();

            ////Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public void GetScoringRequests_WithParameters_ReturnOkRequestResult()
        {
            //Arrange
            string name = "name";
            string lastname = "lastname";
            DateTime dateOfBirth = new(2000, 1, 1);

            var sut = new ScoresController(_scoringUow, _patientHashingUow,  _inputValidationService, _configuration, _userUow);

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
                .Setup(x => x.ProcessScoringRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid?>()))
                .Returns(mockedResponseTask);

            scoringUowMock.Setup(x => x.GetScoreSummary(It.IsAny<ScoringResponseModel>(), It.IsAny<Biomarkers>())).Returns(new ScoringResponse());

            var oldMock = MockDataProvider.GetMockedScoringRequest(CreatedOn: DateTimeOffset.Now.Subtract(TimeSpan.FromDays(3)));
            var newMock = MockDataProvider.GetMockedScoringRequest(CreatedOn: DateTimeOffset.Now.Subtract(TimeSpan.FromDays(1)));

            scoringUowMock.Setup(x => x.RetrieveScoringRequest(OldGuid, It.IsAny<string>())).Returns(oldMock);
            scoringUowMock.Setup(x => x.RetrieveScoringRequest(NewGuid, It.IsAny<string>())).Returns(newMock);
            _scoringUow = scoringUowMock.Object;
        }
    }
}   
