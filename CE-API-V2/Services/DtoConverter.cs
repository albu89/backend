using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;
namespace CE_API_V2.Services
{
    public static class DtoConverter
    {
        public static AiDto ConvertToAiDto(Biomarkers biomarkers)
        {
            var patientToAiDto = new AiDto();    
            patientToAiDto.Id = new Guid(); //Todo
            patientToAiDto.Datum = DateTime.Now;
            patientToAiDto.Age = biomarkers.Age;
            patientToAiDto.Sex_0_female_1male = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Sex);
            patientToAiDto.Gr_sse = biomarkers.Height;
            patientToAiDto.Gewicht = biomarkers.Weight;
            patientToAiDto.Thoraxschmerzen__0_keine_1_extr = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Chestpain);
            patientToAiDto.Nicotin_0_nein_1_St__N__2_ja = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Nicotine);

            patientToAiDto.Diabetes_0_no_1_NIDDM_2_IDDM = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Diabetes);
            patientToAiDto.Statin_od_Chol_senker = TypeToFloatConverter.MapBoolToFloat(biomarkers.Statin);
            patientToAiDto.Tc_Aggregation = TypeToFloatConverter.MapBoolToFloat(biomarkers.Tcagginhibitor);
            patientToAiDto.ACE_od_ATII = TypeToFloatConverter.MapBoolToFloat(biomarkers.Aceinhibitor);
            patientToAiDto.CA_Antagonist = TypeToFloatConverter.MapBoolToFloat(biomarkers.Calciumant);
            patientToAiDto.Betablocker = TypeToFloatConverter.MapBoolToFloat(biomarkers.Betablocker);
            patientToAiDto.Diureticum = TypeToFloatConverter.MapBoolToFloat(biomarkers.Diuretic);
            patientToAiDto.Nitrat_od_Dancor = TypeToFloatConverter.MapBoolToFloat(biomarkers.Nitrate);

            patientToAiDto.BD_syst = biomarkers.Systolicbp;
            patientToAiDto.BD_diast = biomarkers.Diastolicbp;
            patientToAiDto.q_Zacken_0_nein_1_ja = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Qwave);

            patientToAiDto.Pankreas_Amylase = biomarkers.Amylasep;
            patientToAiDto.Alk_Phase = biomarkers.Alkaline;
            // The SI Unit is ng/L and entered in the system as such. The AI was trained on ng/mL however. 
            patientToAiDto.Troponin = biomarkers.Hstroponint;
            patientToAiDto.ALAT = biomarkers.Alat;

            patientToAiDto.Glucose = biomarkers.Glucose;

            patientToAiDto.Bilirubin = biomarkers.Bilirubin;
            patientToAiDto.Harnstoff = biomarkers.Urea;
            patientToAiDto.Harnsaure = biomarkers.Uricacid;

            patientToAiDto.Cholesterin_gesamt = biomarkers.Cholesterol;
            patientToAiDto.HDL = biomarkers.Hdl;
            patientToAiDto.LDL = biomarkers.Ldl;

            patientToAiDto.Total_Proteine = biomarkers.Protein;
            patientToAiDto.Albumin = biomarkers.Albumin;

            patientToAiDto.Leuko = biomarkers.Leukocyte;
            patientToAiDto.MCHC__g_l_oder___ = biomarkers.Mchc;

            return patientToAiDto;
        }
    }
}
