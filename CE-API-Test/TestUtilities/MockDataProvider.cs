using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Records;
using System.Collections.Immutable;
using System.Text.Json;
using static CE_API_V2.Models.Enum.PatientDataEnums;

namespace CE_API_Test.TestUtilities
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
            var scoringRequestDtoMock = GetMockedScoringRequestDto();

            return JsonSerializer.Serialize(scoringRequestDtoMock);
        }

        internal static ScoringRequestDto CreateScoringRequestDto()
        {
            return new ScoringRequestDto()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                DateOfBirth = DateTime.UtcNow,
                Age = new BiomarkerValueDto<int>() { Value = 20 },
                ACEInhibitor = new BiomarkerValueDto<bool>() { Value = true },
                Alat = new BiomarkerValueDto<float>() { Value = 2 },
                Albumin = new BiomarkerValueDto<float>() { Value = 2.0f },
                AlkalinePhosphatase = new BiomarkerValueDto<float>() { Value = 2 },
                Betablocker = new BiomarkerValueDto<bool>() { Value = true },
                Bilirubin = new BiomarkerValueDto<float>() { Value = 2.0f },
                CaAntagonist = new BiomarkerValueDto<bool>() { Value = true },
                Cholesterol = new BiomarkerValueDto<float>() { Value = 2.0f },
                CholesterolLowering_Statin = new BiomarkerValueDto<bool>() { Value = true },
                Diabetes = new BiomarkerValueDto<DiabetesStatus>() { Value = DiabetesStatus.Iddm },
                DiastolicBloodPressure = new BiomarkerValueDto<int>() { Value = 2 },
                Diuretic = new BiomarkerValueDto<bool>() { Value = true },
                GlucoseFasting = new BiomarkerValueDto<float>() { Value = 2.0f },
                Hdl = new BiomarkerValueDto<float>() { Value = 2.0f },
                Height = new BiomarkerValueDto<int>() { Value = 180 },
                HsTroponinT = new BiomarkerValueDto<float>() { Value = 2.0f },
                Ldl = new BiomarkerValueDto<float>() { Value = 2.0f },
                Leukocytes = new BiomarkerValueDto<float>() { Value = 2.0f },
                Mchc = new BiomarkerValueDto<float>() { Value = 2.0f },
                NicotineConsumption = new BiomarkerValueDto<NicotineConsumption>() { Value = NicotineConsumption.StANc },
                OrganicNitrate = new BiomarkerValueDto<bool>() { Value = true },
                PancreaticAmylase = new BiomarkerValueDto<float>() { Value = 2 },
                Protein = new BiomarkerValueDto<float>() { Value = 2.0f },
                RestingECG = new BiomarkerValueDto<RestingEcg>() { Value = RestingEcg.Screening },
                Sex = new BiomarkerValueDto<Sex>() { Value = Sex.Female },
                SystolicBloodPressure = new BiomarkerValueDto<int>() { Value = 2 },
                TCAggregationInhibitor = new BiomarkerValueDto<bool>() { Value = true },
                ChestPain = new BiomarkerValueDto<ChestPain>() { Value = ChestPain.Possible },
                Urea = new BiomarkerValueDto<float>() { Value = 2.0f },
                UricAcid = new BiomarkerValueDto<float>() { Value = 2.0f },
                Weight = new BiomarkerValueDto<int>() { Value = 100 },
                clinical_setting = new BiomarkerValueDto<ClinicalSetting>() { Value = ClinicalSetting.PrimaryCare },
                prior_CAD = new BiomarkerValueDto<bool>() { Value = true },
            };
        }

        internal static ScoringResponse GetMockedScoringResponseForRequest(ScoringRequest request)
        {
            var response = GetMockedScoringResponse();
            response.Request = request;
            response.RequestId = request.Id;
            return response;
        }

        internal static ScoringRequest GetMockedScoringRequest(string userId = "", string patientId = "")
        {
            return new ScoringRequest
            {
                Id = Guid.NewGuid(),
                PatientId = patientId.Equals(string.Empty) ? "PatientId" : patientId,
                UserId = userId.Equals(string.Empty) ? "UserId" : userId,
                Biomarkers = GetFakeBiomarkers()
            };
        }
        private static Biomarkers GetFakeBiomarkers()
        {
            return new Biomarkers
            {
                Age = 25,
                AgeUnit = "SI",
                Alat = 213,
                AlatUnit = "SI",
                Albumin = 123,
                AlbuminUnit = "SI",
                Bilirubin = 123,
                BilirubinUnit = "SI",
                Cholesterol = 123,
                CholesterolUnit = "SI",
                Diabetes = DiabetesStatus.Iddm,
                RestingECG = RestingEcg.Screening,
                Diuretic = false,
                Hdl = 123,
                HdlUnit = "SI",
                Height = 176,
                HeightUnit = "SI",
                Ldl = 123,
                LdlUnit = "SI",
                Leukocytes = 123,
                LeukocytesUnit = "SI",
                Mchc = 123,
                MchcUnit = "SI",
                Sex = Sex.Male,
                Weight = 90,
                WeightUnit = "SI",
                Protein = 123,
                ProteinUnit = "SI",
                Urea = 123,
                UreaUnit = "SI",
                CaAntagonist = false,
                ChestPain = ChestPain.Possible,
                Glucose = 123,
                GlucoseUnit = "SI",
                PancreaticAmylase = 123,
                PancreaticAmylaseUnit = "SI",
                UricAcid = 123,
                UricAcidUnit = "SI",
                DiastolicBloodPressure = 123,
                DiastolicBloodPressureUnit = "SI",
                HsTroponinT = 123,
                HsTroponinTUnit = "SI",
                SystolicBloodPressure = 123,
                SystolicBloodPressureUnit = "SI",
                Nicotine = NicotineConsumption.StANc,
                Statin = false,
                AceInhibitor = false,
                AlkalinePhosphate = 123,
                AlkalinePhosphataseUnit = "SI",
                Betablocker = false,
                ClinicalSetting = ClinicalSetting.PrimaryCare,
                OganicNitrate = false,
                TcAggInhibitor = false,
                PriorCAD = false
            };
        }

        internal static string GetMockedSerializedResponse()
        {
            var mockedResponse = GetMockedScoringResponse();
            return JsonSerializer.Serialize(mockedResponse);
        }

        internal static string GetExpectedQueryString() =>
            "Datum=02/02/2000 00:00:00&Age=2&Sex_0_female_1male=2&Gr_sse=2&Gewicht=2&Thoraxschmerzen__0_keine_1_extr=2&Nicotin_0_nein_1_St__N__2_ja=2&Diabetes_0_no_1_NIDDM_2_IDDM=2&Statin_od_Chol_senker=2&Tc_Aggregation=2&ACE_od_ATII=2&CA_Antagonist=2&Betablocker=2&Diureticum=2&Nitrat_od_Dancor=2&BD_syst=2&BD_diast=2&q_Zacken_0_nein_1_ja=0&Pankreas_Amylase=2&Alk_Phase=2&Troponin=2&ALAT=2&Glucose=2&Bilirubin=2&Harnstoff=2&Harnsaure=2&Cholesterin_gesamt=2&HDL=2&LDL=2&Total_Proteine=2&Albumin=2&Leuko=2&MCHC__g_l_oder___=2&ASAT=2&Art__Hypertonie=2&CK=2&Chlorid=2&Dyspnoe=2&Gamma_GT=2&Hypercholesterin_mie=2&INR=2&Interne_Nummer=2&Kalium=2&Kreatinin=2&MCV__fl_=2&Natrium=2&OAK=2&Phosphat=2&Repolarisationsst_runge=2";

        internal static ScoringResponse GetMockedScoringResponse() =>
            new()
            {
                Id = new Guid(),
                classifier_class = 5,
                classifier_score = 6.0,
                classifier_sign = 7,
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

        public static List<Biomarkers> GetFakeBiomarkersList()
        {
            var biomarkersList = new List<Biomarkers>
            {
                GetFakeBiomarkers(),
                GetFakeBiomarkers(),
                GetFakeBiomarkers(),
                GetFakeBiomarkers()
            };
            foreach (var biomarkers in biomarkersList)
            {
                biomarkers.Id = Guid.NewGuid();
            }
            return biomarkersList;
        }

        public static IEnumerable<ScoringHistoryDto> GetMockedScoringRequestHistory()
        {
            var scoringHistoryList = new List<ScoringHistoryDto>();

            for (int i = 0; i < 3; i++)
            {
                scoringHistoryList.Add(CreateMockedHistoryDto());
            }

            return scoringHistoryList;
        }

        private static ScoringHistoryDto CreateMockedHistoryDto() => new()
        {
            RequestId = Guid.NewGuid(),
            RequestTimeStamp = DateTimeOffset.Now,
            Score = 1.0f
        };

        internal static ScoringRequestDto CreateValidScoringRequestDto()
        {
            return new ScoringRequestDto()
            {
                FirstName = "Mock",
                LastName = "Mock",
                DateOfBirth = new DateTime(1990, 01, 01),
                Age = new BiomarkerValueDto<int>() { Value = 20, UnitType = "SI" },
                ACEInhibitor = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Alat = new BiomarkerValueDto<float>() { Value = 5, UnitType = "SI" },
                Albumin = new BiomarkerValueDto<float>() { Value = 20.0f, UnitType = "SI" },
                AlkalinePhosphatase = new BiomarkerValueDto<float>() { Value = 100, UnitType = "SI" },
                Betablocker = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Bilirubin = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                CaAntagonist = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                ChestPain = new BiomarkerValueDto<ChestPain>() { Value = ChestPain.Possible, UnitType = "SI" },
                Cholesterol = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                CholesterolLowering_Statin = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Diabetes = new BiomarkerValueDto<DiabetesStatus>() { Value = DiabetesStatus.Niddm, UnitType = "SI" },
                DiastolicBloodPressure = new BiomarkerValueDto<int>() { Value = 30, UnitType = "SI" },
                Diuretic = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                GlucoseFasting = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Hdl = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Height = new BiomarkerValueDto<int>() { Value = 180, UnitType = "SI" },
                HsTroponinT = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Ldl = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Leukocytes = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Mchc = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                NicotineConsumption = new BiomarkerValueDto<NicotineConsumption>() { Value = NicotineConsumption.StANc, UnitType = "SI" },
                OrganicNitrate = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                PancreaticAmylase = new BiomarkerValueDto<float>() { Value = 2, UnitType = "SI" },
                Protein = new BiomarkerValueDto<float>() { Value = 5.0f, UnitType = "SI" },
                RestingECG = new BiomarkerValueDto<RestingEcg>() { Value = RestingEcg.Yes, UnitType = "SI" },
                Sex = new BiomarkerValueDto<Sex>() { Value = Sex.Female, UnitType = "SI" },
                SystolicBloodPressure = new BiomarkerValueDto<int>() { Value = 50, UnitType = "SI" },
                TCAggregationInhibitor = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Urea = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                UricAcid = new BiomarkerValueDto<float>() { Value = 10.0f, UnitType = "SI" },
                Weight = new BiomarkerValueDto<int>() { Value = 100, UnitType = "SI" },
                clinical_setting = new BiomarkerValueDto<ClinicalSetting>() { Value = ClinicalSetting.PrimaryCare, UnitType = "SI" },
                prior_CAD = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
            };
        }
        internal static ScoringRequestDto CreateNotValidScoringRequestDto()
        {
            return new ScoringRequestDto()
            {
                FirstName = "Mock",
                LastName = "Mock",
                DateOfBirth = new DateTime(1990, 01, 01),
                Age = new BiomarkerValueDto<int>() { Value = 1, UnitType = "SI" },
                ACEInhibitor = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Alat = new BiomarkerValueDto<float>() { Value = 5, UnitType = "notValid" },
                Albumin = new BiomarkerValueDto<float>() { Value = 20.0f, UnitType = "SI" },
                AlkalinePhosphatase = new BiomarkerValueDto<float>() { Value = 100, UnitType = "SI" },
                Betablocker = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Bilirubin = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                CaAntagonist = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                ChestPain = new BiomarkerValueDto<ChestPain>() { Value = ChestPain.Possible, UnitType = "SI" },
                Cholesterol = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                CholesterolLowering_Statin = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Diabetes = new BiomarkerValueDto<DiabetesStatus>() { Value = DiabetesStatus.Niddm, UnitType = "SI" },
                DiastolicBloodPressure = new BiomarkerValueDto<int>() { Value = 30, UnitType = "SI" },
                Diuretic = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                GlucoseFasting = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Hdl = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Height = new BiomarkerValueDto<int>() { Value = 400, UnitType = "SI" },
                HsTroponinT = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Ldl = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Leukocytes = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                Mchc = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                NicotineConsumption = new BiomarkerValueDto<NicotineConsumption>() { Value = NicotineConsumption.StANc, UnitType = "SI" },
                OrganicNitrate = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                PancreaticAmylase = new BiomarkerValueDto<float>() { Value = 2, UnitType = "SI" },
                Protein = new BiomarkerValueDto<float>() { Value = 5.0f, UnitType = "SI" },
                RestingECG = new BiomarkerValueDto<RestingEcg>() { Value = RestingEcg.Yes, UnitType = "SI" },
                Sex = new BiomarkerValueDto<Sex>() { Value = Sex.Female, UnitType = "SI" },
                SystolicBloodPressure = new BiomarkerValueDto<int>() { Value = 50, UnitType = "notValid" },
                TCAggregationInhibitor = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
                Urea = new BiomarkerValueDto<float>() { Value = 2.0f, UnitType = "SI" },
                UricAcid = new BiomarkerValueDto<float>() { Value = 10.0f, UnitType = "SI" },
                Weight = new BiomarkerValueDto<int>() { Value = 13, UnitType = "SI" },
                clinical_setting = new BiomarkerValueDto<ClinicalSetting>() { Value = ClinicalSetting.PrimaryCare, UnitType = "SI" },
                prior_CAD = new BiomarkerValueDto<bool>() { Value = true, UnitType = "SI" },
            };
        }

        public static UserDto GetMockedUserDto()
        {
            return new UserDto()
            {
                Address = "Mock",
                City = "Mock",
                ClinicalSetting = ClinicalSetting.SecondaryCare,
                Country = "Mock",
                CountryCode = "Mock",
                Department = "Mock",
                EMailAdress = "Mock",
                FirstName = "Mock",
                Language = "Mock",
                PreferredLab = "Mock",
                ProfessionalSpecialisation = "Mock",
                Salutation = "Mock",
                Surname = "Mock",
                TelephoneNumber = "Mock",
                UnitLabValues = "Mock",
                Role = UserRole.MedicalDoctor
            };
        }

        public static User GetMockedUser()
        {
            return new User()
            {
                Address = "Mock",
                City = "Mock",
                ClinicalSetting = ClinicalSetting.SecondaryCare,
                Country = "Mock",
                CountryCode = "Mock",
                Department = "Mock",
                EMailAdress = "Mock",
                FirstName = "Mock",
                Language = "Mock",
                PreferredLab = "Mock",
                ProfessionalSpecialisation = "Mock",
                Salutation = "Mock",
                Surname = "Mock",
                TelephoneNumber = "Mock",
                UnitLabValues = "Mock",
                Role = UserRole.MedicalDoctor
            };
        }

        public static CreateUserDto GetMockedCreateUserDto()
        {
            return new CreateUserDto()
            {
                Address = "Mock",
                City = "Mock",
                ClinicalSetting = ClinicalSetting.SecondaryCare,
                Country = "Mock",
                CountryCode = "Mock",
                Department = "Mock",
                EMailAdress = "Mock",
                FirstName = "Mock",
                Language = "Mock",
                PreferredLab = "Mock",
                ProfessionalSpecialisation = "Mock",
                Salutation = "Mock",
                Surname = "Mock",
                TelephoneNumber = "Mock",
                UnitLabValues = "Mock",
            };
        }

        public static UserIdsRecord GetUserIdInformationRecord() => new()
        {
            TenantId = "MockedTenantId",
            UserId = "MockedUserId",
        };

        public static AccessRequestDto GetMockedAccessRequestDto()
        {
            return new AccessRequestDto()
            {
                EmailAddress = "MockedAddress",
                FirstName = "MockedFirstName",
                Surname = "MockedSurname",
                PhoneNumber = "MockedNumber"
            };
        }

        public static string GetHtmlBodyMock() => "New user tried to register their account: email: {{{EmailAddress}}} ({{{FirstName}}} {{{LastName}}}, {{{PhoneNumber}}})<br/><br/> With kind regards<br/> Exploris Health";

        public static ScoreSummary GetScoreSummaryMock()
        {
            var emptyArray = new string[] { };

            return new()
            {
                ScoreHeader = "ScoreHeader",
                Score = "ScoreValue",
                RiskHeader = "",
                Risk = "",
                RecommendationHeader = "",
                Recommendation = "",
                RecommendationExtended = "",
                RecommendationProbabilityHeader = "",
                WarningHeader = "",
                Warnings = Array.Empty<string>(),
                InfoText = "",
                Abbreviations = ImmutableDictionary<string,string>.Empty,
                CadDefinitionHeader = "",
                CadDefinition = "",
                FootnoteHeader = "",
                Footnote = "",
                IntendedUseHeader = "",
                IntendedUse = "",
                RecommendationTableHeader = "",
                RecommendationScoreRangeHeader = "",
                RecommendationCategories = new List<RecommendationCategory> { new() },
                Biomarkers = new List<BiomarkerSchemaDto> { new() },
            };
        }

        public static ScoringResponseSummary GetScoringResponseSummaryMock()
        {
            return new()
            {
                classifier_class = 1,
                classifier_sign = 1,
                classifier_score = 0.01,
                RequestId = Guid.NewGuid(),
                RiskValue = "RiskValue",
                Warnings = Array.Empty<string>(),
                RecommendationSummary = "RecommendationSummary",
                RecommendationLongText = "RecommendationLongText",
                Biomarkers = GetFakeBiomarkers()
            };
        }
    }
}