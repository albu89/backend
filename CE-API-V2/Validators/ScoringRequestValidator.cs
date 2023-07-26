using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using FluentValidation;
using CE_API_V2.Utility;
using AutoMapper;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Localization.JsonStringFactroy;

namespace CE_API_V2.Validators
{
    public class ScoringRequestValidator : AbstractValidator<ScoringRequest>
    {
        public ScoringRequestValidator()
        {
            JsonStringLocalizerFactory factory = new();
            var loc = factory.Create(this.GetType());
            
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            var mapper = mapperConfig.CreateMapper();
            var biomarkesTemplate = new BiomarkersTemplateService(mapper);

            var template = biomarkesTemplate.GetTemplate().GetAwaiter().GetResult();

            //Age validation
            RuleFor(scoringRequest => scoringRequest.Age).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Age.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Age.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Age), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Age.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Age), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Age), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Age.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Age), x.Age.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Age), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Age.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Age), x.Age.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Age), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Sex validation
            RuleFor(scoringRequest => scoringRequest.Sex).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Sex.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Sex.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Sex), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Sex.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Sex), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Sex), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => scoringRequest.Sex.Value)
                            .IsInEnum()
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Sex), typeof(ScoringRequest))}}}");
                    });
                });
            });
            // //Height validation
            RuleFor(scoringRequest => scoringRequest.Height).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Height.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Height.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Height), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Height.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Height), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Height), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Height.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Height), x.Height.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Height), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Height.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Height), x.Height.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Height), typeof(ScoringRequest))}}}");
                    });
                });
            });

            // //Weight validation
            RuleFor(scoringRequest => scoringRequest.Weight).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Weight.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Weight.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Weight), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Weight.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Weight), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Weight), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Weight.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Weight), x.Weight.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Weight), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Weight.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Weight), x.Weight.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Weight), typeof(ScoringRequest))}}}");
                    });
                });
            });
            // //ChestPain validation
            RuleFor(scoringRequest => scoringRequest.ChestPain).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.ChestPain.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.ChestPain.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.ChestPain), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.ChestPain.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.ChestPain), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.ChestPain), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => scoringRequest.ChestPain.Value).IsInEnum().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.ChestPain), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Nikotine
            RuleFor(scoringRequest => scoringRequest.NicotineConsumption).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.NicotineConsumption.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.NicotineConsumption.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.NicotineConsumption), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.NicotineConsumption.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.NicotineConsumption), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.NicotineConsumption), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => scoringRequest.NicotineConsumption.Value).IsInEnum().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.NicotineConsumption), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Diabetes
            RuleFor(scoringRequest => scoringRequest.Diabetes).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Diabetes.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Diabetes.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Diabetes), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Diabetes.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Diabetes), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Diabetes), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => scoringRequest.Diabetes.Value).IsInEnum().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Diabetes), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Cholesterol
            RuleFor(scoringRequest => scoringRequest.Cholesterol).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Cholesterol.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Cholesterol), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.Cholesterol.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.Cholesterol.UnitType).NotNull().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}");
                });
            });
            //TCAaggregation
            RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.TCAggregationInhibitor), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.TCAggregationInhibitor), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.TCAggregationInhibitor), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor.UnitType).NotNull().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.TCAggregationInhibitor), typeof(ScoringRequest))}}}");
                });
            });
            //ACEInhibitor
            RuleFor(scoringRequest => scoringRequest.ACEInhibitor).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.ACEInhibitor.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.ACEInhibitor), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.ACEInhibitor.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.ACEInhibitor), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.ACEInhibitor), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.ACEInhibitor.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.ACEInhibitor), typeof(ScoringRequest))}}}");
                });
            });

            //Betablocker
            RuleFor(scoringRequest => scoringRequest.Betablocker).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Betablocker.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Betablocker), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.Betablocker.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Betablocker), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Betablocker), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.Betablocker.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Betablocker), typeof(ScoringRequest))}}}");
                });
            });
            //Diuretic
            RuleFor(scoringRequest => scoringRequest.Diuretic).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Diuretic.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Diuretic), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.Diuretic.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Diuretic), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Diuretic), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.Diuretic.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Diuretic), typeof(ScoringRequest))}}}");
                });
            });//CaAntagonist
            RuleFor(scoringRequest => scoringRequest.CaAntagonist).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.CaAntagonist.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.CaAntagonist), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.CaAntagonist.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.CaAntagonist), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.CaAntagonist), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.CaAntagonist.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.CaAntagonist), typeof(ScoringRequest))}}}");
                });
            });
            //OrganicNitrate
            RuleFor(scoringRequest => scoringRequest.OrganicNitrate).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.OrganicNitrate.Value).NotNull().DependentRules(() =>
                {
                    var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.OrganicNitrate), template).Select(x => x.UnitType);
                    RuleFor(scoringRequest => scoringRequest.OrganicNitrate.UnitType).Must(x => unitTypes.Contains(x))
                        .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.OrganicNitrate), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.OrganicNitrate), typeof(ScoringRequest))}}}");

                    RuleFor(scoringRequest => scoringRequest.OrganicNitrate.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.OrganicNitrate), typeof(ScoringRequest))}}}");
                });
            });
            //SystolicBloodPressure Validation
            RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.SystolicBloodPressure), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.SystolicBloodPressure), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.SystolicBloodPressure), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.SystolicBloodPressure.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.SystolicBloodPressure), x.SystolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.SystolicBloodPressure), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.SystolicBloodPressure.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.SystolicBloodPressure), x.SystolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.SystolicBloodPressure), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //SystolicBloodPressure Validation
            RuleFor(scoringRequest => scoringRequest.DiastolicBloodPressure).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.DiastolicBloodPressure.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.DiastolicBloodPressure.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.DiastolicBloodPressure), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.DiastolicBloodPressure.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.DiastolicBloodPressure), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.DiastolicBloodPressure), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.DiastolicBloodPressure.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.DiastolicBloodPressure), x.DiastolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.DiastolicBloodPressure), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.DiastolicBloodPressure.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.DiastolicBloodPressure), x.DiastolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.DiastolicBloodPressure), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //RestingECG
            RuleFor(scoringRequest => scoringRequest.RestingECG).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.RestingECG.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.RestingECG.UnitType).NotNull().DependentRules(() =>
                    {
                        RuleFor(scoringRequest => scoringRequest.RestingECG.Value)
                            .IsInEnum()
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.RestingECG), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //PancreaticAmylase Validation
            RuleFor(scoringRequest => scoringRequest.PancreaticAmylase).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.PancreaticAmylase.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.PancreaticAmylase.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.PancreaticAmylase), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.PancreaticAmylase.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.PancreaticAmylase), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.PancreaticAmylase), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.PancreaticAmylase.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.PancreaticAmylase), x.PancreaticAmylase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.PancreaticAmylase), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.PancreaticAmylase.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.PancreaticAmylase), x.PancreaticAmylase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.PancreaticAmylase), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //AlkalinePhosphatase Validation
            RuleFor(scoringRequest => scoringRequest.AlkalinePhosphatase).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.AlkalinePhosphatase.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.AlkalinePhosphatase.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.AlkalinePhosphatase), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.AlkalinePhosphatase.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.AlkalinePhosphatase), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.AlkalinePhosphatase), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.AlkalinePhosphatase.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.AlkalinePhosphatase), x.AlkalinePhosphatase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.AlkalinePhosphatase), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.AlkalinePhosphatase.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.AlkalinePhosphatase), x.AlkalinePhosphatase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.AlkalinePhosphatase), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //HsTroponinT Validation
            RuleFor(scoringRequest => scoringRequest.HsTroponinT).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.HsTroponinT.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.HsTroponinT.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.HsTroponinT), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.HsTroponinT.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.HsTroponinT), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.HsTroponinT), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.HsTroponinT.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.HsTroponinT), x.HsTroponinT.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.HsTroponinT), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.HsTroponinT.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.HsTroponinT), x.HsTroponinT.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.HsTroponinT), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Alat Validation
            RuleFor(scoringRequest => scoringRequest.Alat).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Alat.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Alat.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Alat), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Alat.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Alat), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Alat), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Alat.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Alat), x.Alat.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Alat), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Alat.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Alat), x.Alat.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Alat), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //GlocuseFasting Validation
            RuleFor(scoringRequest => scoringRequest.GlucoseFasting).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.GlucoseFasting.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.GlucoseFasting.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.GlucoseFasting), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.GlucoseFasting.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.GlucoseFasting), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.GlucoseFasting), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.GlucoseFasting.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.GlucoseFasting), x.GlucoseFasting.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.GlucoseFasting), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.GlucoseFasting.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.GlucoseFasting), x.GlucoseFasting.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.GlucoseFasting), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Bilirubin Validation
            RuleFor(scoringRequest => scoringRequest.Bilirubin).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Bilirubin.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Bilirubin.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Bilirubin), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Bilirubin.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Bilirubin), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Bilirubin), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Bilirubin.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Bilirubin), x.Bilirubin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Bilirubin), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Bilirubin.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Bilirubin), x.Bilirubin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Bilirubin), typeof(ScoringRequest))}}}");
                    });
                });
            });

            //Urea Validation
            RuleFor(scoringRequest => scoringRequest.Urea).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Urea.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Urea.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Urea), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Urea.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Urea), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Urea), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Urea.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Urea), x.Urea.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Urea), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Urea.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Urea), x.Urea.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Urea), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //UricAcid Validation
            RuleFor(scoringRequest => scoringRequest.UricAcid).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.UricAcid.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.UricAcid.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.UricAcid), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.UricAcid.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.UricAcid), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.UricAcid), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.UricAcid.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.UricAcid), x.UricAcid.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.UricAcid), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.UricAcid.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.UricAcid), x.UricAcid.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.UricAcid), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Cholesterol Validation
            RuleFor(scoringRequest => scoringRequest.Cholesterol).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Cholesterol.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Cholesterol.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Cholesterol), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Cholesterol.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Cholesterol.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Cholesterol), x.Cholesterol.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Cholesterol.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Cholesterol), x.Cholesterol.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Cholesterol), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Hdl Validation
            RuleFor(scoringRequest => scoringRequest.Hdl).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Hdl.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Hdl.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Hdl), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Hdl.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Hdl), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Hdl), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Hdl.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Hdl), x.Hdl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Hdl), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Hdl.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Hdl), x.Hdl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Hdl), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Ldl Validation
            RuleFor(scoringRequest => scoringRequest.Ldl).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Ldl.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Ldl.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Ldl), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Ldl.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Ldl), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Ldl), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Ldl.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Ldl), x.Ldl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Ldl), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Ldl.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Ldl), x.Ldl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Ldl), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Protein Validation
            RuleFor(scoringRequest => scoringRequest.Protein).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Protein.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Protein.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Protein), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Protein.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Protein), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Protein), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Protein.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Protein), x.Protein.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Protein), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Protein.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Protein), x.Protein.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Protein), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Albumin Validation
            RuleFor(scoringRequest => scoringRequest.Albumin).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Albumin.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Albumin.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Albumin), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Albumin.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Albumin), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Albumin), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Albumin.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Albumin), x.Albumin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Albumin), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Albumin.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Albumin), x.Albumin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Albumin), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Leukocytes Validation
            RuleFor(scoringRequest => scoringRequest.Leukocytes).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Leukocytes.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Leukocytes.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Leukocytes), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Leukocytes.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Leukocytes), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Leukocytes), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Leukocytes.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Leukocytes), x.Leukocytes.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Leukocytes), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Leukocytes.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Leukocytes), x.Leukocytes.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Leukocytes), typeof(ScoringRequest))}}}");
                    });
                });
            });
            //Mchc Validation
            RuleFor(scoringRequest => scoringRequest.Mchc).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Mchc.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Mchc.UnitType).NotNull().DependentRules(() =>
                    {
                        var unitTypes = ValidationHelpers.GetAllUnitsForProperty(nameof(ScoringRequest.Mchc), template).Select(x => x.UnitType);
                        RuleFor(scoringRequest => scoringRequest.Mchc.UnitType).Must(x => unitTypes.Contains(x))
                            .WithMessage(x => $"'{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Mchc), typeof(ScoringRequest))}}}': {loc["Validation.ErrorUnitDoesNotExist"]}")
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Mchc), typeof(ScoringRequest))}}}");

                        RuleFor(scoringRequest => (float)scoringRequest.Mchc.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Mchc), x.Mchc.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Mchc), typeof(ScoringRequest))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Mchc.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Mchc), x.Mchc.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequest.Mchc), typeof(ScoringRequest))}}}");
                    });
                });
            });
        }
    }
}