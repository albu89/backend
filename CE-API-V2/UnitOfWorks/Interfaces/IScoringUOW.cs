using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
namespace CE_API_V2.UnitOfWorks.Interfaces;

public interface IScoringUOW
{
    IGenericRepository<ScoringRequest> ScoringRequestRepository { get; }
    
    IGenericRepository<ScoringResponse> ScoringResponseRepository { get; }
    
    ScoringRequest StoreScoringRequest(ScoringRequest scoringRequest, string UserId);
    
    ScoringRequest RetrieveScoringRequest(Guid ScoringRequestId, string userId);
    
    ScoringResponse StoreScoringResponse(ScoringResponse scoringResponse);
    
    IEnumerable<ScoringHistoryDto>? RetrieveScoringHistoryForUser(string UserId);
    
    IEnumerable<ScoringHistoryDto> RetrieveScoringHistoryForPatient(string PatientId, string UserId);
    
    ScoringResponse RetrieveScoringResponse(Guid ScoringRequestId, string UserId);
    
    Task<ScoringResponse> ProcessScoringRequest(ScoringRequestDto scoringRequest, string userId, string patientId);
    ScoringResponseSummary GetScoreSummary(ScoringResponse recentScore);
}