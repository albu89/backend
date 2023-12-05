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
    public virtual IEnumerable<BiomarkersDraft>? BiomarkersDraft { get; set; } = new List<BiomarkersDraft>();
    public virtual Biomarkers? LatestBiomarkers => Biomarkers?.MaxBy(t => t.CreatedOn) ?? null;
    public virtual BiomarkersDraft? LatestBiomarkersDraft => BiomarkersDraft?.MaxBy(t => t.CreatedOn) ?? null;
    public virtual IEnumerable<ScoringResponseModel>? Responses { get; set; } = new List<ScoringResponseModel>();
    public virtual ScoringResponseModel? LatestResponse => LatestBiomarkers?.Response;
    public DateTimeOffset CreatedOn { get; init; }
    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }
    public void AddBiomarkers(Biomarkers biomarkers)
    {
        if (Biomarkers.Any(b => b.Id == biomarkers.Id))
            return;
        Biomarkers = Biomarkers?.Append(biomarkers).ToList();
    }
    public void AddDraftBiomarkers(BiomarkersDraft biomarkers)
    {
        if (BiomarkersDraft.Any(b => b.Id == biomarkers.Id))
            return;
        BiomarkersDraft = BiomarkersDraft?.Append(biomarkers).ToList();
    }
}

public class ScoringRequestModelDraft : ScoringRequestModel
{
    public override IEnumerable<Biomarkers>? Biomarkers { get; set; } = new List<Biomarkers>();
    public override Biomarkers? LatestBiomarkers => Biomarkers?.MaxBy(t => t.CreatedOn) ?? null;
    public override IEnumerable<ScoringResponseModel>? Responses { get; set; } = new List<ScoringResponseModel>();
    public override ScoringResponseModel? LatestResponse => LatestBiomarkers?.Response;
}
