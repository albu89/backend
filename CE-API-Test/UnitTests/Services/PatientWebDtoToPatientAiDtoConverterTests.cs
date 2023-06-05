using System.ComponentModel.DataAnnotations.Schema;
using CE_API_V2.DTO;
using CE_API_V2.Utility;
using System.Text.Json.Serialization;
using CE_API_Test.TestUtility;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    internal class PatientWebDtoToPatientAiDtoConverterTests
    {
        private ScoringRequestDto _scoringRequestDto;

        [SetUp]
        public void Setup()
        {
            _scoringRequestDto = MockDataProvider.GetMockedScoringRequestDto();
        }

        [Test]
        public async Task ConvertToAiDto_GivenMockedDto_ReturnConvertedObject()
        {
            //Arrange

            //Act
            var convertedObject = DtoConverter.ConvertToAiDto(_scoringRequestDto);

            //Assert
            convertedObject.Should().NotBeNull();
            convertedObject.Should().BeOfType(typeof(AiDto));
            AssertPropertiesNotNull(convertedObject);
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
