namespace CE_API_V2.Models.DTO;

public class ScoringResponseDto
{
    public Guid Id { get; set; }

    public int? classifier_class { get; set; }
    public double? classifier_score { get; set; }
    public int? classifier_sign { get; set; }
    public DateTimeOffset CreatedOn { get; }
}