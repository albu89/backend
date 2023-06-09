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
            patientToAiDto.Thoraxschmerzen__0_keine_1_extr = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.ChestPain);
            patientToAiDto.Nicotin_0_nein_1_St__N__2_ja = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Nicotine);

            patientToAiDto.Diabetes_0_no_1_NIDDM_2_IDDM = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.Diabetes);
            patientToAiDto.Statin_od_Chol_senker = TypeToFloatConverter.MapBoolToFloat(biomarkers.Statin);
            patientToAiDto.Tc_Aggregation = TypeToFloatConverter.MapBoolToFloat(biomarkers.TcAggInhibitor);
            patientToAiDto.ACE_od_ATII = TypeToFloatConverter.MapBoolToFloat(biomarkers.AceInhibitor);
            patientToAiDto.CA_Antagonist = TypeToFloatConverter.MapBoolToFloat(biomarkers.CaAntagonist);
            patientToAiDto.Betablocker = TypeToFloatConverter.MapBoolToFloat(biomarkers.Betablocker);
            patientToAiDto.Diureticum = TypeToFloatConverter.MapBoolToFloat(biomarkers.Diuretic);
            patientToAiDto.Nitrat_od_Dancor = TypeToFloatConverter.MapBoolToFloat(biomarkers.OganicNitrate);

            patientToAiDto.BD_syst = biomarkers.SystolicBloodPressure;
            patientToAiDto.BD_diast = biomarkers.DiastolicBloodPressure;
            patientToAiDto.q_Zacken_0_nein_1_ja = TypeToFloatConverter.MapEnumValueToFloat(biomarkers.RestingECG);

            patientToAiDto.Pankreas_Amylase = biomarkers.PancreaticAmylase;
            patientToAiDto.Alk_Phase = biomarkers.AlkalinePhosphate;
            patientToAiDto.Troponin = biomarkers.HsTroponinT;
            patientToAiDto.ALAT = biomarkers.Alat;

            patientToAiDto.Glucose = biomarkers.Glucose;

            patientToAiDto.Bilirubin = biomarkers.Bilirubin;
            patientToAiDto.Harnstoff = biomarkers.Urea;
            patientToAiDto.Harnsaure = biomarkers.UricAcid;

            patientToAiDto.Cholesterin_gesamt = biomarkers.Cholesterol;
            patientToAiDto.HDL = biomarkers.Hdl;
            patientToAiDto.LDL = biomarkers.Ldl;

            patientToAiDto.Total_Proteine = biomarkers.Protein;
            patientToAiDto.Albumin = biomarkers.Albumin;

            patientToAiDto.Leuko = biomarkers.Leukocytes;
            patientToAiDto.MCHC__g_l_oder___ = biomarkers.Mchc;

            //Todo - currently unused?
            //_patientToAiDto.CustomToken = string.Empty;
            //_patientToAiDto.ScroingRecordID = 0;
            //_patientToAiDto.incomplete = false;
            //_patientToAiDto.chosenOrgClient = string.Empty;
            //_patientToAiDto.promocode = string.Empty;
            //_patientToAiDto.classifier_type = string.Empty;
            //_patientToAiDto.units = string.Empty;
            //_patientToAiDto.overwrite = false;
            //_patientToAiDto.PopulationRiskLevel = string.Empty; 
            return patientToAiDto;
        }
    }
}
