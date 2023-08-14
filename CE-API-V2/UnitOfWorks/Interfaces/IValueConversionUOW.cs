using CE_API_V2.Models;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.UnitOfWorks;

public interface IValueConversionUOW
{
    public (ScoringRequestModel, Biomarkers) ConvertToScoringRequest(ScoringRequest scoringRequest, string userId, string patientId);
    
    public Task<ScoringRequest> ConvertToSiValues(ScoringRequest scoringRequest);
}