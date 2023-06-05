﻿using System.Text.Json;
using CE_API_V2.DTO;
using CE_API_V2.Models;
using CE_API_V2.Utility;
using static CE_API_V2.DTO.Enums.PatientDataEnums;

namespace CE_API_Test.TestUtility
{
    internal static class MockDataProvider
    {
        internal static ScoringRequestDto GetMockedScoringRequestDto()
        {
            var mockingService = new MockedInputPatientDataService();
            var biomarkers = mockingService.GetPatientBiomarkers("2");

            return biomarkers;
        }

        internal static string GetMockedSerializedScoringRequestDto()
        {
            var scoringRequestDtoMock = new ScoringRequestDto()
            {
                Age = new BiomarkerValueDto<float>() { Value = 2.0f },
                ACEInhibitor = new BiomarkerValueDto<bool>() { Value = true },
                Alat = new BiomarkerValueDto<float>() { Value = 2.0f },
                Albumin = new BiomarkerValueDto<float>() { Value = 2.0f },
                AlkalinePhosphatase = new BiomarkerValueDto<float>() { Value = 2.0f },
                Betablocker = new BiomarkerValueDto<bool>() { Value = true },
                Bilirubin = new BiomarkerValueDto<float>() { Value = 2.0f },
                CaAntagonist = new BiomarkerValueDto<bool>() { Value = true },
                Cholesterol = new BiomarkerValueDto<float>() { Value = 2.0f },
                CholesterolLowering_Statin = new BiomarkerValueDto<bool>() { Value = true },
                Diabetes = new BiomarkerValueDto<DiabetesStatusEnum>() { Value = DiabetesStatusEnum.IDDM },
                DiastolicBloodPressure = new BiomarkerValueDto<float>() { Value = 2.0f },
                Diuretic = new BiomarkerValueDto<bool>() { Value = true },
                GlocuseFasting = new BiomarkerValueDto<float>() { Value = 2.0f },
                Hdl = new BiomarkerValueDto<float>() { Value = 2.0f },
                Height = new BiomarkerValueDto<float>() { Value = 2.0f },
                HsTroponinT = new BiomarkerValueDto<float>() { Value = 2.0f },
                Ldl = new BiomarkerValueDto<float>() { Value = 2.0f },
                Leukocytes = new BiomarkerValueDto<float>() { Value = 2.0f },
                Mchc = new BiomarkerValueDto<float>() { Value = 2.0f },
                NicotineConsumption = new BiomarkerValueDto<NicotineConsumptionEnum>() { Value = NicotineConsumptionEnum.St_a_Nc },
                OrganicNitrate = new BiomarkerValueDto<bool>() { Value = true },
                PancreaticAmylase = new BiomarkerValueDto<float>() { Value = 2.0f },
                PatientId = new BiomarkerValueDto<string>() { Value = "Mock" },
                Protein = new BiomarkerValueDto<float>() { Value = 2.0f },
                RestingECG = new BiomarkerValueDto<RestingECGEnum>() { Value = RestingECGEnum.screening },
                Sex = new BiomarkerValueDto<SexEnum>() { Value = SexEnum.female },
                SystolicBloodPressure = new BiomarkerValueDto<float>() { Value = 2.0f },
                TCAggregationInhibitor = new BiomarkerValueDto<bool>() { Value = true },
                ChestPain = new BiomarkerValueDto<ThoraicPainEnum>() { Value = ThoraicPainEnum.possible },
                Urea = new BiomarkerValueDto<float>() { Value = 2.0f },
                UricAcid = new BiomarkerValueDto<float>() { Value = 2.0f },
                Weight = new BiomarkerValueDto<float>() { Value = 2.0f },
                clinical_setting = new BiomarkerValueDto<string>() { Value = "Mock" },
                prior_CAD = new BiomarkerValueDto<bool>() { Value = true },
            };

            return JsonSerializer.Serialize(scoringRequestDtoMock);
        }
        
        internal static string GetMockedSerializedResponse()
        {
            var mockedResponse = GetMockedScoringResponse();
            return JsonSerializer.Serialize(mockedResponse);
        }

        internal static string GetExpectedQueryString() =>
            "Datum=02/02/2000 00:00:00&Age=2&Sex_0_female_1male=2&Gr_sse=2&Gewicht=2&" +
            "Thoraxschmerzen__0_keine_1_extr=2&Nicotin_0_nein_1_St__N__2_ja=2&Diabetes_0_no_1_NIDDM_2_IDDM=2&Statin_od_Chol_senker=2&" +
            "Tc_Aggregation=2&ACE_od_ATII=2&CA_Antagonist=2&Betablocker=2&Diureticum=2&Nitrat_od_Dancor=2&BD_syst=2&BD_diast=2&" +
            "q_Zacken_0_nein_1_ja=0&Pankreas_Amylase=2&Alk_Phase=2&Troponin=2&ALAT=2&Glucose=2&Bilirubin=2&Harnstoff=2&Harnsaure=2&" +
            "Cholesterin_gesamt=2&HDL=2&LDL=2&Total_Proteine=2&Albumin=2&Leuko=2&MCHC__g_l_oder___=2&CustomToken=mock&" +
            "incomplete=True&chosenOrgClient=mock&promocode=mock&classifier_type=mock&overwrite=True&PopulationRiskLevel=mock";

        internal static ScoringResponse GetMockedScoringResponse() =>
            new()
            {
                Class = 2,
                Id = 3,
                Score = 4.0f,
                classifier_class = 5,
                classifier_score = 6.0,
                classifier_sign = 7,
                classifier_type = "mock",
                error_code = 0,
                hidden = true,
                is_CAD_plus = 0,
                is_H_plus = 1,
                message = "mock",
                orgclient = "mock",
                timestamp = 9,
                username = "mock"
            };

        internal static AiDto GetMockedAiDto()
        {
            var mockedAiDto = new AiDto();

            foreach (var property in mockedAiDto.GetType().GetProperties())
            {
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) != null
                    ? Nullable.GetUnderlyingType(property.PropertyType)
                    : property.PropertyType;

                switch (propertyType.Name)
                {
                    case "String":
                        property.SetValue(mockedAiDto, "mock");
                        break;
                    case "Boolean":
                        property.SetValue(mockedAiDto, true);
                        break;
                    case "Single":
                        property.SetValue(mockedAiDto, 2.0f);
                        break;
                    case "Int32":
                        property.SetValue(mockedAiDto, 10);
                        break;
                    case "DateTime":
                        property.SetValue(mockedAiDto, new DateTime(2000, 2, 2));
                        break;
                    case "Guid":
                        property.SetValue(mockedAiDto, new Guid());
                        break;
                }

            }

            return mockedAiDto;
        }
    }
}
