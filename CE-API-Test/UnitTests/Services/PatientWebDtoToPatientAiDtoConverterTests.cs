using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    internal class PatientWebDtoToPatientAiDtoConverterTests
    {
        private ScoringRequestModel _scoringRequestModel;

        [SetUp]
        public void Setup()
        {
            _scoringRequestModel = MockDataProvider.GetMockedScoringRequest();
        }

        [Test]
        public void ConvertToAiDto_GivenMockedDto_ReturnConvertedObject()
        {
            //Arrange

            //Act
            var convertedObject = DtoConverter.ConvertToAiDto(_scoringRequestModel.LatestBiomarkers);

            //Assert
            convertedObject.Should().NotBeNull();
            convertedObject.Should().BeOfType(typeof(AiDto));
            AssertPropertiesNotNull(convertedObject);
        }

        [Test]
        public void ConvertToAiDto_GivenNull_ReturnConvertedObject()
        {
            //Arrange
            Biomarkers? invalidBiomarkers = null;

            //Act
            var convertToAiDtoTask= () => DtoConverter.ConvertToAiDto(invalidBiomarkers);

            //Assert
            convertToAiDtoTask.Should().NotBeNull();
            convertToAiDtoTask.Should().Throw<Exception>();
        }

        private void AssertPropertiesNotNull(AiDto aiDto)
        {
            var properties = aiDto.GetType().GetProperties()
                .Where(x => !x.IsDefined(typeof(NotMappedAttribute), false) &&
                            !x.IsDefined(typeof(JsonIgnoreAttribute), false))
                .ToList();

            foreach (var item in properties)
            {
                //Todo these properties are currently unused
                if (item.Name.Equals("promocode") ||
                    item.Name.Equals("units") ||
                    item.Name.Equals("CustomToken"))
                {
                    continue;
                }

                item.GetValue(aiDto).Should().NotBeNull();
            }
        }
    }
}
