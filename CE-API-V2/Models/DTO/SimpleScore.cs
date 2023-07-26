namespace CE_API_V2.Models.DTO;

public class SimpleScore
{
    public Guid RequestId { get; set; }
    public DateTimeOffset RequestTimeStamp { get; set; }
    public float Score { get; set; }
    public string Risk { get; set; }
    public int RiskClass { get; set; }
}