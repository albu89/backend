using CE_API_V2.Models.Enum;

namespace CE_API_V2.Models.DTO;

public class User
{
    //Person related
    public string Salutation { get; set; }
    public string Title { get; set; }
    public string Surname { get; set; }
    public string FirstName { get; set; }
    public string ProfessionalSpecialisation { get; set; }
    public string Department { get; set; }
    public string PreferredLab { get; set; } // for Cardio Explorer test analysis

    //Contact details
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public string Country { get; set; }
    public string TelephoneNumber { get; set; }
    public string EMailAddress { get; set; }

    //Settings
    public string Language { get; set; }
    public string UnitLabValues { get; set; }
    public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }
    public UserRole Role { get; set; }
    
    public IEnumerable<BiomarkerOrder> BiomarkerOrders {get; set; }
}