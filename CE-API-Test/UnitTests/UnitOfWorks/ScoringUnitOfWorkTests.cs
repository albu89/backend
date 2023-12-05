using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CE_API_Test.UnitTests.UnitOfWorks;

public class ScoringUnitOfWorkTests
{
    private IScoringUOW _scoringUow;
    private IMapper _mapper;
    private IAiRequestService _requestService;
    private IScoreSummaryUtility _scoreSummaryUtility;
    private IValueConversionUOW _valueConversionUow;
    private IConfigurationRoot _configuration;
    private CEContext _ceContext;
    private CEContext _invalidContext;
    private string _userId = "TestUserId";
    private string _patientId = "TestPatientId";


    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var options = new DbContextOptionsBuilder<CEContext>()
            .UseInMemoryDatabase(databaseName: "CEDatabase")
            .Options;

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

        _scoreSummaryUtility = new ScoreSummaryUtility(_mapper, _configuration);
        _requestService = MockServiceProvider.GenerateAiRequestService();
        _ceContext = new CEContext(options);
        var valConv = new ValueConversionUOW(_mapper, new BiomarkersTemplateService(_mapper));
        _valueConversionUow = valConv;
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CEContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _ceContext = new CEContext(options);
        _invalidContext = null;
        _scoringUow = new ScoringUOW(_ceContext, _requestService, _mapper, _valueConversionUow, _scoreSummaryUtility);
    }

    [Test]
    public void StoreScoringResponse_GivenValidParameter_ExpectedValidReturnedScoringResponseModel()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequest();
        var response = MockDataProvider.GetMockedScoringResponseForRequest(request);
        _scoringUow.StoreScoringRequest(request, _userId);

        // Act
        var result = _scoringUow.StoreScoringResponse(response);

        // Assert
        result.Should().NotBeNull();
        result.classifier_class.Should().Be(response.classifier_class);
        result.classifier_score.Should().Be(response.classifier_score);
        result.classifier_sign.Should().Be(response.classifier_sign);
        result.RequestId.Should().Be(request.Id);
        result.Request.Should().Be(request);
    }

    [Test]
    public void StoreScoringResponse_GivenValidParameter_ExpectedScoringRequestStoredInDb()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequest();
        var response = MockDataProvider.GetMockedScoringResponseForRequest(request);
        _scoringUow.StoreScoringRequest(request, _userId);

        // Act
        var result = _scoringUow.StoreScoringResponse(response);
        var resFromDb = _scoringUow.RetrieveScoringResponse(request.Id, _userId);

        // Assert
        resFromDb.Should().NotBeNull();
        resFromDb.classifier_class.Should().Be(result.classifier_class);
        resFromDb.classifier_score.Should().Be(result.classifier_score);
        resFromDb.classifier_sign.Should().Be(result.classifier_sign);
        resFromDb.RequestId.Should().Be(request.Id);
        resFromDb.Request.Should().Be(request);
    }

    [Test]
    public void StoreScoringResponse_GivenNull_ExpectedScoringRequestStoredInDb()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequest();
        ScoringResponseModel response = null;
        _scoringUow.StoreScoringRequest(request, _userId);

        // Act
        var result = () => _scoringUow.StoreScoringResponse(response);

        // Assert
        result.Should().Throw<Exception>();
    }

    [Test]
    public void StoreScoringResponse_GivenNotPresentDatabase_ExpectedScoringRequestStoredInDb()
    {
        //Arrange
        var scoringUow = new ScoringUOW(_invalidContext, _requestService, _mapper, _valueConversionUow, _scoreSummaryUtility);

        var request = MockDataProvider.GetMockedScoringRequest();
        ScoringResponseModel response = null;

        // Act
        var result = () => scoringUow.StoreScoringResponse(response);

        // Assert
        result.Should().Throw<Exception>();
    }

    [Test]
    public void RetrieveScoringRequest_ExpectedCorrectRetrievedRequest()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequest(_userId);
        _scoringUow.StoreScoringRequest(request, _userId);
        var request2 = MockDataProvider.GetMockedScoringRequest("differentUserId");
        _scoringUow.StoreScoringRequest(request2, "differentUserId");

        //Act
        var resFromDb = _scoringUow.RetrieveScoringRequest(request.Id, _userId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(2);
        resFromDb.Should().NotBeNull();
        resFromDb.PatientId.Should().Be(request.PatientId);
        resFromDb.UserId.Should().Be(request.UserId);
        resFromDb.LatestBiomarkers.Should().Be(request.LatestBiomarkers);
        resFromDb.Id.Should().Be(request.Id);
        resFromDb.LatestResponse.Should().Be(request.LatestResponse);
    }

    [Test]
    public void RetrieveScoringRequest_GivenInvalidId_ExpectedThrownException()
    {
        //Arrange
        var invalidId = Guid.NewGuid();
        var request = MockDataProvider.GetMockedScoringRequest(_userId);
        _scoringUow.StoreScoringRequest(request, _userId);
        var request2 = MockDataProvider.GetMockedScoringRequest("differentUserId");
        _scoringUow.StoreScoringRequest(request2, "differentUserId");

        //Act
        var task = () => _scoringUow.RetrieveScoringRequest(invalidId, _userId);

        //Assert
        task.Should().NotBeNull();
        task.Should().Throw<Exception>();
    }

    [Test]
    public void RetrieveScoringRequest_GivenNotPresentUserIdInDb_ExpectedThrownException()
    {
        //Arrange
        var invalidId = Guid.NewGuid().ToString();
        var request = MockDataProvider.GetMockedScoringRequest(_userId);
        _scoringUow.StoreScoringRequest(request, _userId);
        var request2 = MockDataProvider.GetMockedScoringRequest("differentUserId");
        _scoringUow.StoreScoringRequest(request2, "differentUserId");

        //Act
        var task = () => _scoringUow.RetrieveScoringRequest(request.Id, invalidId);

        //Assert
        task.Should().NotBeNull();
        task.Should().Throw<Exception>();
    }

    [Test]
    public void RetrieveScoringRequest_GivenDbIsEmpty_ExpectedThrownException()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequest(_userId);

        //Act
        var retrieveScoringRequestTask = () => _scoringUow.RetrieveScoringRequest(request.Id, _userId);

        //Assert
        retrieveScoringRequestTask.Should().Throw<Exception>();
    }

    [Test]
    public void RetrieveScoringHistoryForUser_ExpectedCorrectRetrievedRequest()
    {
        //Arrange
        var testedUserId = "testuserid";

        var request = MockDataProvider.GetMockedScoringRequest(testedUserId);
        _scoringUow.StoreScoringRequest(request, testedUserId);
        var request2 = MockDataProvider.GetMockedScoringRequest(testedUserId);
        _scoringUow.StoreScoringRequest(request2, testedUserId);
        var request3 = MockDataProvider.GetMockedScoringRequest("differentUserId");
        _scoringUow.StoreScoringRequest(request3, "differentUserId");
        var request4 = MockDataProvider.GetMockedScoringRequest("differentUserId2");
        _scoringUow.StoreScoringRequest(request4, "differentUserId2");

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForUser(testedUserId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Should().NotBeNull();
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(4);

        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid.Count().Should().Be(2);

        var firstResFromDbByGuid = resourcesFromDb.FirstOrDefault(x => x.Id.Equals(request.Id));

        firstResFromDbByGuid.Should().NotBeNull();
        firstResFromDbByGuid.PatientId.Should().Be(request.PatientId);
        firstResFromDbByGuid.UserId.Should().Be(request.UserId);
        firstResFromDbByGuid.LatestBiomarkers.Should().Be(request.LatestBiomarkers);
        firstResFromDbByGuid.Id.Should().Be(request.Id);
        firstResFromDbByGuid.LatestResponse.Should().Be(request.LatestResponse);
    }

    [Test]
    public void RetrieveScoringHistoryForUser_GivenInvalidUserId_ExpectedEmptyReturnedList()
    {
        //Arrange
        var testedUserId = "userId";
        var invalidUserId = "invalidId";

        var request = MockDataProvider.GetMockedScoringRequest(testedUserId);
        _scoringUow.StoreScoringRequest(request, testedUserId);
        var request2 = MockDataProvider.GetMockedScoringRequest(testedUserId);
        _scoringUow.StoreScoringRequest(request2, testedUserId);
        var request3 = MockDataProvider.GetMockedScoringRequest("differentUserId");
        _scoringUow.StoreScoringRequest(request3, "differentUserId");
        var request4 = MockDataProvider.GetMockedScoringRequest("differentUserId2");
        _scoringUow.StoreScoringRequest(request4, "differentUserId2");

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForUser(invalidUserId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Should().NotBeNull();
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(4);

        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid.Count().Should().Be(0);
    }

    [Test]
    public void RetrieveScoringHistoryForUser_GivenEmptyDB_ExpectedEmptyReturnedList()
    {
        //Arrange
        var testedUserId = "testuserid";

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForUser(testedUserId);

        //Assert
        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid!.Should().BeOfType(typeof(List<SimpleScore>));
        ((List<SimpleScore>)resourcesFromDbByGuid).Count.Should().Be(0);
    }

    [Test]
    public void RetrieveScoringHistoryForPatient_ExpectedCorrectRetrievedRequest()
    {
        //Arrange
        var testPatientId = "testpatientid";

        var request = MockDataProvider.GetMockedScoringRequest(_userId, testPatientId);
        request.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request, _userId);
        var request2 = MockDataProvider.GetMockedScoringRequest(_userId, "differentPatientId");
        request2.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request2, "differentPatientId");
        var request3 = MockDataProvider.GetMockedScoringRequest("differentUserId", testPatientId);
        request3.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request3, "differentUserId");
        var request4 = MockDataProvider.GetMockedScoringRequest(_userId, testPatientId);
        request4.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request4, _userId);

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForPatient(testPatientId, _userId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Should().NotBeNull();
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(4);

        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid.Count().Should().Be(2);

        var firstResFromDbByGuid = resourcesFromDb.FirstOrDefault(x => x.Id.Equals(request.Id));

        firstResFromDbByGuid.Should().NotBeNull();
        firstResFromDbByGuid.PatientId.Should().Be(request.PatientId);
        firstResFromDbByGuid.UserId.Should().Be(request.UserId);
        firstResFromDbByGuid.LatestBiomarkers.Should().Be(request.LatestBiomarkers);
        firstResFromDbByGuid.Id.Should().Be(request.Id);
        firstResFromDbByGuid.LatestResponse.Should().Be(request.LatestResponse);
    }

    [Test]
    public void RetrieveScoringHistoryForPatient_GivenEmptyDb_ExpectedEmptyReturnedList()
    {
        //Arrange
        var testPatientId = "testpatientid";

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForPatient(testPatientId, _userId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Should().NotBeNull();
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(0);

        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid.Count().Should().Be(0);
    }

    [Test]
    public void RetrieveScoringHistoryForPatient_GivenInvalidPatientId_ExpectedEmptyReturnedList()
    {
        //Arrange
        var testPatientId = "testpatientid";
        var invalidPatientId = "invalid";

        var request = MockDataProvider.GetMockedScoringRequest(_userId, testPatientId);
        request.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request, _userId);
        var request2 = MockDataProvider.GetMockedScoringRequest(_userId, "differentPatientId");
        request2.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request2, "differentPatientId");
        var request3 = MockDataProvider.GetMockedScoringRequest("differentUserId", testPatientId);
        request3.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request3, "differentUserId");
        var request4 = MockDataProvider.GetMockedScoringRequest(_userId, testPatientId);
        request4.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request4, _userId);

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForPatient(invalidPatientId, _userId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Should().NotBeNull();
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(4);

        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid.Count().Should().Be(0);
    }

    [Test]
    public void RetrieveScoringHistoryForPatient_GivenInvalidUserId_ExpectedEmptyReturnedList()
    {
        //Arrange
        var testPatientId = "testpatientid";
        var invalidUserId = "invalid";

        var request = MockDataProvider.GetMockedScoringRequest(_userId, testPatientId);
        request.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request, _userId);
        var request2 = MockDataProvider.GetMockedScoringRequest(_userId, "differentPatientId");
        request2.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request2, "differentPatientId");
        var request3 = MockDataProvider.GetMockedScoringRequest("differentUserId", testPatientId);
        request3.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request3, "differentUserId");
        var request4 = MockDataProvider.GetMockedScoringRequest(_userId, testPatientId);
        request4.Biomarkers = null;
        _scoringUow.StoreScoringRequest(request4, _userId);

        //Act
        var resourcesFromDbByGuid = _scoringUow.RetrieveScoringHistoryForPatient(testPatientId, invalidUserId);
        var resourcesFromDb = _scoringUow.ScoringRequestRepository.Get();

        //Assert
        resourcesFromDb.Should().NotBeNull();
        resourcesFromDb.Count().Should().BeGreaterOrEqualTo(4);

        resourcesFromDbByGuid.Should().NotBeNull();
        resourcesFromDbByGuid.Count().Should().Be(0);
    }

    [Test]
    public async Task ProcessScoringRequest_ExpectedCorrectRetrievedRequest()
    {
        //Arrange
        var prevalence = PatientDataEnums.ClinicalSetting.PrimaryCare;
        var request = MockDataProvider.GetMockedScoringRequestDto();
        
        //Act
        var result = await _scoringUow.ProcessScoringRequest(request, _patientId, _userId, prevalence);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ScoringResponse>();
        result.classifier_score.Should().NotBeNull();
    }

    [Test]
    public async Task ProcessScoringRequest_ExpectNewScoreWhenCorrecting()
    {
        //Arrange
        var prevalence = PatientDataEnums.ClinicalSetting.PrimaryCare;
        var request = MockDataProvider.GetMockedScoringRequestDto();

        //Act
        var result = await _scoringUow.ProcessScoringRequest(request, _userId, _patientId, prevalence);
        request.prior_CAD.Value = true;
        var newResult = await _scoringUow.ProcessScoringRequest(request, _userId, _patientId, prevalence, result.RequestId);

        //Assert
        newResult.Should().NotBeNull();
        newResult.Should().BeOfType<ScoringResponse>();
        newResult.classifier_score.Should().NotBeNull();

        var dbResult = await _ceContext.ScoringRequests.FindAsync(result.RequestId);
        dbResult.Should().NotBeNull();
        dbResult.Biomarkers.Count().Should().Be(2);
        dbResult.Responses.Count().Should().Be(2);
    }

    [Test]
    public async Task ProcessScoringRequest_AiRequestServiceReturnsNull_ExpectedCorrectRetrievedRequest()
    {
        //Arrange
        var prevalence = PatientDataEnums.ClinicalSetting.PrimaryCare;
        var requestService = new Mock<IAiRequestService>();
        Task<ScoringResponseModel>? nullTask = null;
        
        requestService.Setup(x => x.RequestScore(It.IsAny<ScoringRequestModel>())).Returns(nullTask);
        var scoringUow =
            new ScoringUOW(_ceContext, requestService.Object, _mapper, _valueConversionUow, _scoreSummaryUtility);
        var request = MockDataProvider.GetMockedScoringRequestDto();

        //Act
        var processScoringRequestTask = async () => await scoringUow.ProcessScoringRequest(request, _patientId, _userId, prevalence);

        //Assert
        processScoringRequestTask.Should().NotBeNull();
        await processScoringRequestTask.Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task StoreDraftRequest_GivenValidData_StoresDraftInDatabase()
    {
        // Arrange
        var prevalence = PatientDataEnums.ClinicalSetting.PrimaryCare;
        var request = MockDataProvider.CreateScoringRequestDraft();

        // Act
        var result = _scoringUow.StoreDraftRequest(request, _userId, _patientId, prevalence);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        var dbResult = await _ceContext.ScoringRequests.FindAsync(result.RequestId);
        dbResult.Should().NotBeNull();
        dbResult.LatestBiomarkersDraft.Should().NotBeNull();
        var biomarkersDraft = dbResult.LatestBiomarkersDraft;
        biomarkersDraft.Cholesterol.Should().Be(request.Cholesterol.Value);
        biomarkersDraft.Aceinhibitor.Should().Be(request.ACEInhibitor.Value);
        biomarkersDraft.Bilirubin.Should().Be(request.Bilirubin.Value);
        biomarkersDraft.Calciumant.Should().Be(request.CaAntagonist.Value);
        biomarkersDraft.Diastolicbp.Should().Be(request.DiastolicBloodPressure.Value);
        _ceContext.ScoringResponses.Any(rsp => rsp.BiomarkersId == biomarkersDraft.Id).Should().BeFalse("no score should have been requested from the AI model.");
    }

    [Test]
    public async Task StoreDraftRequest_GivenEmptyDB_ReturnsThrownException()
    {
        // Arrange
        var prevalence = PatientDataEnums.ClinicalSetting.PrimaryCare;
        var scoringUow = new ScoringUOW(_invalidContext, _requestService, _mapper, _valueConversionUow,
            _scoreSummaryUtility);
        var request = MockDataProvider.CreateScoringRequestDraft();

        // Act
        var result =  () => scoringUow.StoreDraftRequest(request, _userId, _patientId, prevalence);

        // Assert
        result.Should().NotBeNull();
        result.Should().Throw<Exception>();
    }

    [TearDown]
    public void OneTimeTearDown()
    {
        _ceContext.Database.EnsureDeleted();
    }
}