using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using System.Text.Json.Serialization;

namespace CE_API_V2.Models
{
    public class UserInputFormSchema : UserInputFormSchemaHeaders
    {
        #region CreateUser
        //CreateUser
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PatientDataEnums.ClinicalSetting ClinicalSetting { get; set; }

        //Clinical intended use
        public string Department { get; set; }
        public string Organization { get; set; }
        #endregion

        #region UpdateUser
        //UpdateUser
        //Person related
        public string Salutation { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string ProfessionalSpecialisation { get; set; }
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
        public bool IsActive { get; set; }
        //Billing
        public bool IsSeparateBilling { get; set; }
        #endregion
    }

    public class UserInputFormSchemaHeaders
    {
        public string SalutationHeader { get; set; }
        public string TitleHeader { get; set; }
        public string SurnameHeader { get; set; }
        public string FirstNameHeader { get; set; }
        public string ProfessionalSpecialisationHeader { get; set; }
        public string PreferredLabHeader { get; set; } // for Cardio Explorer test analysis

        //Contact details headers
        public string AddressHeader { get; set; }
        public string CityHeader { get; set; }
        public string ZipCodeHeader { get; set; }
        public string CountryCodeHeader { get; set; }
        public string CountryHeader { get; set; }
        public string TelephoneNumberHeader { get; set; }
        public string EMailAddressHeader { get; set; }

        //Settings headers
        public string LanguageHeader { get; set; }
        public string UnitLabValuesHeader { get; set; }
        public string IsActiveHeader { get; set; }

        //Billing headers
        public string IsSeparateBillingHeader { get; set; }

        public BillingTemplate Billing { get; set; }

        //create user fields
        public string ClinicalSettingHeader { get; set; }

        //Clinical intended use
        public string DepartmentHeader { get; set; }
        public string OrganizationHeader { get; set; }
        public string IntendedUseClinicalSettingHeader { get; set; }
        public string IntendedUseClinicalSetting { get; set; }
        public string ChangeClinicalSettingHeader { get; set; }
        public string ChangeClinicalSetting { get; set; }
    }

    public class BillingTemplate : Billing
    {
        public string BillingNameHeader { get; set; }
        public string BillingAddressHeader { get; set; }
        public string BillingZipHeader { get; set; }
        public string BillingCityHeader { get; set; }
        public string BillingCountryHeader { get; set; }
        public string BillingCountryCodeHeader { get; set; }
        public string BillingPhoneHeader { get; set; }
    }
}
