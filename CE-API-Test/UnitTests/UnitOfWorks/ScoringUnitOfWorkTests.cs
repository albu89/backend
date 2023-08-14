using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CE_API_Test.UnitTests.UnitOfWorks;

public class ScoringUnitOfWorkTests
{
    private IScoringUOW _scoringUow;
    private CEContext _ceContext;
    private IMapper _mapper;
    private IAiRequestService _requestService;
    private IScoreSummaryUtility _scoreSummaryUtility;
    private IValueConversionUOW _valueConversionUow;
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

        _scoreSummaryUtility = new ScoreSummaryUtility(_mapper);
        _requestService = MockServiceProvider.GenerateAiRequestService();
        _ceContext = new CEContext(options);
        var valueConversionUow = new Mock<IValueConversionUOW>();
        valueConversionUow
                .Setup(x => x.ConvertToScoringRequest(It.IsAny<ScoringRequest>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(() =>
                {
                    var mockedScoringRequest = MockDataProvider.GetMockedScoringRequest();
                    var markers = mockedScoringRequest.LatestBiomarkers;
                    mockedScoringRequest.Biomarkers = new List<Biomarkers>();
                    return (mockedScoringRequest, markers);
                } );

        valueConversionUow.Setup(x => x.ConvertToSiValues(It.IsAny<ScoringRequest>())).Returns(() => Task.FromResult(MockDataProvider.GetMockedScoringRequestDto()));

        _valueConversionUow = valueConversionUow.Object;
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CEContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _ceContext = new CEContext(options);
        _scoringUow = new ScoringUOW(_ceContext, _requestService, _mapper, _valueConversionUow, _scoreSummaryUtility);
    }

    [Test]
    public void TestStoringScoringResponse()
    {
        // Act
        var request = MockDataProvider.GetMockedScoringRequest();
        var response = MockDataProvider.GetMockedScoringResponseForRequest(request);
        _scoringUow.StoreScoringRequest(request, _userId);
        var result = _scoringUow.StoreScoringResponse(response);
        
        // Assert
        result.Should().NotBeNull();
        result.classifier_class.Should().Be(response.classifier_class);
        result.classifier_score.Should().Be(response.classifier_score);
        result.classifier_sign.Should().Be(response.classifier_sign);
        result.RequestId.Should().Be(request.Id);
        result.Request.Should().Be(request);
        
        // Act
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
    public async Task ProcessScoringRequest_ExpectedCorrectRetrievedRequest()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequestDto();

        //Act
        var result = await _scoringUow.ProcessScoringRequest(request, _patientId, _userId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ScoringResponse>();
        result.classifier_score.Should().NotBeNull();
        result.classifier_sign.Should().NotBeNull();
    }
    
    [Test]
    public async Task ProcessScoringRequest_ExpectNewScoreWhenCorrecting()
    {
        //Arrange
        var request = MockDataProvider.GetMockedScoringRequestDto();

        //Act
        var result = await _scoringUow.ProcessScoringRequest(request, _patientId, _userId);
        request.prior_CAD.Value = true;
        var newResult = await _scoringUow.ProcessScoringRequest(request, _userId, _patientId, result.RequestId);
        
        //Assert
        newResult.Should().NotBeNull();
        newResult.Should().BeOfType<ScoringResponse>();
        newResult.classifier_score.Should().NotBeNull();
        newResult.classifier_sign.Should().NotBeNull();
    }

    [TearDown]
    public void OneTimeTearDown()
    {
        _ceContext.Database.EnsureDeleted();
    }
}