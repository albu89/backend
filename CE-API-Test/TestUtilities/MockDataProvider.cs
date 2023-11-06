﻿using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Records;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text.Json;
using CE_API_V2.Models.Mapping;
using static CE_API_V2.Models.Enum.PatientDataEnums;

namespace CE_API_Test.TestUtilities
{
    internal static class MockDataProvider
    {
        internal static ScoringRequest GetMockedScoringRequestDto()
        {
            var biomarkers = MockedInputPatientDataService.GetPatientBiomarkers("2");

            return biomarkers;
        }

        internal static string GetMockedSerializedScoringRequestDto()
        {
            var scoringRequestDtoMock = GetMockedScoringRequestDto();

            return JsonSerializer.Serialize(scoringRequestDtoMock);
        }

        internal static ScoringRequest CreateScoringRequest()
        {
            return new ScoringRequest()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                DateOfBirth = DateTime.UtcNow,
                Age = new BiomarkerValue<int>() { Value = 20, UnitType = "SI"},
                ACEInhibitor = new BiomarkerValue<bool>() { Value = true , UnitType = "SI"},
                Alat = new BiomarkerValue<float>() { Value = 2 , UnitType = "SI"},
                Albumin = new BiomarkerValue<float>() { Value = 2.0f , UnitType = "SI"},
                AlkalinePhosphatase = new BiomarkerValue<float>() { Value = 2 , UnitType = "SI"},
                Betablocker = new BiomarkerValue<bool>() { Value = true , UnitType = "SI"},
                Bilirubin = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                CaAntagonist = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Cholesterol = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                CholesterolLowering_Statin = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Diabetes = new BiomarkerValue<DiabetesStatus>() { Value = DiabetesStatus.Iddm, UnitType = "SI" },
                DiastolicBloodPressure = new BiomarkerValue<int>() { Value = 2, UnitType = "SI" },
                Diuretic = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                GlucoseFasting = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Hdl = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Height = new BiomarkerValue<int>() { Value = 180, UnitType = "SI" },
                HsTroponinT = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Ldl = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Leukocytes = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Mchc = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                NicotineConsumption = new BiomarkerValue<NicotineConsumption>() { Value = NicotineConsumption.StANc, UnitType = "SI" },
                OrganicNitrate = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                PancreaticAmylase = new BiomarkerValue<float>() { Value = 2, UnitType = "SI" },
                Protein = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                RestingECG = new BiomarkerValue<RestingEcg>() { Value = RestingEcg.Screening, UnitType = "SI" },
                Sex = new BiomarkerValue<Sex>() { Value = Sex.Female, UnitType = "SI" },
                SystolicBloodPressure = new BiomarkerValue<int>() { Value = 2, UnitType = "SI" },
                TCAggregationInhibitor = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                ChestPain = new BiomarkerValue<ChestPain>() { Value = ChestPain.Possible, UnitType = "SI" },
                Urea = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                UricAcid = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Weight = new BiomarkerValue<int>() { Value = 100, UnitType = "SI" },
                prior_CAD = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
            };
        }

        internal static ScoringResponseModel GetMockedScoringResponseForRequest(ScoringRequestModel requestModel)
        {
            var response = GetMockedScoringResponse();
            response.Request = requestModel;
            response.RequestId = requestModel.Id;
            return response;
        }

        internal static ScoringRequestModel GetMockedScoringRequest(string userId = "", string patientId = "", DateTimeOffset? CreatedOn = null)
        {
            return new ScoringRequestModel
            {
                Id = Guid.NewGuid(),
                PatientId = patientId.Equals(string.Empty) ? "PatientId" : patientId,
                UserId = userId.Equals(string.Empty) ? "UserId" : userId,
                Biomarkers = GetFakeBiomarkers(),
                CreatedOn = CreatedOn ?? DateTimeOffset.Now
            };
        }

        private static IEnumerable<Biomarkers> GetFakeBiomarkers()
        {
            var biomarkerList = new List<Biomarkers>
            {
                new Biomarkers
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
                    Qwave = RestingEcg.Screening,
                    Diuretic = false,
                    Hdl = 123,
                    HdlUnit = "SI",
                    Height = 176,
                    HeightUnit = "SI",
                    Ldl = 123,
                    LdlUnit = "SI",
                    Leukocyte = 123,
                    LeukocyteUnit = "SI",
                    Mchc = 123,
                    MchcUnit = "SI",
                    Sex = Sex.Male,
                    Weight = 90,
                    WeightUnit = "SI",
                    Protein = 123,
                    ProteinUnit = "SI",
                    Urea = 123,
                    UreaUnit = "SI",
                    Calciumant = false,
                    Chestpain = ChestPain.Possible,
                    Glucose = 123,
                    GlucoseUnit = "SI",
                    Amylasep = 123,
                    AmylasepUnit = "SI",
                    Uricacid = 123,
                    UricacidUnit = "SI",
                    Diastolicbp = 123,
                    DiastolicbpUnit = "SI",
                    Hstroponint = 123,
                    HstroponintUnit = "SI",
                    Systolicbp = 123,
                    SystolicbpUnit = "SI",
                    Nicotine = NicotineConsumption.StANc,
                    Statin = false,
                    Aceinhibitor = false,
                    Alkaline = 123,
                    AlkalineUnit = "SI",
                    Betablocker = false,
                    ClinicalSetting = ClinicalSetting.PrimaryCare,
                    Nitrate = false,
                    Tcagginhibitor = false,
                    PriorCAD = false
                }
            };
            return biomarkerList;
        }

        internal static string GetMockedSerializedResponse()
        {
            var mockedResponse = GetMockedScoringResponse();
            return JsonSerializer.Serialize(mockedResponse);
        }

        internal static string GetExpectedQueryString() =>
            "Datum=02/02/2000 00:00:00&Age=2&Sex_0_female_1male=2&Gr_sse=2&Gewicht=2&Thoraxschmerzen__0_keine_1_extr=2&Nicotin_0_nein_1_St__N__2_ja=2&Diabetes_0_no_1_NIDDM_2_IDDM=2&Statin_od_Chol_senker=2&Tc_Aggregation=2&ACE_od_ATII=2&CA_Antagonist=2&Betablocker=2&Diureticum=2&Nitrat_od_Dancor=2&BD_syst=2&BD_diast=2&q_Zacken_0_nein_1_ja=0&Pankreas_Amylase=2&Alk_Phase=2&Troponin=2&ALAT=2&Glucose=2&Bilirubin=2&Harnstoff=2&Harnsaure=2&Cholesterin_gesamt=2&HDL=2&LDL=2&Total_Proteine=2&Albumin=2&Leuko=2&MCHC__g_l_oder___=2&ASAT=2&Art__Hypertonie=2&CK=2&Chlorid=2&Dyspnoe=2&Gamma_GT=2&Hypercholesterin_mie=2&INR=2&Interne_Nummer=2&Kalium=2&Kreatinin=2&MCV__fl_=2&Natrium=2&OAK=2&Phosphat=2&Repolarisationsst_runge=2";

        internal static ScoringResponseModel GetMockedScoringResponse() =>
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
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                switch (propertyType?.Name ?? string.Empty)
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

        public static List<IEnumerable<Biomarkers>> GetFakeBiomarkersList()
        {
            var biomarkersList = new List<IEnumerable<Biomarkers>>
            {
                GetFakeBiomarkers(),
                GetFakeBiomarkers(),
                GetFakeBiomarkers(),
                GetFakeBiomarkers()
            };
            foreach (var biomarkers in biomarkersList.FirstOrDefault())
            {
                biomarkers.Id = Guid.NewGuid();
            }
            return biomarkersList;
        }

        public static IEnumerable<SimpleScore> GetMockedScoringRequestHistory()
        {
            var scoringHistoryList = new List<SimpleScore>();

            for (int i = 0; i < 3; i++)
            {
                scoringHistoryList.Add(CreateMockedHistoryDto());
            }

            return scoringHistoryList;
        }

        private static SimpleScore CreateMockedHistoryDto() => new()
        {
            RequestId = Guid.NewGuid(),
            RequestTimeStamp = DateTimeOffset.Now,
            Score = 1.0f,
            Risk = ">75%",
            RiskClass = 4
        };

        internal static ScoringRequest CreateValidScoringRequestDto()
        {
            return new ScoringRequest()
            {
                FirstName = "Mock",
                LastName = "Mock",
                DateOfBirth = new DateTime(1990, 01, 01),
                Age = new BiomarkerValue<int>() { Value = 20, UnitType="SI" },
                ACEInhibitor = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Alat = new BiomarkerValue<float>() { Value = 5, UnitType = "SI" },
                Albumin = new BiomarkerValue<float>() { Value = 20.0f, UnitType = "SI" },
                AlkalinePhosphatase = new BiomarkerValue<float>() { Value = 100, UnitType = "SI" },
                Betablocker = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Bilirubin = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                CaAntagonist = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                ChestPain = new BiomarkerValue<ChestPain>() { Value = ChestPain.Possible, UnitType = "SI" },
                Cholesterol = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                CholesterolLowering_Statin = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Diabetes = new BiomarkerValue<DiabetesStatus>() { Value = DiabetesStatus.Niddm, UnitType = "SI" },
                DiastolicBloodPressure = new BiomarkerValue<int>() { Value = 30, UnitType = "SI" },
                Diuretic = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                GlucoseFasting = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Hdl = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Height = new BiomarkerValue<int>() { Value = 180, UnitType = "SI" },
                HsTroponinT = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Ldl = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Leukocytes = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Mchc = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                NicotineConsumption = new BiomarkerValue<NicotineConsumption>() { Value = NicotineConsumption.StANc , UnitType = "SI" },
                OrganicNitrate = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                PancreaticAmylase = new BiomarkerValue<float>() { Value = 2, UnitType = "SI" },
                Protein = new BiomarkerValue<float>() { Value = 5.0f, UnitType = "SI" },
                RestingECG = new BiomarkerValue<RestingEcg>() { Value = RestingEcg.Yes, UnitType = "SI" },
                Sex = new BiomarkerValue<Sex>() { Value = Sex.Female, UnitType = "SI" },
                SystolicBloodPressure = new BiomarkerValue<int>() { Value = 50, UnitType = "SI" },
                TCAggregationInhibitor = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Urea = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                UricAcid = new BiomarkerValue<float>() { Value = 10.0f, UnitType = "SI" },
                Weight = new BiomarkerValue<int>() { Value = 100, UnitType = "SI" },
                prior_CAD = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
            };
        }
        internal static ScoringRequest CreateInvalidScoringRequestDto()
        {
            return new ScoringRequest()
            {
                FirstName = "Mock",
                LastName = "Mock",
                DateOfBirth = new DateTime(1990, 01, 01),
                Age = new BiomarkerValue<int>() { Value = 1, UnitType = "Conventional" },
                ACEInhibitor = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Alat = new BiomarkerValue<float>() { Value = 5, UnitType = "notValid" },
                Albumin = new BiomarkerValue<float>() { Value = 20.0f, UnitType = "SI" },
                AlkalinePhosphatase = new BiomarkerValue<float>() { Value = 100, UnitType = "SI" },
                Betablocker = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Bilirubin = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                CaAntagonist = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                ChestPain = new BiomarkerValue<ChestPain>() { Value = ChestPain.Possible, UnitType = "SI" },
                Cholesterol = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                CholesterolLowering_Statin = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Diabetes = new BiomarkerValue<DiabetesStatus>() { Value = DiabetesStatus.Niddm, UnitType = "SI" },
                DiastolicBloodPressure = new BiomarkerValue<int>() { Value = 30, UnitType = "SI" },
                Diuretic = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                GlucoseFasting = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Hdl = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Height = new BiomarkerValue<int>() { Value = 400, UnitType = "SI" },
                HsTroponinT = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Ldl = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Leukocytes = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                Mchc = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "SI" },
                NicotineConsumption = new BiomarkerValue<NicotineConsumption>() { Value = NicotineConsumption.StANc, UnitType = "SI" },
                OrganicNitrate = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                PancreaticAmylase = new BiomarkerValue<float>() { Value = 2, UnitType = "SI" },
                Protein = new BiomarkerValue<float>() { Value = 5.0f, UnitType = "Conventional" },
                RestingECG = new BiomarkerValue<RestingEcg>() { Value = RestingEcg.Yes, UnitType = "SI" },
                Sex = new BiomarkerValue<Sex>() { Value = Sex.Female, UnitType = "Conventional" },
                SystolicBloodPressure = new BiomarkerValue<int>() { Value = 50, UnitType = "notValid" },
                TCAggregationInhibitor = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
                Urea = new BiomarkerValue<float>() { Value = 2.0f, UnitType = "Conventional" },
                UricAcid = new BiomarkerValue<float>() { Value = 10.0f, UnitType = "Conventional" },
                Weight = new BiomarkerValue<int>() { Value = 13, UnitType = "Conventional" },
                prior_CAD = new BiomarkerValue<bool>() { Value = true, UnitType = "SI" },
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
                EMailAddress = "Mock",
                FirstName = "Mock",
                Language = "Mock",
                PreferredLab = "Mock",
                ProfessionalSpecialisation = "Mock",
                Salutation = "Mock",
                Surname = "Mock",
                TelephoneNumber = "Mock",
                UnitLabValues = "Mock", 
                Role = UserRole.User
            };
        }
        
        public static UserModel GetMockedUserModel()
        {
            return new UserModel()
            {
                ZipCode = "12345",
                Title = "Dr.",
                Address = "Mock",
                City = "Mock",
                ClinicalSetting = ClinicalSetting.SecondaryCare,
                Country = "Mock",
                CountryCode = "Mock",
                Department = "Mock",
                EMailAddress = "Mock",
                FirstName = "Mock",
                Language = "Mock",
                PreferredLab = "Mock",
                ProfessionalSpecialisation = "Mock",
                Salutation = "Mock",
                Surname = "Mock",
                TelephoneNumber = "Mock",
                UnitLabValues = "Mock",
                Role = UserRole.User,
                UserId = "MockedUserId",
                TenantID = "MockedTenantId",
                BiomarkerOrders = new Collection<BiomarkerOrderModel>
                {
                    new() { OrderNumber = 1, BiomarkerId = "first", PreferredUnit = "unit", User = null, UserId = "id" },
                    new() { OrderNumber = 2, BiomarkerId = "second", PreferredUnit = "unit", User = null, UserId = "id" }
                }
        };
        }

        public static UpdateUser GetMockedUpdateUserDto()
        {
            return new UpdateUser()
            {
                Address = "Mock",
                City = "Mock",
                Country = "Mock",
                CountryCode = "Mock",
                EMailAddress = "Mock",
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
        
        public static CreateUser GetMockedCreateUser()
        {
            return new CreateUser()
            {
                Address = "Mock",
                City = "Mock",
                ClinicalSetting = ClinicalSetting.SecondaryCare,
                Country = "Mock",
                CountryCode = "Mock",
                Department = "Mock",
                EMailAddress = "Mock",
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
        
        public static AccessRequest GetMockedAccessRequestDto()
        {
            return new AccessRequest()
            {
                EmailAddress = "MockedAddress",
                FirstName = "MockedFirstName",
                Surname = "MockedSurname",
                PhoneNumber = "MockedNumber"
            };
        }

        public static string GetRequestHtmlBodyMock() => "New user tried to register their account: email: {{{EmailAddress}}} ({{{FirstName}}} {{{LastName}}}, {{{PhoneNumber}}})<br/><br/> With kind regards<br/> Exploris Health";
       
        public static string GetActivateUserHtmlBodyMock() => "New user has registered their account: email: {{{Id}}} ({{{FirstName}}} {{{LastName}}})<br/><br/> With kind regards<br/> Exploris Health";

        public static ScoreSchema GetScoreSummaryMock()
        {
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
                Biomarkers = new CadRequestSchema(),
            };
        }

        public static ScoringResponse GetScoringResponseSummaryMock()
        {
            return new()
            {
                classifier_score = 0.01,
                RequestId = Guid.NewGuid(),
                RiskValue = "RiskValue",
                Warnings = Array.Empty<string>(),
                RecommendationSummary = "RecommendationSummary",
                RecommendationLongText = "RecommendationLongText",
                Biomarkers = GetFakeBiomarkers().FirstOrDefault()
            };
        }


        public static BiomarkerOrder GetMockedOrder()
        {
            var order = new BiomarkerOrder()
            {
                Alat = new BiomarkerOrderEntry() { OrderNumber = 1, PreferredUnit = "SI" },
                Age = new BiomarkerOrderEntry() { OrderNumber = 2, PreferredUnit = "SI" },
                Albumin = new BiomarkerOrderEntry() { OrderNumber = 3, PreferredUnit = "SI" },
                Betablocker = new BiomarkerOrderEntry() { OrderNumber = 4, PreferredUnit = "SI" },
                Bilirubin = new BiomarkerOrderEntry() { OrderNumber = 5, PreferredUnit = "SI" },
                Cholesterol = new BiomarkerOrderEntry() { OrderNumber = 6, PreferredUnit = "SI" },
                Diabetes = new BiomarkerOrderEntry() { OrderNumber = 7, PreferredUnit = "SI" },
                Diuretic = new BiomarkerOrderEntry() { OrderNumber = 8, PreferredUnit = "SI" },
                Hdl = new BiomarkerOrderEntry() { OrderNumber = 9, PreferredUnit = "SI" },
                Height = new BiomarkerOrderEntry() { OrderNumber = 10, PreferredUnit = "SI" },
                Ldl = new BiomarkerOrderEntry() { OrderNumber = 11, PreferredUnit = "SI" },
                Leukocytes = new BiomarkerOrderEntry() { OrderNumber = 12, PreferredUnit = "SI" },
                Mchc = new BiomarkerOrderEntry() { OrderNumber = 13, PreferredUnit = "SI" },
                Protein = new BiomarkerOrderEntry() { OrderNumber = 14, PreferredUnit = "SI" },
                Sex = new BiomarkerOrderEntry() { OrderNumber = 15, PreferredUnit = "SI" },
                Urea = new BiomarkerOrderEntry() { OrderNumber = 16, PreferredUnit = "SI" },
                Weight = new BiomarkerOrderEntry() { OrderNumber = 17, PreferredUnit = "SI" },
                AlkalinePhosphatase = new BiomarkerOrderEntry() { OrderNumber = 18, PreferredUnit = "SI" },
                CaAntagonist = new BiomarkerOrderEntry() { OrderNumber = 19, PreferredUnit = "SI" },
                ChestPain = new BiomarkerOrderEntry() { OrderNumber = 20, PreferredUnit = "SI" },
                GlucoseFasting = new BiomarkerOrderEntry() { OrderNumber = 21, PreferredUnit = "SI" },
                NicotineConsumption = new BiomarkerOrderEntry() { OrderNumber = 22, PreferredUnit = "SI" },
                OrganicNitrate = new BiomarkerOrderEntry() { OrderNumber = 23, PreferredUnit = "SI" },
                PancreaticAmylase = new BiomarkerOrderEntry() { OrderNumber = 24, PreferredUnit = "SI" },
                UricAcid = new BiomarkerOrderEntry() { OrderNumber = 25, PreferredUnit = "SI" },
                CholesterolLowering_Statin = new BiomarkerOrderEntry() { OrderNumber = 26, PreferredUnit = "SI" },
                DiastolicBloodPressure = new BiomarkerOrderEntry() { OrderNumber = 27, PreferredUnit = "SI" },
                HsTroponinT = new BiomarkerOrderEntry() { OrderNumber = 28, PreferredUnit = "SI" },
                prior_CAD = new BiomarkerOrderEntry() { OrderNumber = 29, PreferredUnit = "SI" },
                SystolicBloodPressure = new BiomarkerOrderEntry() { OrderNumber = 30, PreferredUnit = "SI" },
                ACEInhibitor = new BiomarkerOrderEntry() { OrderNumber = 31, PreferredUnit = "SI" },
                RestingECG = new BiomarkerOrderEntry() { OrderNumber = 32, PreferredUnit = "SI" },
                TCAggregationInhibitor = new BiomarkerOrderEntry() { OrderNumber = 33, PreferredUnit = "SI" },
            };
            return order;
        }

        public static List<BiomarkerOrderModel> GetMockedOrderModels()
        {
            return ManualMapper.ToBiomarkerOrderModels(GetMockedOrder());
            
        }

        public static CreateCountry GetMockedCountry() => new()
        {
            Name = "Country",
            ContactEmail = "ContactEmail",
        };
        
        public static CountryModel GetMockedCountryModel() => new()
        {
            Name = "Country",
            ContactEmail = "ContactEmail",
            Id = Guid.NewGuid()
        };

        public static CreateOrganization GetMockedCreateOrganization() => new()
        {
            Name = "Organization",
            ContactEmail = "ContactEmail",
            TenantId = Guid.NewGuid()
        };
        
        public static OrganizationModel GetMockedOrganizationModel() => new()
        {
            Name = "Organization",
            ContactEmail = "ContactEmail",
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid()
        };
    }
}