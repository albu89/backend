namespace CE_API_V2.Models.DTO;

public class BiomarkerValue<T>
{
    public T Value { get; set; }
    public string UnitType { get; set; }
    public string DisplayValue { get; set; }
}

public class BiomarkerReturnValue<T>
{
    public string Id { get; set; }
    public T Value { get; set; }
    public string Unit { get; set; }
    public string DisplayValue { get; set; }
}