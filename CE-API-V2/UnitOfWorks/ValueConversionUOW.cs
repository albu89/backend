using AutoMapper;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using CE_API_V2.Utility;
using System.Reflection;

namespace CE_API_V2.UnitOfWorks
{
    public class ValueConversionUOW : IValueConversionUOW
    {
        private readonly IMapper _mapper;

        public ValueConversionUOW(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ScoringRequestModel ConvertToScoringRequest(ScoringRequest scoringRequest, string userId, string patientId)
        {
            var requestModel = _mapper.Map<ScoringRequestModel>(scoringRequest);
            requestModel.UserId = userId;
            requestModel.PatientId = patientId;

            return requestModel;
        }

        /// <summary>
        /// Convert Biomarkervalues to SI Values
        /// </summary>
        /// <param name="scoringRequest">ScoringRequest object to be converted.</param>
        /// <returns>returns converted Scoring Request object</returns>
        public ScoringRequest ConvertToSiValues(ScoringRequest scoringRequest)
        {
            var props = scoringRequest.GetType().GetProperties();
            var biomarkesTemplate = new BiomarkersTemplateService(_mapper);
            var template = biomarkesTemplate.GetTemplate().GetAwaiter().GetResult();

            foreach (var prop in props)
            {
                if ((prop.PropertyType == typeof(BiomarkerValue<int>)))
                {
                    var propWithUnit = prop.GetValue(scoringRequest) as BiomarkerValue<int>;
                    var unitsForProperty = ValidationHelpers.GetAllUnitsForProperty(prop.Name, template).FirstOrDefault(x => x.UnitType == propWithUnit.UnitType);
                    var siUnitForProperty = ValidationHelpers.GetAllUnitsForProperty(prop.Name, template).FirstOrDefault(x => x.UnitType == "SI");

                    if (propWithUnit != null && propWithUnit?.UnitType != "SI")
                    {
                        propWithUnit.Value = (int)(propWithUnit.Value
                            / unitsForProperty?.ConversionFactor ?? 1
                            * siUnitForProperty?.ConversionFactor ?? 1);
                        propWithUnit.UnitType = "SI";
                    }
                }
                else if ((prop.PropertyType == typeof(BiomarkerValue<float>)))
                {
                    var propWithUnit = prop.GetValue(scoringRequest) as BiomarkerValue<float>;
                    if (propWithUnit != null && propWithUnit?.UnitType != "SI")
                    {
                        var unitsForProperty = ValidationHelpers.GetAllUnitsForProperty(prop.Name, template).FirstOrDefault(x => x.UnitType == propWithUnit.UnitType);
                        var siUnitForProperty = ValidationHelpers.GetAllUnitsForProperty(prop.Name, template).FirstOrDefault(x => x.UnitType == "SI");
                        propWithUnit.Value = propWithUnit.Value
                                             / unitsForProperty?.ConversionFactor ?? 1
                                             * siUnitForProperty?.ConversionFactor ?? 1;
                        propWithUnit.UnitType = "SI";
                    }
                }
            }
            return scoringRequest;
        }
    }
}