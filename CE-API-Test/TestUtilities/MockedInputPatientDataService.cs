using CE_API_V2.Models.DTO;
namespace CE_API_Test.TestUtilities;

public class MockedInputPatientDataService
{
    public static ScoringRequest? GetPatientBiomarkers(string patientId) => GetMockedPatient();

    private static ScoringRequest GetMockedPatient()
    {
        var patientData = CreateMockedPatientData();

        return patientData.First();
    }

    private static List<ScoringRequest> CreateMockedPatientData()
    {
        var patientDto = new ScoringRequest();
        var mockDataProvider = new MockPatientDataProvider(patientDto);

        patientDto = mockDataProvider.CreateMockedData();

        return new List<ScoringRequest>() { patientDto };
    }

    internal class MockPatientDataProvider
    {
        private readonly ScoringRequest _scoringRequest;
        private readonly Random _random;

        public MockPatientDataProvider(ScoringRequest scoringRequest)
        {
            _scoringRequest = scoringRequest;
            _random = new Random();
        }

        public ScoringRequest CreateMockedData()
        {
            FillDtoPropertiesWithRandomValues();

            return _scoringRequest;
        }

        private void FillDtoPropertiesWithRandomValues()
        {
            foreach (var property in _scoringRequest.GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(BiomarkerValue<>))
                {
                    var wrapperInstance = Activator.CreateInstance(property.PropertyType);
                    var valueProperty = property.PropertyType.GetProperty("Value");
                    if (valueProperty is not null)
                    {
                        valueProperty.SetValue(wrapperInstance, GetRandomValue(valueProperty.PropertyType));
                        property.SetValue(_scoringRequest, wrapperInstance);
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