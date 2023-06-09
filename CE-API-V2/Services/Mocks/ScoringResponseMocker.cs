using CE_API_V2.Models;
namespace CE_API_V2.Services.Mocks
{
    public class ScoringResponseMocker
    {
        public ScoringResponse MockScoringResponse()
        {
            return new ScoringResponse()
            {
                Id = 1660033,
                classifier_class = 2,
                classifier_score = 0.28023079037666321,
                classifier_sign = 0,
                classifier_type = "Low",
                hidden = false,
                orgclient = "client",
                timestamp = 1685437831,
                username = "end_user_1",
                Class = 2,
                Score = 0.28023079037666321f,
                error_code = 0,
                is_CAD_plus = 1,
                is_H_plus = 1,
                message = "No further examination necessary"
            };
        }
    }
}
