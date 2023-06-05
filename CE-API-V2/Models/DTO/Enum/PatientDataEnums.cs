using System.ComponentModel;

namespace CE_API_V2.DTO.Enums
{
    public static class PatientDataEnums
    {
        public enum NicotineConsumptionEnum
        {
            [Description("non-smoker")]
            no,
            [Description("ex-smoker")]
            St_a_Nc,
            [Description("smoker")]
            yes
        }

        public enum ThoraicPainEnum
        {
            no,
            unspecific,
            possible,
            typical
        }

        public enum SexEnum // Todo: divers? weitere Bezeichnungen?
        {
            male,
            female,
        }

        public enum DiabetesStatusEnum
        {
            No,
            NIDDM,
            IDDM
        }

        public enum RestingECGEnum //Todo correct matching -> description - value?
        {
            [Description("no ECG available")]
            no, // only primary care!
            [Description("normal")]
            yes,
            [Description("pathological")]
            screening
        }
    }
}
