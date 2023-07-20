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

        public ScoringRequest ConvertToScoringRequest(ScoringRequestDto scoringRequestDto, string userId, string patientId)
        {
            var requestModel = _mapper.Map<ScoringRequest>(scoringRequestDto);
            requestModel.UserId = userId;
            requestModel.PatientId = patientId;

            return requestModel;
        }

        public void ConvertToSiValues(ScoringRequest scoringRequest)
        {
        }
    }
}
