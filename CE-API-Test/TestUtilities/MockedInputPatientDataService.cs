using CE_API_V2.Models.DTO;
namespace CE_API_Test.TestUtilities;

public class MockedInputPatientDataService
{
    public ScoringRequestDto? GetPatientBiomarkers(string patientId) => GetMockedPatientById(patientId);

    private ScoringRequestDto GetMockedPatientById(string patientId)
    {
        var patientData = CreateMockedPatientData(patientId);

        return patientData.First();
    }

    private List<ScoringRequestDto> CreateMockedPatientData(string patientId)
    {
        var patientDto = new ScoringRequestDto();
        var mockDataProvider = new MockPatientDataProvider(patientDto);

        patientDto = mockDataProvider.CreateMockedData(patientId);

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

        public ScoringRequestDto CreateMockedData(string patientId)
        {
            FillDtoPropertiesWithRandomValues(patientId);

            return _ScoringRequestDto;
        }

        private void FillDtoPropertiesWithRandomValues(string patientId)
        {
            foreach (var property in _ScoringRequestDto.GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(BiomarkerValueDto<>))
                {
                    var wrapperInstance = Activator.CreateInstance(property.PropertyType);
                    var valueProperty = property.PropertyType.GetProperty("Value");

                    valueProperty.SetValue(wrapperInstance, GetRandomValue(valueProperty.PropertyType));
                    property.SetValue(_ScoringRequestDto, wrapperInstance);
                }
            }
        }

        private object? GetRandomValue(Type valuePropPropertyType)
        {
            switch (valuePropPropertyType.Name)
            {
                case "Boolean":
                    return GetRandomBoolValue();
                case "Single":
                    return GetNextFloatValue();
                case "String":
                    return "Mock";
                default:
                    return null;
            }
        }

        private bool GetRandomBoolValue() => _random.Next(2) > 1;

        private float GetNextFloatValue() => (float)_random.NextDouble();
    }
}