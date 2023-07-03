using CE_API_V2.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace CE_API_V2.Models
{
    public class User
    {
        //Not transmitted by the frontend-Dto
        [Key]
        public string UserId { get; set; }
        public string TenantID { get; set; }

        //User related information
        public string Salutation { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string ProfessionalSpecialisation { get; set; }
        public string Department { get; set; }
        public string PreferredLab { get; set; } // for Cardio Explorer test analysis
        public UserRole Role { get; set; }

        //ContactDetails?
        public string Address { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string TelephoneNumber { get; set; }
        public string EMailAdress { get; set; }
        public DateTimeOffset CreatedOn { get; }

        //Settings
        public string Language { get; set; }
        public string UnitLabValues { get; set; } //Todo FK checken
        //public string BioMarkerOrder { get; set; } // Todo - ist das hier auch benötigt? 
        public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }
    }
}
