using CE_API_V2.Models;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.UnitOfWorks;

public interface IValueConversionUOW
{
    public ScoringRequest ConvertToScoringRequest(ScoringRequestDto scoringRequestDto, string userId, string patientId);
}