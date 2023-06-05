using CE_API_V2.DTO;

namespace CE_API_V2.Utility
{
    public static class DtoConverter
    {
        public static AiDto ConvertToAiDto(ScoringRequestDto patientDetailWebDto)
        {
            var patientToAiDto = new AiDto();    
            patientToAiDto.Id = new Guid(); //Todo
            patientToAiDto.Datum = DateTime.Now;
            patientToAiDto.Age = (float)patientDetailWebDto.Age.Value;
            patientToAiDto.Sex_0_female_1male = TypeToFloatConverter.MapEnumValueToFloat(patientDetailWebDto.Sex.Value);
            patientToAiDto.Gr_sse = (float)patientDetailWebDto.Height.Value;
            patientToAiDto.Gewicht = (float)patientDetailWebDto.Weight.Value;
            patientToAiDto.Thoraxschmerzen__0_keine_1_extr = TypeToFloatConverter.MapEnumValueToFloat(patientDetailWebDto.ChestPain.Value);
            patientToAiDto.Nicotin_0_nein_1_St__N__2_ja = TypeToFloatConverter.MapEnumValueToFloat(patientDetailWebDto.NicotineConsumption.Value);

            patientToAiDto.Diabetes_0_no_1_NIDDM_2_IDDM = TypeToFloatConverter.MapEnumValueToFloat(patientDetailWebDto.Diabetes.Value);
            patientToAiDto.Statin_od_Chol_senker = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.CholesterolLowering_Statin.Value);
            patientToAiDto.Tc_Aggregation = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.TCAggregationInhibitor.Value);
            patientToAiDto.ACE_od_ATII = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.ACEInhibitor.Value);
            patientToAiDto.CA_Antagonist = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.CaAntagonist.Value);
            patientToAiDto.Betablocker = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.Betablocker.Value);
            patientToAiDto.Diureticum = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.Diuretic.Value);
            patientToAiDto.Nitrat_od_Dancor = TypeToFloatConverter.MapBoolToFloat(patientDetailWebDto.OrganicNitrate.Value);

            patientToAiDto.BD_syst = (float)patientDetailWebDto.SystolicBloodPressure.Value;
            patientToAiDto.BD_diast = (float)patientDetailWebDto.DiastolicBloodPressure.Value;
            patientToAiDto.q_Zacken_0_nein_1_ja = TypeToFloatConverter.MapEnumValueToFloat(patientDetailWebDto.RestingECG.Value);

            patientToAiDto.Pankreas_Amylase = (float)patientDetailWebDto.PancreaticAmylase.Value;
            patientToAiDto.Alk_Phase = (float)patientDetailWebDto.AlkalinePhosphatase.Value;
            patientToAiDto.Troponin = (float)patientDetailWebDto.HsTroponinT.Value;
            patientToAiDto.ALAT = (float)patientDetailWebDto.Alat.Value;

            patientToAiDto.Glucose = (float)patientDetailWebDto.GlocuseFasting.Value;

            patientToAiDto.Bilirubin = (float)patientDetailWebDto.Bilirubin.Value;
            patientToAiDto.Harnstoff = (float)patientDetailWebDto.Urea.Value;
            patientToAiDto.Harnsaure = (float)patientDetailWebDto.UricAcid.Value;

            patientToAiDto.Cholesterin_gesamt = (float)patientDetailWebDto.Cholesterol.Value;
            patientToAiDto.HDL = (float)patientDetailWebDto.Hdl.Value;
            patientToAiDto.LDL = (float)patientDetailWebDto.Ldl.Value;

            patientToAiDto.Total_Proteine = (float)patientDetailWebDto.Protein.Value;
            patientToAiDto.Albumin = (float)patientDetailWebDto.Albumin.Value;

            patientToAiDto.Leuko = (float)patientDetailWebDto.Leukocytes.Value;
            patientToAiDto.MCHC__g_l_oder___ = (float)patientDetailWebDto.Mchc.Value;

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
