using CE_API_V2.Models;
namespace CE_API_V2.Services.Mocks
{
    public class ScoringResponseMocker
    {
        public ScoringResponse MockScoringResponse()
        {
            return new ScoringResponse()
            {
                Id = new Guid(),
                classifier_class = 2,
                classifier_score = 0.28023079037666321,
                classifier_sign = 0,
            };
        }
    }
}
