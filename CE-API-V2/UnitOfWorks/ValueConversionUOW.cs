using System.Numerics;
using AutoMapper;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Utility;
using System.Reflection;

namespace CE_API_V2.UnitOfWorks
{
    public class ValueConversionUOW : IValueConversionUOW
    {
        private readonly IMapper _mapper;
        private readonly IBiomarkersTemplateService _templateService;

        public ValueConversionUOW(IMapper mapper, IBiomarkersTemplateService templateService)
        {
            _mapper = mapper;
            _templateService = templateService;
        }

        public (ScoringRequestModel, Biomarkers) ConvertToScoringRequest(ScoringRequest scoringRequest, string userId, string patientId)
        {
            var requestModel = _mapper.Map<ScoringRequestModel>(scoringRequest);
            var biomarkers = _mapper.Map<Biomarkers>(scoringRequest);
            requestModel.UserId = userId;
            requestModel.PatientId = patientId;

            return (requestModel, biomarkers);
        }

        /// <summary>
        /// Convert Biomarkervalues to SI Values
        /// </summary>
        /// <param name="scoringRequest">ScoringRequest object to be converted.</param>
        /// <returns>returns converted Scoring Request object</returns>
        public async Task<ScoringRequest> ConvertToSiValues(ScoringRequest scoringRequest)
        {
            var props = scoringRequest.GetType().GetProperties();
            var template = (await _templateService.GetTemplate()).ToList();
            

            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(BiomarkerValue<int>))
                {
                    if (prop.GetValue(scoringRequest) is not BiomarkerValue<int> propWithUnit || propWithUnit.UnitType == "SI")
                        continue;
                    var conversionFactor = FindConversionFactor(propWithUnit, prop, template);
                    
                    propWithUnit.Value = (int)(propWithUnit.Value * conversionFactor);
                    propWithUnit.UnitType = "SI";
                }
                else if (prop.PropertyType == typeof(BiomarkerValue<float>))
                {
                    
                    if (prop.GetValue(scoringRequest) is not BiomarkerValue<float> propWithUnit || propWithUnit.UnitType == "SI")
                        continue;
                    var conversionFactor = FindConversionFactor(propWithUnit, prop, template);

                    propWithUnit.Value *= conversionFactor;
                    propWithUnit.UnitType = "SI";
                }
            }
            return scoringRequest;
        }
        private static float FindConversionFactor<T>(BiomarkerValue<T> propWithUnit, PropertyInfo prop, List<BiomarkerSchema> template)
            where T : INumber<T>
        {
            var unitsForProperty = ValidationHelpers.GetAllUnitsForProperty(prop.Name, template).FirstOrDefault(x => x.UnitType == propWithUnit.UnitType);
         
            if (propWithUnit.UnitType == "SI")
                return default;

            return unitsForProperty?.ConversionFactor ?? 1;   
        }
    }
}