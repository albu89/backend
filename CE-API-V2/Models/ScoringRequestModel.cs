using System.ComponentModel.DataAnnotations;
namespace CE_API_V2.Models;

public class ScoringRequestModel
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string PatientId { get; set; }
    public virtual Biomarkers? Biomarkers { get; set; }
    public DateTimeOffset CreatedOn { get; }
    public virtual ScoringResponseModel? Response { get; set; }
}