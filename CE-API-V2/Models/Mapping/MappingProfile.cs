using AutoMapper;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;

namespace CE_API_V2.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ScoringRequest, ScoringHistoryDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Response.classifier_score))
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestTimeStamp, opt => opt.MapFrom(src => src.CreatedOn))
                ;

            CreateMap<ScoringRequestDto, ScoringRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src))
                ;

            CreateMap<ScoringResponse, ScoringResponseDto>();

            CreateMap<ScoringRequestDto, Biomarkers>()
                .ForMember(dest => dest.Glucose, opt => opt.MapFrom(src => src.GlucoseFasting))
                .ForMember(dest => dest.Nicotine, opt => opt.MapFrom(src => src.NicotineConsumption))
                .ForMember(dest => dest.AceInhibitor, opt => opt.MapFrom(src => src.ACEInhibitor))
                .ForMember(dest => dest.AlkalinePhosphate, opt => opt.MapFrom(src => src.AlkalinePhosphatase))
                .ForMember(dest => dest.TcAggInhibitor, opt => opt.MapFrom(src => src.TCAggregationInhibitor))
                .ForMember(dest => dest.PriorCAD, opt => opt.MapFrom(src => src.prior_CAD))
                .ForMember(dest => dest.Statin, opt => opt.MapFrom(src => src.CholesterolLowering_Statin))

                .ForMember(dest => dest.AgeUnit, opt => opt.MapFrom(src => src.Age.UnitType))
                .ForMember(dest => dest.HdlUnit, opt => opt.MapFrom(src => src.Hdl.UnitType))
                .ForMember(dest => dest.LdlUnit, opt => opt.MapFrom(src => src.Ldl.UnitType))
                .ForMember(dest => dest.AlatUnit, opt => opt.MapFrom(src => src.Alat.UnitType))
                .ForMember(dest => dest.AlbuminUnit, opt => opt.MapFrom(src => src.Albumin.UnitType))
                .ForMember(dest => dest.UreaUnit, opt => opt.MapFrom(src => src.Urea.UnitType))
                .ForMember(dest => dest.MchcUnit, opt => opt.MapFrom(src => src.Mchc.UnitType))
                .ForMember(dest => dest.AlkalinePhosphataseUnit, opt => opt.MapFrom(src => src.AlkalinePhosphatase.UnitType))
                .ForMember(dest => dest.BilirubinUnit, opt => opt.MapFrom(src => src.Bilirubin.UnitType))
                .ForMember(dest => dest.CholesterolUnit, opt => opt.MapFrom(src => src.Cholesterol.UnitType))
                .ForMember(dest => dest.PancreaticAmylaseUnit, opt => opt.MapFrom(src => src.PancreaticAmylase.UnitType))
                .ForMember(dest => dest.HeightUnit, opt => opt.MapFrom(src => src.Height.UnitType))
                .ForMember(dest => dest.WeightUnit, opt => opt.MapFrom(src => src.Weight.UnitType))
                .ForMember(dest => dest.GlucoseUnit, opt => opt.MapFrom(src => src.GlucoseFasting.UnitType))
                .ForMember(dest => dest.LeukocytesUnit, opt => opt.MapFrom(src => src.Leukocytes.UnitType))
                .ForMember(dest => dest.UricAcidUnit, opt => opt.MapFrom(src => src.UricAcid.UnitType))
                .ForMember(dest => dest.DiastolicBloodPressureUnit, opt => opt.MapFrom(src => src.DiastolicBloodPressure.UnitType))
                .ForMember(dest => dest.SystolicBloodPressureUnit, opt => opt.MapFrom(src => src.SystolicBloodPressure.UnitType))
                .ForMember(dest => dest.HsTroponinTUnit, opt => opt.MapFrom(src => src.HsTroponinT.UnitType))
                .ForMember(dest => dest.ProteinUnit, opt => opt.MapFrom(src => src.Protein.UnitType))
                ;

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.TenantID, opt => opt.MapFrom(_ => string.Empty));

            CreateMap<User, UserDto>();

            CreateMap(typeof(BiomarkerValueDto<string>), typeof(string))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<string>));
            CreateMap(typeof(BiomarkerValueDto<int>), typeof(int))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<int>));
            CreateMap(typeof(BiomarkerValueDto<float>), typeof(float))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<float>));
            CreateMap(typeof(BiomarkerValueDto<bool>), typeof(bool))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<bool>));
            CreateMap(typeof(BiomarkerValueDto<PatientDataEnums.Sex>), typeof(PatientDataEnums.Sex))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.Sex>));
            CreateMap(typeof(BiomarkerValueDto<PatientDataEnums.NicotineConsumption>),
                    typeof(PatientDataEnums.NicotineConsumption))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.NicotineConsumption>));
            CreateMap(typeof(BiomarkerValueDto<PatientDataEnums.ChestPain>), typeof(PatientDataEnums.ChestPain))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.ChestPain>));
            CreateMap(typeof(BiomarkerValueDto<PatientDataEnums.ClinicalSetting>),
                    typeof(PatientDataEnums.ClinicalSetting))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.ClinicalSetting>));
            CreateMap(typeof(BiomarkerValueDto<PatientDataEnums.DiabetesStatus>),
                    typeof(PatientDataEnums.DiabetesStatus))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.DiabetesStatus>));
            CreateMap(typeof(BiomarkerValueDto<PatientDataEnums.RestingEcg>), typeof(PatientDataEnums.RestingEcg))
                .ConvertUsing(typeof(ValueToUnderlyingTypeConverter<PatientDataEnums.RestingEcg>));
            ;

            CreateMap<BiomarkersGeneral, BiomarkerSchemaDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.PreferredUnit, opt => opt.MapFrom(src => src.PreferredUnit))
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units));

            CreateMap<BiomarkersLocalized, BiomarkerSchemaDto>()
                .ForMember(dest => dest.InfoText, opt => opt.MapFrom(src => src.InfoText))
                .ForMember(dest => dest.Fieldname, opt => opt.MapFrom(src => src.Fieldname));

            CreateMap<ScoringResponse, ScoringResponseSummary>()
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src.Request.Biomarkers))
                .ForMember(dest => dest.RiskValue, opt => opt.Ignore())
                .ForMember(dest => dest.Warnings, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationSummary, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationLongText, opt => opt.Ignore());
            
            CreateMap<ScoringSchema, ScoreSummary>()
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore())
                .ForMember(dest => dest.RecommendationCategories, opt => opt.Ignore());

            CreateMap<BiomarkersLocalized, ScoreSummary>()
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src))
                .ForAllMembers(opts => opts.Ignore());
            
            CreateMap<IEnumerable<BiomarkerSchemaDto>, ScoreSummary>()
                .ForMember(dest => dest.Biomarkers, opt => opt.MapFrom(src => src))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<IEnumerable<RecommendationCategory>, ScoreSummary>()
                .ForMember(dest => dest.RecommendationCategories, opt => opt.MapFrom(src => src.ToArray()))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<RecommendationCategory, ScoringResponseSummary>()
                .ForMember(dest => dest.RecommendationSummary, opt => opt.MapFrom(src => src.ShortText))
                .ForMember(dest => dest.RecommendationLongText, opt => opt.MapFrom(src => src.LongText))
                .ForMember(dest => dest.RiskValue, opt => opt.MapFrom(src => src.RiskValue))
                .ForMember(dest => dest.RiskClass, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Warnings, opt => opt.Ignore())
                .ForMember(dest => dest.classifier_score, opt => opt.Ignore())
                .ForMember(dest => dest.classifier_sign, opt => opt.Ignore())
                .ForMember(dest => dest.Biomarkers, opt => opt.Ignore());
        }
    }
    public class ValueToUnderlyingTypeConverter<T> : ITypeConverter<BiomarkerValueDto<T>, T>
    {
        public T Convert(BiomarkerValueDto<T> source, T destination, ResolutionContext context)
        {
            return source.Value;
        }
    }
}