namespace CE_API_V2.Models.DTO;

public class ScoringHistoryDto
{
    public Guid RequestId { get; set; }
    public DateTimeOffset RequestTimeStamp { get; set; }
    public float Score { get; set; }
}