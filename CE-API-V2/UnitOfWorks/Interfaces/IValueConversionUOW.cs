using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;

namespace CE_API_V2.UnitOfWorks;

public interface IValueConversionUOW
{
    public (ScoringRequestModel, Biomarkers) ConvertToScoringRequest(ScoringRequest scoringRequest, string userId, string patientId, PatientDataEnums.ClinicalSetting clinicalSetting);
    public Task<ScoringRequest> ConvertToSiValues(ScoringRequest scoringRequest);
}