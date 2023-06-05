using CE_API_V2.DTO;
using CE_API_V2.Models;

namespace CE_API_V2.Services;

public interface IAiRequestService
{
    public Task<ScoringResponse>? RequestScore(ScoringRequestDto scoringRequestDto);
}