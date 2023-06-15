﻿using CE_API_V2.Models;
using CE_API_V2.Services;
using Moq;
using System.Net;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq.Protected;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class AiRequestServiceTests

    {
        private IAiRequestService _sut;
        private ScoringRequestDto _scoringRequestDto;
        private ScoringRequest _scoringRequest;

        [SetUp]
        public void SetUp()
        {
            _sut = MockServiceProvider.GenerateAiRequestService();
        }
       

        [Test]
        public async Task PostPatientData_GivenMockedDto_ReturnOkResult()
        {
            //Arrange
            _scoringRequest = MockDataProvider.GetMockedScoringRequest();
            
            //Act
            var result = await _sut.RequestScore(_scoringRequest);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ScoringResponse));
        }

        [Test]
        public async Task? PostPatientData_GivenIncorrectHttpConfiguration_ThrowsException()
        {
            //Arrange
            Func<Task> requestFunc = async () => await _sut.RequestScore(_scoringRequest);

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
