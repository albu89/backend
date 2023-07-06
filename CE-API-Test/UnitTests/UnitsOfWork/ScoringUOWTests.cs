﻿
using CE_API_V2.Models.DTO;
using CE_API_V2.Models;
using AutoMapper;
using CE_API_V2.Data;
using CE_API_V2.Services.Interfaces;
using Moq;

namespace CE_API_Test.UnitTests.UnitsOfWork
{
    [TestFixture]
    public class ScoringUOWTests
    {
        private IMapper _mapper;
        private IAiRequestService _requestService;
        private CEContext _context;

        [SetUp]
        public void SetUp()
        {
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<ScoringRequest>(It.IsAny<ScoringRequestDto>())).Returns(new ScoringRequest());

            var requestService = new Mock<IAiRequestService>();

            _mapper = mapperMock.Object;
            _requestService = requestService.Object;
            _context = new CEContext();
        }

        [Test]
        public async Task StoreScoringRequest_Given_Return()
        {
            //    //Arrange
            //    var scoringUow = new ScoringUOW(_context, _requestService, _mapper);
            //    var storingRequest = MockDataProvider.GetMockedScoringRequest();
            //    var userId = "mockeduser";

            //    //Act
            //    scoringUow.StoreScoringRequest(storingRequest, userId);
            //    //Assert
            //    Assert.True(true);
        }
    }
}
