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