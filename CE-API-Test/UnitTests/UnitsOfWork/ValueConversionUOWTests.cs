using AutoMapper;
using CE_API_V2.Hasher;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CE_API_Test.UnitTests.UnitsOfWork
{
    [TestFixture]
    public class ValueConversionUOWTests
    {
        private IMapper _mapper;
        private IConfiguration _config;

        [SetUp]
        public void SetUp()
        {
            var testConfig = new Dictionary<string, string>()
            {
                { "Salt", "ABCDEF" }
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfig)
                .Build();
            
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<ScoringRequest>(It.IsAny<ScoringRequestDto>())).Returns(new ScoringRequest());
            
            _mapper = mapperMock.Object;
        }


        [Test]
        public void ConvertToScoringRequest_GivenCorrectParameters_ExpectedObjectWithCorrectUserIdAndPatientId()
        {
            //Arrange
            var patientIdHashingUow = new PatientIdHashingUOW(_config);   
            var scoringRequestDto = new ScoringRequestDto();
            var userId = "anonymous";
            var patientId = patientIdHashingUow.HashPatientId("mock", "mock", DateTime.Now);
            

            var sut = new ValueConversionUOW(_mapper);

            //Act
            var result = sut.ConvertToScoringRequest(scoringRequestDto, userId, patientId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ScoringRequest));
            result.PatientId.Should().Be(patientId);
            result.UserId.Should().Be(userId);
        }
    }
}
