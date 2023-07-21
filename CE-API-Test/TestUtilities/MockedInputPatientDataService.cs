using CE_API_V2.Models.DTO;
namespace CE_API_Test.TestUtilities;

public class MockedInputPatientDataService
{
    public static ScoringRequestDto GetPatientBiomarkers(string patientId) => GetMockedPatientById(patientId);

    private static ScoringRequestDto GetMockedPatientById(string patientId)
    {
        var patientData = CreateMockedPatientData();

        return patientData.First();
    }

    private static List<ScoringRequestDto> CreateMockedPatientData()
    {
        var patientDto = new ScoringRequestDto();
        var mockDataProvider = new MockPatientDataProvider(patientDto);

        patientDto = mockDataProvider.CreateMockedData();

        return new List<ScoringRequestDto>() { patientDto };
    }

    internal class MockPatientDataProvider
    {
        private readonly ScoringRequestDto _ScoringRequestDto;
        private readonly Random _random;

        public MockPatientDataProvider(ScoringRequestDto ScoringRequestDto)
        {
            _ScoringRequestDto = ScoringRequestDto;
            _random = new Random();
        }

        public ScoringRequestDto CreateMockedData()
        {
            FillDtoPropertiesWithRandomValues();

            return _ScoringRequestDto;
        }

        private void FillDtoPropertiesWithRandomValues()
        {
            foreach (var property in _ScoringRequestDto.GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(BiomarkerValueDto<>))
                {
                    var wrapperInstance = Activator.CreateInstance(property.PropertyType);
                    var valueProperty = property.PropertyType.GetProperty("Value");
                    if (valueProperty is not null)
                    {
                        valueProperty.SetValue(wrapperInstance, GetRandomValue(valueProperty.PropertyType));
                        property.SetValue(_ScoringRequestDto, wrapperInstance);
                    }
                }
            }
        }

        private object GetRandomValue(Type valuePropPropertyType)
        {
            return valuePropPropertyType.Name switch {
                "Boolean" => GetRandomBoolValue(),
                "Single" => GetNextFloatValue(),
                "String" => "Mock",
                _ => Activator.CreateInstance(valuePropPropertyType)!,
            };
            
        }

        private bool GetRandomBoolValue() => _random.Next(2) > 1;

        private float GetNextFloatValue() => (float)_random.NextDouble();
    }
}