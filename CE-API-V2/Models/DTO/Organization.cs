using System.Text.Json.Serialization;
namespace CE_API_V2.Models.DTO;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class CreateOrganization 
{
    public string Name { get; set; }
    public Guid TenantId { get; set; }
    public string ContactEmail { get; set; }
}