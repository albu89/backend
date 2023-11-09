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
            No,
            Yes,
            Screening
        }
    }
}