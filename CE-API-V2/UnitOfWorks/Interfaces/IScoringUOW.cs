using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
namespace CE_API_V2.UnitOfWorks.Interfaces;

public interface IScoringUOW
{
    IGenericRepository<ScoringRequest> ScoringRequestRepository { get; }
    
    IGenericRepository<ScoringResponse> ScoringResponseRepository { get; }
    
    ScoringRequest StoreScoringRequest(ScoringRequest storingRequest, string UserId);
    
    ScoringRequest RetrieveScoringRequest(string ScoringRequestId, string userId);
    
    ScoringResponse StoreScoringResponse(ScoringResponse scoringResponse);
    
    IEnumerable<ScoringHistoryDto> RetrieveScoringHistoryForUser(string UserId);
    
    IEnumerable<ScoringHistoryDto> RetrieveScoringHistoryForPatient(string PatientId, string UserId);
    
    ScoringResponse RetrieveScoringResponse(string ScoringRequestId, string UserId);
    
    Task<ScoringResponse> ProcessScoringRequest(ScoringRequestDto scoringRequestDto, string userId);

}