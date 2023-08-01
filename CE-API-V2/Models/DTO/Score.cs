namespace CE_API_V2.Models.DTO;

public class Score
{
    public ScoringRequest Request { get; set; }
    public ScoringResponse Response { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public Guid Id { get; set; } // Identical to RequestId?
}