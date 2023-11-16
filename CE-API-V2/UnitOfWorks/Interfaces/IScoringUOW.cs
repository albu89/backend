﻿using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using static CE_API_V2.Models.Enum.PatientDataEnums;

namespace CE_API_V2.UnitOfWorks.Interfaces;

public interface IScoringUOW
{
    IGenericRepository<ScoringRequestModel> ScoringRequestRepository { get; }
    
    IGenericRepository<ScoringResponseModel> ScoringResponseRepository { get; }

    public IGenericRepository<Biomarkers> BiomarkersRepository { get; }

    public Biomarkers StoreBiomarkers(Guid scoringRequestId, Biomarkers biomarkers);

    public Task<ScoringRequestModel> StoreDraftRequest(ScoringRequest value, string userId, string patientId, ClinicalSetting clinicalSetting);

    ScoringRequestModel StoreScoringRequest(ScoringRequestModel scoringRequestModel, string UserId);
    
    ScoringRequestModel RetrieveScoringRequest(Guid ScoringRequestId, string userId);
    
    ScoringResponseModel StoreScoringResponse(ScoringResponseModel scoringResponseModel);
    
    IEnumerable<SimpleScore>? RetrieveScoringHistoryForUser(string UserId);
    
    IEnumerable<SimpleScore> RetrieveScoringHistoryForPatient(string PatientId, string UserId);
    
    ScoringResponseModel RetrieveScoringResponse(Guid ScoringRequestId, string UserId);
    
    Task<ScoringResponse> ProcessScoringRequest(ScoringRequest scoringRequestModel, string userId, string patientId, PatientDataEnums.ClinicalSetting clinicalSetting, Guid? existingScoringRequest = null);
    
    ScoringResponse GetScoringResponse(ScoringResponseModel recentScore, Biomarkers biomarkers, Guid requestId);
}