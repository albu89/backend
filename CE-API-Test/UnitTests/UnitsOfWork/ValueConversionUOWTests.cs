using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Hasher;
using CE_API_V2.Models;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using Microsoft.Extensions.Configuration;

namespace CE_API_Test.UnitTests.UnitsOfWork
{
    [TestFixture]
    public class ValueConversionUOWTests
    {
        private IMapper _mapper;
        private IConfiguration _config;
        private IBiomarkersTemplateService _templateService;

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
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _templateService = new BiomarkersTemplateService(_mapper);
        }


        [Test]
        public void ConvertToScoringRequest_GivenCorrectParameters_ExpectedObjectWithCorrectUserIdAndPatientId()
        {
            //Arrange
            var patientIdHashingUow = new PatientIdHashingUOW(_config);   
            var scoringRequestDto = MockDataProvider.CreateValidScoringRequestDto();
            var userId = "anonymous";
            var patientId = patientIdHashingUow.HashPatientId("mock", "mock", DateTime.Now);
            

            var sut = new ValueConversionUOW(_mapper, _templateService);

            //Act
            var result = sut.ConvertToScoringRequest(scoringRequestDto, userId, patientId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ScoringRequestModel));
            result.PatientId.Should().Be(patientId);
            result.UserId.Should().Be(userId);
        }

        [Test]
        public async Task ConvertToSIValue()
        {
            //Arrange
            var patientIdHashingUow = new PatientIdHashingUOW(_config);
            var scoringRequestDto = MockDataProvider.CreateValidScoringRequestDto();
            
            // Set to specific value
            scoringRequestDto.Protein.Value = 5.0f;
            scoringRequestDto.Protein.UnitType = "Conventional";
            
            scoringRequestDto.Alat.Value = 58.0f;
            scoringRequestDto.Alat.UnitType = "Conventional";
            
            scoringRequestDto.Weight.Value = 50;
            scoringRequestDto.Weight.UnitType = "Conventional";
            
            scoringRequestDto.UricAcid.Value = 150;
            scoringRequestDto.UricAcid.UnitType = "Conventional";
            
            var userId = "anonymous";
            var patientId = patientIdHashingUow.HashPatientId("mock", "mock", DateTime.Now);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            var sut = new ValueConversionUOW(mapper, _templateService);
         
            //Act
            await sut.ConvertToSiValues(scoringRequestDto);
            var result = sut.ConvertToScoringRequest(scoringRequestDto, userId, patientId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ScoringRequestModel));
            
            // conversion-factor = 10; 10 * 5.0f = 50f
            result.Biomarkers.Protein.Should().Be(50f);
            result.PatientId.Should().Be(patientId);
            result.UserId.Should().Be(userId);
            
            // conversion-factor = 1; 1 * 58f = 58f
            result.Biomarkers.Alat.Should().Be(58f);
            
            // conversion-factor = 1; 1 * 50 = 50
            result.Biomarkers.Weight.Should().Be(50);
            
            // conversion-factor = 58.823; 58.823 * 150f = 8,823.45f
            result.Biomarkers.UricAcid.Should().Be(8_823.45f);
        }
    }
}
