using System.Text.Json.Serialization;
using CE_API_V2.Models.Enum;

namespace CE_API_V2.Models.DTO;
public class UpdateUser
{
    //Person related
    public string Salutation { get; set; }
    public string Title { get; set; }
    public string Surname { get; set; }
    public string FirstName { get; set; }
    public string ProfessionalSpecialisation { get; set; }
    public string PreferredLab { get; set; } // for Cardio Explorer test analysis
    public string? Department { get; set; }
    public string? Organization { get; set; }

    //Contact details
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string? CountryCode { get; set; }
    public string Country { get; set; }
    public string TelephoneNumber { get; set; }
    public string EMailAddress { get; set; }

    //Settings
    public string Language { get; set; }
    public string? UnitLabValues { get; set; }
    public bool IsActive { get; set; }

    //Billing
    public bool IsSeparateBilling { get; set; }

    public Billing? Billing { get; set; }
}

public class CreateUser : UpdateUser
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }

    //Clinical intended use
    public string ProfessionalSpecialisation { get; set; }
    public string PreferredLab { get; set; } // for Cardio Explorer test analysis
}

public class Billing
{
    public string? BillingName { get; set; }
    public string? BillingAddress { get; set; }
    public string? BillingZip { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingCountry { get; set; }
    public string? BillingCountryCode { get; set; }
    public string? BillingPhone { get; set; }
}