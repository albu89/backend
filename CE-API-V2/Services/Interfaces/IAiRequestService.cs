﻿using CE_API_V2.Models;
namespace CE_API_V2.Services.Interfaces;

public interface IAiRequestService
{
    public Task<ScoringResponseModel>? RequestScore(ScoringRequestModel scoringRequestModel);
}