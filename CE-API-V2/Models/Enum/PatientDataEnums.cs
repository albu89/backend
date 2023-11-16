namespace CE_API_V2.Models.Enum
{
    public static class PatientDataEnums
    {
        public enum ClinicalSetting
        {
            PrimaryCare,
            SecondaryCare
        }

        public enum NicotineConsumption
        {
            No,
            StANc,
            Yes
        }

        public enum ChestPain
        {
            No = 0,
            Unspecific = 1,
            Possible = 2,
            Typical = 4
        }

        public enum Sex
        {
            Female,
            Male,
        }

        public enum DiabetesStatus
        {
            No,
            Niddm,
            Iddm
        }

        public enum RestingEcg
        {
            No,
            Yes,
            Screening
        }
    }
}