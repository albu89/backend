using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
namespace CE_API_V2.UnitOfWorks.Interfaces;

public interface IScoringUOW
{
    IGenericRepository<ScoringRequestModel> ScoringRequestRepository { get; }
    
    IGenericRepository<ScoringResponseModel> ScoringResponseRepository { get; }
    
    ScoringRequestModel StoreScoringRequest(ScoringRequestModel scoringRequestModel, string UserId);
    
    ScoringRequestModel RetrieveScoringRequest(Guid ScoringRequestId, string userId);
    
    ScoringResponseModel StoreScoringResponse(ScoringResponseModel scoringResponseModel);
    
    IEnumerable<SimpleScore>? RetrieveScoringHistoryForUser(string UserId);
    
    IEnumerable<SimpleScore> RetrieveScoringHistoryForPatient(string PatientId, string UserId);
    
    ScoringResponseModel RetrieveScoringResponse(Guid ScoringRequestId, string UserId);
    
    Task<ScoringResponseModel> ProcessScoringRequest(ScoringRequest scoringRequestModel, string userId, string patientId);
    
    ScoringResponse GetScoreSummary(ScoringResponseModel recentScore);
}