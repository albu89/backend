using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace CE_API_Test.UnitTests.UnitOfWorks;

public class ScoringUnitOfWorkTests
{
    private IScoringUOW _scoringUow;
    private CEContext _ceContext;
    private IMapper _mapper;
    private IAiRequestService _requestService;
    private string _userId = "TestUserId";

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
        
        _requestService = MockServiceProvider.GenerateAiRequestService();
        _ceContext = new CEContext(options);
        _scoringUow = new ScoringUOW(_ceContext, _requestService, _mapper);
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

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _ceContext.Database.EnsureDeleted();
    }
}