using AutoMapper;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.UnitOfWorks
{
    public class ValueConversionUOW : IValueConversionUOW
    {
        private readonly IMapper _mapper;

        public ValueConversionUOW(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ScoringRequestModel ConvertToScoringRequest(ScoringRequest scoringRequest, string userId, string patientId)
        {
            var requestModel = _mapper.Map<ScoringRequestModel>(scoringRequest);
            requestModel.UserId = userId;
            requestModel.PatientId = patientId;

            return requestModel;
        }

        public void ConvertToSiValues(ScoringRequestModel scoringRequest)
        {
        }
    }
}
