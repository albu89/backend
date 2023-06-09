namespace CE_API_V2.Models.DTO;

public class BiomarkerValueDto<T>
{
    public T Value { get; set; }
    public string UnitType { get; set; }
}