using CE_API_V2.Models.DTO;
namespace CE_API_V2.Models;

public class ScoringRequest
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string PatientId { get; set; }
    public Biomarkers Biomarkers { get; set; }
    public ScoringResponse? Response { get; set; }
}