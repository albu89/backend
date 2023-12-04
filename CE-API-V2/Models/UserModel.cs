using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;

namespace CE_API_V2.Models
{
    public class UserModel
    {
        public ICollection<BiomarkerOrderModel> BiomarkerOrders { get; set; }

        //Not transmitted by the frontend-Dto
        [Key]
        public string UserId { get; set; }
        public string TenantID { get; set; }

        //User related information
        public string Salutation { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public UserRole Role { get; set; }

        //ContactDetails?
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string? CountryCode { get; set; }
        public string Country { get; set; }
        public string TelephoneNumber { get; set; }
        public string EMailAddress { get; set; }
        public DateTimeOffset CreatedOn { get; }

        //Settings
        public string Language { get; set; }
        public string? UnitLabValues { get; set; }
        public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }
        public IEnumerable<ScoringRequestModel> ScoringRequestModels { get; set; }
        public bool IsActive { get; set; }

        //Clinical intended use
        public string ProfessionalSpecialisation { get; set; }
        public string? Department { get; set; }
        public string? Organization { get; set; }
        public string PreferredLab { get; set; } // for Cardio Explorer test analysis

        //Billing
        public bool IsSeparateBilling { get; set; }

        public Guid BillingId { get; set; }
        public BillingModel Billing { get; set; }
    }

    public class BillingModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? BillingName { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingZip { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingCountry { get; set; }
        public string? BillingCountryCode { get; set; }
        public string? BillingPhone { get; set; }
        public UserModel UserModel { get; set; }
    }
}