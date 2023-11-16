using System.ComponentModel.DataAnnotations;
using CE_API_V2.Models.Enum;

namespace CE_API_V2.Models;

public class ScoringRequestModel
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public UserModel User { get; set; }
    public string PatientId { get; set; }
    public virtual IEnumerable<Biomarkers>? Biomarkers { get; set; } = new List<Biomarkers>();
    public virtual Biomarkers? LatestBiomarkers => Biomarkers?.MaxBy(t => t.CreatedOn) ?? null;
    public virtual IEnumerable<ScoringResponseModel>? Responses { get; set; } = new List<ScoringResponseModel>();
    public virtual ScoringResponseModel LatestResponse => LatestBiomarkers?.Response;
    public DateTimeOffset CreatedOn { get; init; }
    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }
    public void AddBiomarkers(Biomarkers biomarkers)
    {
        if (Biomarkers.Any(b => b.Id == biomarkers.Id))
            return;
        Biomarkers = Biomarkers?.Append(biomarkers).ToList();
    }
}
