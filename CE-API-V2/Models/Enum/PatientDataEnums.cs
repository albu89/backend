using System.ComponentModel;
namespace CE_API_V2.Models.Enum
{
    public static class PatientDataEnums
    {
        public enum ClinicalSetting
        {
            [Description("Primary Care")]
            PrimaryCare,
            [Description("Secondary Care")]
            SecondaryCare
        }
        
        public enum NicotineConsumption
        {
            [Description("non-smoker")]
            No,
            [Description("ex-smoker")]
            StANc,
            [Description("smoker")]
            Yes
        }

        public enum ChestPain
        {
            No,
            Unspecific,
            Possible,
            Typical
        }

        public enum Sex 
        {
            Male,
            Female,
        }

        public enum DiabetesStatus
        {
            No,
            Niddm,
            Iddm
        }

        public enum RestingEcg
        {
            [Description("no ECG available")]
            No, // only primary care!
            [Description("normal")]
            Yes,
            [Description("pathological")]
            Screening
        }
    }
}
