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
                .ForMember(dest => dest.RequestTimeStamp, opt => opt.MapFrom(src => src.LatestBiomarkers.CreatedOn))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.LatestResponse.RiskClass))
                .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => src.LatestResponse.Risk))
                .ForMember(dest => dest.IsDraft, opt => opt.MapFrom(src => src.LatestBiomarkers.Response == null))
                ;

            CreateMap<ScoringRequest, ScoringRequestModel>();

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
                ;

            CreateMap<CreateUser, UserModel>()
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
            ;

            CreateMap<BiomarkersGeneral, BiomarkerSchema>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.PreferredUnit, opt => opt.MapFrom(src => src.PreferredUnit))
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units));

            CreateMap<BiomarkersLocalized, BiomarkerSchema>()
                .ForMember(dest => dest.InfoText, opt => opt.MapFrom(src => src.InfoText))
                .ForMember(dest => dest.Fieldname, opt => opt.MapFrom(src => src.Fieldname));

            CreateMap<ScoringResponse, ScoringResponseModel>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.classifier_score))
                .ForMember(dest => dest.Recommendation, opt => opt.MapFrom(src => src.RecommendationSummary))
                .ForMember(dest => dest.RecommendationLong, opt => opt.MapFrom(src => src.RecommendationLongText))
                .ForMember(dest => dest.Warnings, opt => opt.MapFrom(src => src.Warnings))
                .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => src.RiskValue))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.RiskClass))
                ;

            CreateMap<ScoringResponseModel, ScoringResponse>()
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src.Request.LatestBiomarkers))
                .ForMember(dest => dest.RiskValue, opt => opt.Ignore())
                .ForMember(dest => dest.Warnings, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationSummary, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationLongText, opt => opt.Ignore());
            
            CreateMap<ScoringSchema, ScoreSchema>()
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationCategories, opt => opt.Ignore());

            CreateMap<BiomarkersLocalized, ScoreSchema>()
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src))
                .ForAllMembers(opts => opts.Ignore());
            
            CreateMap<IEnumerable<BiomarkerSchema>, ScoreSchema>()
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<IEnumerable<RecommendationCategory>, ScoreSchema>()
                .ForMember(dest => dest.RecommendationCategories, opt => opt.MapFrom(src => src.ToArray()))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<RecommendationCategory, ScoringResponse>()
                .ForMember(dest => dest.RecommendationSummary, opt => opt.MapFrom(src => src.ShortText))
                .ForMember(dest => dest.RecommendationLongText, opt => opt.MapFrom(src => src.LongText))
                .ForMember(dest => dest.RiskValue, opt => opt.MapFrom(src => src.RiskValue))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Warnings, opt => opt.Ignore())
                .ForMember(dest => dest.classifier_score, opt => opt.Ignore())
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore());

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