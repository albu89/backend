using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;

namespace CE_API_V2.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ScoringRequestModel, SimpleScore>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.LatestResponse.classifier_score))
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestTimeStamp, opt => opt.MapFrom(src => src.LatestBiomarkers.CreatedOn < src.LatestBiomarkersDraft.CreatedOn ? src.LatestBiomarkersDraft.CreatedOn : src.LatestBiomarkers.CreatedOn))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.LatestResponse.RiskClass))
                .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => src.LatestResponse.Risk))
                .ForMember(dest => dest.IsDraft, opt => opt.MapFrom(src => src.LatestBiomarkers.Response == null));

            CreateMap<ScoringRequest, ScoringRequestModel>();
            CreateMap<ScoringRequestDraft, ScoringRequestModelDraft>();
            CreateMap<ScoringRequestDraft, BiomarkersDraft>()
                .ForMember(dest => dest.Glucose, opt => opt.MapFrom(src => src.GlucoseFasting.Value))
                .ForMember(dest => dest.Nicotine, opt => opt.MapFrom(src => src.NicotineConsumption.Value))
                .ForMember(dest => dest.Aceinhibitor, opt => opt.MapFrom(src => src.ACEInhibitor.Value))
                .ForMember(dest => dest.Alkaline, opt => opt.MapFrom(src => src.AlkalinePhosphatase.Value))
                .ForMember(dest => dest.Tcagginhibitor, opt => opt.MapFrom(src => src.TCAggregationInhibitor.Value))
                .ForMember(dest => dest.PriorCAD, opt => opt.MapFrom(src => src.prior_CAD.Value))
                .ForMember(dest => dest.Statin, opt => opt.MapFrom(src => src.CholesterolLowering_Statin.Value))
                .ForMember(dest => dest.Diastolicbp, opt => opt.MapFrom(src => src.DiastolicBloodPressure.Value))
                .ForMember(dest => dest.Systolicbp, opt => opt.MapFrom(src => src.SystolicBloodPressure.Value))
                .ForMember(dest => dest.Leukocyte, opt => opt.MapFrom(src => src.Leukocytes.Value))
                .ForMember(dest => dest.Qwave, opt => opt.MapFrom(src => src.RestingECG.Value))
                .ForMember(dest => dest.Nitrate, opt => opt.MapFrom(src => src.OrganicNitrate.Value))
                .ForMember(dest => dest.Amylasep, opt => opt.MapFrom(src => src.PancreaticAmylase.Value))
                .ForMember(dest => dest.Calciumant, opt => opt.MapFrom(src => src.CaAntagonist.Value))

                .ForMember(dest => dest.AgeUnit, opt => opt.MapFrom(src => src.Age.UnitType))
                .ForMember(dest => dest.HdlUnit, opt => opt.MapFrom(src => src.Hdl.UnitType))
                .ForMember(dest => dest.LdlUnit, opt => opt.MapFrom(src => src.Ldl.UnitType))
                .ForMember(dest => dest.AlatUnit, opt => opt.MapFrom(src => src.Alat.UnitType))
                .ForMember(dest => dest.AlbuminUnit, opt => opt.MapFrom(src => src.Albumin.UnitType))
                .ForMember(dest => dest.UreaUnit, opt => opt.MapFrom(src => src.Urea.UnitType))
                .ForMember(dest => dest.MchcUnit, opt => opt.MapFrom(src => src.Mchc.UnitType))
                .ForMember(dest => dest.AlkalineUnit, opt => opt.MapFrom(src => src.AlkalinePhosphatase.UnitType))
                .ForMember(dest => dest.BilirubinUnit, opt => opt.MapFrom(src => src.Bilirubin.UnitType))
                .ForMember(dest => dest.CholesterolUnit, opt => opt.MapFrom(src => src.Cholesterol.UnitType))
                .ForMember(dest => dest.AmylasepUnit, opt => opt.MapFrom(src => src.PancreaticAmylase.UnitType))
                .ForMember(dest => dest.HeightUnit, opt => opt.MapFrom(src => src.Height.UnitType))
                .ForMember(dest => dest.WeightUnit, opt => opt.MapFrom(src => src.Weight.UnitType))
                .ForMember(dest => dest.GlucoseUnit, opt => opt.MapFrom(src => src.GlucoseFasting.UnitType))
                .ForMember(dest => dest.LeukocyteUnit, opt => opt.MapFrom(src => src.Leukocytes.UnitType))
                .ForMember(dest => dest.UricacidUnit, opt => opt.MapFrom(src => src.UricAcid.UnitType))
                .ForMember(dest => dest.DiastolicbpUnit, opt => opt.MapFrom(src => src.DiastolicBloodPressure.UnitType))
                .ForMember(dest => dest.SystolicbpUnit, opt => opt.MapFrom(src => src.SystolicBloodPressure.UnitType))
                .ForMember(dest => dest.HstroponintUnit, opt => opt.MapFrom(src => src.HsTroponinT.UnitType))
                .ForMember(dest => dest.ProteinUnit, opt => opt.MapFrom(src => src.Protein.UnitType))

                .ForMember(dest => dest.PriorCADDisplayValue, opt => opt.MapFrom(src => src.prior_CAD.DisplayValue))
                .ForMember(dest => dest.SexDisplayValue, opt => opt.MapFrom(src => src.Sex.DisplayValue))
                .ForMember(dest => dest.ChestpainDisplayValue, opt => opt.MapFrom(src => src.ChestPain.DisplayValue))
                .ForMember(dest => dest.NicotineDisplayValue, opt => opt.MapFrom(src => src.NicotineConsumption.DisplayValue))
                .ForMember(dest => dest.DiabetesDisplayValue, opt => opt.MapFrom(src => src.Diabetes.DisplayValue))
                .ForMember(dest => dest.StatinDisplayValue, opt => opt.MapFrom(src => src.CholesterolLowering_Statin.DisplayValue))
                .ForMember(dest => dest.TcagginhibitorDisplayValue, opt => opt.MapFrom(src => src.TCAggregationInhibitor.DisplayValue))
                .ForMember(dest => dest.AceinhibitorDisplayValue, opt => opt.MapFrom(src => src.ACEInhibitor.DisplayValue))
                .ForMember(dest => dest.CalciumantDisplayValue, opt => opt.MapFrom(src => src.CaAntagonist.DisplayValue))
                .ForMember(dest => dest.BetablockerDisplayValue, opt => opt.MapFrom(src => src.Betablocker.DisplayValue))
                .ForMember(dest => dest.DiureticDisplayValue, opt => opt.MapFrom(src => src.Diuretic.DisplayValue))
                .ForMember(dest => dest.NitrateDisplayValue, opt => opt.MapFrom(src => src.OrganicNitrate.DisplayValue))
                .ForMember(dest => dest.QwaveDisplayValue, opt => opt.MapFrom(src => src.RestingECG.DisplayValue))

                .ForMember(dest => dest.AgeDisplayValue, opt => opt.MapFrom(src => src.Age.DisplayValue))
                .ForMember(dest => dest.HdlDisplayValue, opt => opt.MapFrom(src => src.Hdl.DisplayValue))
                .ForMember(dest => dest.LdlDisplayValue, opt => opt.MapFrom(src => src.Ldl.DisplayValue))
                .ForMember(dest => dest.AlatDisplayValue, opt => opt.MapFrom(src => src.Alat.DisplayValue))
                .ForMember(dest => dest.AlbuminDisplayValue, opt => opt.MapFrom(src => src.Albumin.DisplayValue))
                .ForMember(dest => dest.UreaDisplayValue, opt => opt.MapFrom(src => src.Urea.DisplayValue))
                .ForMember(dest => dest.MchcDisplayValue, opt => opt.MapFrom(src => src.Mchc.DisplayValue))
                .ForMember(dest => dest.AlkalineDisplayValue, opt => opt.MapFrom(src => src.AlkalinePhosphatase.DisplayValue))
                .ForMember(dest => dest.BilirubinDisplayValue, opt => opt.MapFrom(src => src.Bilirubin.DisplayValue))
                .ForMember(dest => dest.CholesterolDisplayValue, opt => opt.MapFrom(src => src.Cholesterol.DisplayValue))
                .ForMember(dest => dest.AmylasepDisplayValue, opt => opt.MapFrom(src => src.PancreaticAmylase.DisplayValue))
                .ForMember(dest => dest.HeightDisplayValue, opt => opt.MapFrom(src => src.Height.DisplayValue))
                .ForMember(dest => dest.WeightDisplayValue, opt => opt.MapFrom(src => src.Weight.DisplayValue))
                .ForMember(dest => dest.GlucoseDisplayValue, opt => opt.MapFrom(src => src.GlucoseFasting.DisplayValue))
                .ForMember(dest => dest.LeukocyteDisplayValue, opt => opt.MapFrom(src => src.Leukocytes.DisplayValue))
                .ForMember(dest => dest.UricacidDisplayValue, opt => opt.MapFrom(src => src.UricAcid.DisplayValue))
                .ForMember(dest => dest.DiastolicbpDisplayValue, opt => opt.MapFrom(src => src.DiastolicBloodPressure.DisplayValue))
                .ForMember(dest => dest.SystolicbpDisplayValue, opt => opt.MapFrom(src => src.SystolicBloodPressure.DisplayValue))
                .ForMember(dest => dest.HstroponintDisplayValue, opt => opt.MapFrom(src => src.HsTroponinT.DisplayValue))
                .ForMember(dest => dest.ProteinDisplayValue, opt => opt.MapFrom(src => src.Protein.DisplayValue))
                .ForMember(dest => dest.ClinicalSettingDisplayValue, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.ClinicalSettingUnit, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.ClinicalSetting, opt => opt.MapFrom(src => string.Empty))
                ;

            CreateMap<ScoringRequest, Biomarkers>()
                .ForMember(dest => dest.Glucose, opt => opt.MapFrom(src => src.GlucoseFasting))
                .ForMember(dest => dest.Nicotine, opt => opt.MapFrom(src => src.NicotineConsumption))
                .ForMember(dest => dest.Aceinhibitor, opt => opt.MapFrom(src => src.ACEInhibitor))
                .ForMember(dest => dest.Alkaline, opt => opt.MapFrom(src => src.AlkalinePhosphatase))
                .ForMember(dest => dest.Tcagginhibitor, opt => opt.MapFrom(src => src.TCAggregationInhibitor))
                .ForMember(dest => dest.PriorCAD, opt => opt.MapFrom(src => src.prior_CAD))
                .ForMember(dest => dest.Statin, opt => opt.MapFrom(src => src.CholesterolLowering_Statin))
                .ForMember(dest => dest.Diastolicbp, opt => opt.MapFrom(src => src.DiastolicBloodPressure))
                .ForMember(dest => dest.Systolicbp, opt => opt.MapFrom(src => src.SystolicBloodPressure))
                .ForMember(dest => dest.Leukocyte, opt => opt.MapFrom(src => src.Leukocytes))
                .ForMember(dest => dest.Qwave, opt => opt.MapFrom(src => src.RestingECG))
                .ForMember(dest => dest.Nitrate, opt => opt.MapFrom(src => src.OrganicNitrate))
                .ForMember(dest => dest.Amylasep, opt => opt.MapFrom(src => src.PancreaticAmylase))
                .ForMember(dest => dest.Calciumant, opt => opt.MapFrom(src => src.CaAntagonist))

                .ForMember(dest => dest.AgeUnit, opt => opt.MapFrom(src => src.Age.UnitType))
                .ForMember(dest => dest.HdlUnit, opt => opt.MapFrom(src => src.Hdl.UnitType))
                .ForMember(dest => dest.LdlUnit, opt => opt.MapFrom(src => src.Ldl.UnitType))
                .ForMember(dest => dest.AlatUnit, opt => opt.MapFrom(src => src.Alat.UnitType))
                .ForMember(dest => dest.AlbuminUnit, opt => opt.MapFrom(src => src.Albumin.UnitType))
                .ForMember(dest => dest.UreaUnit, opt => opt.MapFrom(src => src.Urea.UnitType))
                .ForMember(dest => dest.MchcUnit, opt => opt.MapFrom(src => src.Mchc.UnitType))
                .ForMember(dest => dest.AlkalineUnit, opt => opt.MapFrom(src => src.AlkalinePhosphatase.UnitType))
                .ForMember(dest => dest.BilirubinUnit, opt => opt.MapFrom(src => src.Bilirubin.UnitType))
                .ForMember(dest => dest.CholesterolUnit, opt => opt.MapFrom(src => src.Cholesterol.UnitType))
                .ForMember(dest => dest.AmylasepUnit, opt => opt.MapFrom(src => src.PancreaticAmylase.UnitType))
                .ForMember(dest => dest.HeightUnit, opt => opt.MapFrom(src => src.Height.UnitType))
                .ForMember(dest => dest.WeightUnit, opt => opt.MapFrom(src => src.Weight.UnitType))
                .ForMember(dest => dest.GlucoseUnit, opt => opt.MapFrom(src => src.GlucoseFasting.UnitType))
                .ForMember(dest => dest.LeukocyteUnit, opt => opt.MapFrom(src => src.Leukocytes.UnitType))
                .ForMember(dest => dest.UricacidUnit, opt => opt.MapFrom(src => src.UricAcid.UnitType))
                .ForMember(dest => dest.DiastolicbpUnit, opt => opt.MapFrom(src => src.DiastolicBloodPressure.UnitType))
                .ForMember(dest => dest.SystolicbpUnit, opt => opt.MapFrom(src => src.SystolicBloodPressure.UnitType))
                .ForMember(dest => dest.HstroponintUnit, opt => opt.MapFrom(src => src.HsTroponinT.UnitType))
                .ForMember(dest => dest.ProteinUnit, opt => opt.MapFrom(src => src.Protein.UnitType))

                .ForMember(dest => dest.PriorCADDisplayValue, opt => opt.MapFrom(src => src.prior_CAD.DisplayValue))
                .ForMember(dest => dest.SexDisplayValue, opt => opt.MapFrom(src => src.Sex.DisplayValue))
                .ForMember(dest => dest.ChestpainDisplayValue, opt => opt.MapFrom(src => src.ChestPain.DisplayValue))
                .ForMember(dest => dest.NicotineDisplayValue, opt => opt.MapFrom(src => src.NicotineConsumption.DisplayValue))
                .ForMember(dest => dest.DiabetesDisplayValue, opt => opt.MapFrom(src => src.Diabetes.DisplayValue))
                .ForMember(dest => dest.StatinDisplayValue, opt => opt.MapFrom(src => src.CholesterolLowering_Statin.DisplayValue))
                .ForMember(dest => dest.TcagginhibitorDisplayValue, opt => opt.MapFrom(src => src.TCAggregationInhibitor.DisplayValue))
                .ForMember(dest => dest.AceinhibitorDisplayValue, opt => opt.MapFrom(src => src.ACEInhibitor.DisplayValue))
                .ForMember(dest => dest.CalciumantDisplayValue, opt => opt.MapFrom(src => src.CaAntagonist.DisplayValue))
                .ForMember(dest => dest.BetablockerDisplayValue, opt => opt.MapFrom(src => src.Betablocker.DisplayValue))
                .ForMember(dest => dest.DiureticDisplayValue, opt => opt.MapFrom(src => src.Diuretic.DisplayValue))
                .ForMember(dest => dest.NitrateDisplayValue, opt => opt.MapFrom(src => src.OrganicNitrate.DisplayValue))
                .ForMember(dest => dest.QwaveDisplayValue, opt => opt.MapFrom(src => src.RestingECG.DisplayValue))

                .ForMember(dest => dest.AgeDisplayValue, opt => opt.MapFrom(src => src.Age.DisplayValue))
                .ForMember(dest => dest.HdlDisplayValue, opt => opt.MapFrom(src => src.Hdl.DisplayValue))
                .ForMember(dest => dest.LdlDisplayValue, opt => opt.MapFrom(src => src.Ldl.DisplayValue))
                .ForMember(dest => dest.AlatDisplayValue, opt => opt.MapFrom(src => src.Alat.DisplayValue))
                .ForMember(dest => dest.AlbuminDisplayValue, opt => opt.MapFrom(src => src.Albumin.DisplayValue))
                .ForMember(dest => dest.UreaDisplayValue, opt => opt.MapFrom(src => src.Urea.DisplayValue))
                .ForMember(dest => dest.MchcDisplayValue, opt => opt.MapFrom(src => src.Mchc.DisplayValue))
                .ForMember(dest => dest.AlkalineDisplayValue, opt => opt.MapFrom(src => src.AlkalinePhosphatase.DisplayValue))
                .ForMember(dest => dest.BilirubinDisplayValue, opt => opt.MapFrom(src => src.Bilirubin.DisplayValue))
                .ForMember(dest => dest.CholesterolDisplayValue, opt => opt.MapFrom(src => src.Cholesterol.DisplayValue))
                .ForMember(dest => dest.AmylasepDisplayValue, opt => opt.MapFrom(src => src.PancreaticAmylase.DisplayValue))
                .ForMember(dest => dest.HeightDisplayValue, opt => opt.MapFrom(src => src.Height.DisplayValue))
                .ForMember(dest => dest.WeightDisplayValue, opt => opt.MapFrom(src => src.Weight.DisplayValue))
                .ForMember(dest => dest.GlucoseDisplayValue, opt => opt.MapFrom(src => src.GlucoseFasting.DisplayValue))
                .ForMember(dest => dest.LeukocyteDisplayValue, opt => opt.MapFrom(src => src.Leukocytes.DisplayValue))
                .ForMember(dest => dest.UricacidDisplayValue, opt => opt.MapFrom(src => src.UricAcid.DisplayValue))
                .ForMember(dest => dest.DiastolicbpDisplayValue, opt => opt.MapFrom(src => src.DiastolicBloodPressure.DisplayValue))
                .ForMember(dest => dest.SystolicbpDisplayValue, opt => opt.MapFrom(src => src.SystolicBloodPressure.DisplayValue))
                .ForMember(dest => dest.HstroponintDisplayValue, opt => opt.MapFrom(src => src.HsTroponinT.DisplayValue))
                .ForMember(dest => dest.ProteinDisplayValue, opt => opt.MapFrom(src => src.Protein.DisplayValue))
                .ForMember(dest => dest.ClinicalSettingDisplayValue, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.ClinicalSettingUnit, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.ClinicalSetting, opt => opt.MapFrom(src => string.Empty));

            CreateMap<CreateUser, UserModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.TenantID, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.BiomarkerOrders, opt => opt.Ignore());

            CreateMap<UpdateUser, UserModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.TenantID, opt => opt.MapFrom(_ => string.Empty));

            CreateMap<UserModel, User>();

            CreateMap(typeof(BiomarkerValue<string>), typeof(string))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<string>));
            CreateMap(typeof(BiomarkerValue<int>), typeof(int))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<int>));
            CreateMap(typeof(BiomarkerValue<float>), typeof(float))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<float>));
            CreateMap(typeof(BiomarkerValue<bool>), typeof(bool))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<bool>));
            CreateMap(typeof(BiomarkerValue<PatientDataEnums.Sex>), typeof(PatientDataEnums.Sex))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.Sex>));
            CreateMap(typeof(BiomarkerValue<PatientDataEnums.NicotineConsumption>),
                    typeof(PatientDataEnums.NicotineConsumption))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.NicotineConsumption>));
            CreateMap(typeof(BiomarkerValue<PatientDataEnums.ChestPain>), typeof(PatientDataEnums.ChestPain))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.ChestPain>));
            CreateMap(typeof(BiomarkerValue<PatientDataEnums.ClinicalSetting>),
                    typeof(PatientDataEnums.ClinicalSetting))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.ClinicalSetting>));
            CreateMap(typeof(BiomarkerValue<PatientDataEnums.DiabetesStatus>),
                    typeof(PatientDataEnums.DiabetesStatus))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.DiabetesStatus>));
            CreateMap(typeof(BiomarkerValue<PatientDataEnums.RestingEcg>), typeof(PatientDataEnums.RestingEcg))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.RestingEcg>));
            
            CreateMap<ScoringResponse, ScoringResponseModel>()
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.classifier_score))
                .ForMember(dest => dest.Recommendation, opt => opt.MapFrom(src => src.RecommendationSummary))
                .ForMember(dest => dest.RecommendationLong, opt => opt.MapFrom(src => src.RecommendationLongText))
                .ForMember(dest => dest.Warnings, opt => opt.MapFrom(src => src.Warnings))
                .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => src.RiskValue))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.RiskClass))
                ;

            CreateMap<ScoringResponseModel, ScoringResponse>()
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore())
                .ForMember(dest => dest.RiskValue, opt => opt.Ignore())
                .ForMember(dest => dest.Warnings, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationSummary, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationLongText, opt => opt.Ignore());

            CreateMap<ScoringSchema, ScoreSchema>()
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationCategories, opt => opt.Ignore());

            CreateMap<RecommendationCategory, ScoringResponse>()
                .ForMember(dest => dest.RecommendationSummary, opt => opt.MapFrom(src => src.ShortText))
                .ForMember(dest => dest.RecommendationLongText, opt => opt.MapFrom(src => src.LongText))
                .ForMember(dest => dest.RiskValue, opt => opt.MapFrom(src => src.RiskValue))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Warnings, opt => opt.Ignore())
                .ForMember(dest => dest.classifier_score, opt => opt.Ignore());

            CreateMap<UserModel, User>()
                .ForMember(dest => dest.BiomarkerOrders, opt => opt.Ignore());

            CreateMap<User, UserModel>()
                .ForMember(dest => dest.BiomarkerOrders, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.ClinicalSetting, opt => opt.Ignore())
                .ForMember(src => src.BiomarkerOrders, opt => opt.Ignore());

            CreateMap<RecommendationCategoryStaticPart, RecommendationCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LongText, opt => opt.MapFrom(src => src.LowerLimit))
                .ForMember(dest => dest.UpperLimit, opt => opt.MapFrom(src => src.UpperLimit))
                .ForMember(dest => dest.RiskValue, opt => opt.MapFrom(src => src.RiskValue));

            CreateMap<RecommendationCategoryLocalizedPart, RecommendationCategory>()
                .ForMember(dest => dest.LongText, opt => opt.MapFrom(src => src.LongText))
                .ForMember(dest => dest.ShortText, opt => opt.MapFrom(src => src.ShortText));

            CreateMap<CreateCountry, CountryModel>();

            CreateMap<CountryModel, CreateCountry>();

            CreateMap<CountryModel, Country>();

            CreateMap<CreateOrganization, OrganizationModel>();

            CreateMap<OrganizationModel, OrganizationModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<OrganizationModel, Organization>();
            CreateMap<OrganizationModel, CreateOrganization>();

            CreateMap<BiomarkersDraft, BiomarkersDraft>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                ;

            CreateMap<UserInputFormSchemaHeaders, UserInputFormSchema>()
                .ForMember(dest => dest.AddressHeader, opt => opt.MapFrom(src => src.AddressHeader))
                .ForMember(dest => dest.Billing, opt => opt.MapFrom(src => src.Billing))
                .ForMember(dest => dest.CityHeader, opt => opt.MapFrom(src => src.CityHeader))
                .ForMember(dest => dest.ClinicalSettingHeader, opt => opt.MapFrom(src => src.ClinicalSettingHeader))
                .ForMember(dest => dest.CountryCodeHeader, opt => opt.MapFrom(src => src.CountryCodeHeader))
                .ForMember(dest => dest.CountryHeader, opt => opt.MapFrom(src => src.CountryHeader))
                .ForMember(dest => dest.DepartmentHeader, opt => opt.MapFrom(src => src.DepartmentHeader))
                .ForMember(dest => dest.EMailAddressHeader, opt => opt.MapFrom(src => src.EMailAddressHeader))
                .ForMember(dest => dest.FirstNameHeader, opt => opt.MapFrom(src => src.FirstNameHeader))
                .ForMember(dest => dest.IsActiveHeader, opt => opt.MapFrom(src => src.IsActiveHeader))
                .ForMember(dest => dest.IsSeparateBillingHeader, opt => opt.MapFrom(src => src.IsSeparateBillingHeader))
                .ForMember(dest => dest.ZipCodeHeader, opt => opt.MapFrom(src => src.ZipCodeHeader))
                .ForMember(dest => dest.UnitLabValuesHeader, opt => opt.MapFrom(src => src.UnitLabValuesHeader))
                .ForMember(dest => dest.TelephoneNumberHeader, opt => opt.MapFrom(src => src.TelephoneNumberHeader))
                .ForMember(dest => dest.SurnameHeader, opt => opt.MapFrom(src => src.SurnameHeader))
                .ForMember(dest => dest.LanguageHeader, opt => opt.MapFrom(src => src.LanguageHeader))
                .ForMember(dest => dest.OrganizationHeader, opt => opt.MapFrom(src => src.OrganizationHeader))
                .ForMember(dest => dest.PreferredLabHeader, opt => opt.MapFrom(src => src.PreferredLabHeader))
                .ForMember(dest => dest.ProfessionalSpecialisationHeader, opt => opt.MapFrom(src => src.ProfessionalSpecialisationHeader))
                .ForMember(dest => dest.TitleHeader, opt => opt.MapFrom(src => src.TitleHeader))
                .ForMember(dest => dest.IntendedUseClinicalSettingHeader, opt => opt.MapFrom(src => src.IntendedUseClinicalSettingHeader))
                ;

            CreateMap<CreateUser, UserInputFormSchema>()
                .ForMember(dest => dest.Salutation, opt => opt.MapFrom(src => src.Salutation))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.ProfessionalSpecialisation, opt => opt.MapFrom(src => src.ProfessionalSpecialisation))
                .ForMember(dest => dest.PreferredLab, opt => opt.MapFrom(src => src.PreferredLab))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.TelephoneNumber, opt => opt.MapFrom(src => src.TelephoneNumber))
                .ForMember(dest => dest.EMailAddress, opt => opt.MapFrom(src => src.EMailAddress))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.UnitLabValues, opt => opt.MapFrom(src => src.UnitLabValues))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsSeparateBilling, opt => opt.MapFrom(src => src.IsSeparateBilling))
                .ForMember(dest => dest.ChangeClinicalSetting, opt => opt.Ignore())
                .ForMember(dest => dest.ClinicalSetting, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.ClinicalSetting))
                .ForMember(dest => dest.Organization, opt => opt.Ignore())
                .ForMember(dest => dest.Billing, opt => opt.Ignore())
                ;

            CreateMap<UserModel, CreateUser>();

            CreateMap<Billing, BillingTemplate>()
                .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src => src.BillingAddress))
                .ForMember(dest => dest.BillingCity, opt => opt.MapFrom(src => src.BillingCity))
                .ForMember(dest => dest.BillingCountry, opt => opt.MapFrom(src => src.BillingCountry))
                .ForMember(dest => dest.BillingCountryCode, opt => opt.MapFrom(src => src.BillingCountryCode))
                .ForMember(dest => dest.BillingName, opt => opt.MapFrom(src => src.BillingName))
                .ForMember(dest => dest.BillingPhone, opt => opt.MapFrom(src => src.BillingPhone))
                .ForMember(dest => dest.BillingZip, opt => opt.MapFrom(src => src.BillingZip));

            CreateMap<Billing, BillingModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<BillingTemplate, BillingModel>()
                .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src => src.BillingAddress))
                .ForMember(dest => dest.BillingCity, opt => opt.MapFrom(src => src.BillingCity))
                .ForMember(dest => dest.BillingCountry, opt => opt.MapFrom(src => src.BillingCountry))
                .ForMember(dest => dest.BillingCountryCode, opt => opt.MapFrom(src => src.BillingCountryCode))
                .ForMember(dest => dest.BillingName, opt => opt.MapFrom(src => src.BillingName))
                .ForMember(dest => dest.BillingPhone, opt => opt.MapFrom(src => src.BillingPhone))
                .ForMember(dest => dest.BillingZip, opt => opt.MapFrom(src => src.BillingZip))
                .ForMember(dest => dest.Id, opt => Guid.NewGuid().ToString());

            CreateMap<BillingModel, Billing>();

        }
    }

    public class ValueToUnderlyingTypeConverter<T> : ITypeConverter<BiomarkerValue<T>, T>
    {
        public T Convert(BiomarkerValue<T> source, T destination, ResolutionContext context)
        {
            return source.Value;
        }
    }
}