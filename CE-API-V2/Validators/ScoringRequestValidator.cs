using CE_API_V2.Models.DTO;
using CE_API_V2.Services;
using FluentValidation;
using CE_API_V2.Utility;

namespace CE_API_V2.Validators
{
    public class ScoringRequestValidator : AbstractValidator<ScoringRequestDto>
    {
        public ScoringRequestValidator()
        {
            var biomarkesTemplate = new BiomarkersTemplateService();

            var template = biomarkesTemplate.GetTemplate().GetAwaiter().GetResult();

            //Age validation
            RuleFor(scoringRequest => scoringRequest.Age).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Age.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Age.UnitType).NotNull().DependentRules(() =>
                    {
                        RuleFor(scoringRequest => (float)scoringRequest.Age.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Age), x.Age.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Age), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Age.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Age), x.Age.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Age), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => scoringRequest.Sex.Value)
                            .IsInEnum()
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Sex), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Height.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Height), x.Height.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Height), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Height.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Height), x.Height.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Height), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Weight.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Weight), x.Weight.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Weight), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Weight.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Weight), x.Weight.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Weight), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => scoringRequest.ChestPain.Value).IsInEnum().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.ChestPain), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => scoringRequest.NicotineConsumption.Value).IsInEnum().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.NicotineConsumption), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => scoringRequest.Diabetes.Value).IsInEnum().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Diabetes), typeof(ScoringRequestDto))}}}");
                    });
                });
            });
            //Cholesterol
            RuleFor(scoringRequest => scoringRequest.Cholesterol).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Cholesterol.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Cholesterol.UnitType).NotNull().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Cholesterol), typeof(ScoringRequestDto))}}}");
                });
            });
            //TCAaggregation
            RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.TCAggregationInhibitor.UnitType).NotNull().WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.TCAggregationInhibitor), typeof(ScoringRequestDto))}}}");
                });
            });
            //ACEInhibitor
            RuleFor(scoringRequest => scoringRequest.ACEInhibitor).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.ACEInhibitor.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.ACEInhibitor.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.ACEInhibitor), typeof(ScoringRequestDto))}}}");
                });
            });
            //CaAntagonist
            RuleFor(scoringRequest => scoringRequest.CaAntagonist).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.CaAntagonist.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.CaAntagonist.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.CaAntagonist), typeof(ScoringRequestDto))}}}");
                });
            });
            //Betablocker
            RuleFor(scoringRequest => scoringRequest.Betablocker).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Betablocker.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Betablocker.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Betablocker), typeof(ScoringRequestDto))}}}");
                });
            });
            //Diuretic
            RuleFor(scoringRequest => scoringRequest.Diuretic).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.Diuretic.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.Diuretic.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Diuretic), typeof(ScoringRequestDto))}}}");
                });
            });//CaAntagonist
            RuleFor(scoringRequest => scoringRequest.CaAntagonist).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.CaAntagonist.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.CaAntagonist.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.CaAntagonist), typeof(ScoringRequestDto))}}}");
                });
            });
            //OrganicNitrate
            RuleFor(scoringRequest => scoringRequest.OrganicNitrate).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.OrganicNitrate.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.OrganicNitrate.UnitType)
                        .NotNull()
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.OrganicNitrate), typeof(ScoringRequestDto))}}}");
                });
            });
            //SystolicBloodPressure Validation
            RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure).NotNull().DependentRules(() =>
            {
                RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure.Value).NotNull().DependentRules(() =>
                {
                    RuleFor(scoringRequest => scoringRequest.SystolicBloodPressure.UnitType).NotNull().DependentRules(() =>
                    {
                        RuleFor(scoringRequest => (float)scoringRequest.SystolicBloodPressure.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.SystolicBloodPressure), x.SystolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.SystolicBloodPressure), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.SystolicBloodPressure.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.SystolicBloodPressure), x.SystolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.SystolicBloodPressure), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.DiastolicBloodPressure.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.DiastolicBloodPressure), x.DiastolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.DiastolicBloodPressure), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.DiastolicBloodPressure.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.DiastolicBloodPressure), x.DiastolicBloodPressure.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.DiastolicBloodPressure), typeof(ScoringRequestDto))}}}");
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
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.RestingECG), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.PancreaticAmylase.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.PancreaticAmylase), x.PancreaticAmylase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.PancreaticAmylase), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.PancreaticAmylase.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.PancreaticAmylase), x.PancreaticAmylase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.PancreaticAmylase), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.AlkalinePhosphatase.Value)
                            .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.AlkalinePhosphatase), x.AlkalinePhosphatase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.AlkalinePhosphatase), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.AlkalinePhosphatase.Value)
                            .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.AlkalinePhosphatase), x.AlkalinePhosphatase.UnitType, template))
                            .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.AlkalinePhosphatase), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.HsTroponinT.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.HsTroponinT), x.HsTroponinT.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.HsTroponinT), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.HsTroponinT.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.HsTroponinT), x.HsTroponinT.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.HsTroponinT), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Alat.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Alat), x.Alat.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Alat), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Alat.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Alat), x.Alat.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Alat), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.GlucoseFasting.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.GlucoseFasting), x.GlucoseFasting.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.GlucoseFasting), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.GlucoseFasting.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.GlucoseFasting), x.GlucoseFasting.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.GlucoseFasting), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Bilirubin.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Bilirubin), x.Bilirubin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Bilirubin), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Bilirubin.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Bilirubin), x.Bilirubin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Bilirubin), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Urea.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Urea), x.Urea.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Urea), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Urea.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Urea), x.Urea.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Urea), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.UricAcid.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.UricAcid), x.UricAcid.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.UricAcid), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.UricAcid.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.UricAcid), x.UricAcid.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.UricAcid), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Cholesterol.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Cholesterol), x.Cholesterol.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Cholesterol), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Cholesterol.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Cholesterol), x.Cholesterol.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Cholesterol), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Hdl.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Hdl), x.Hdl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Hdl), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Hdl.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Hdl), x.Hdl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Hdl), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Ldl.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Ldl), x.Ldl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Ldl), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Ldl.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Ldl), x.Ldl.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Ldl), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Protein.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Protein), x.Protein.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Protein), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Protein.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Protein), x.Protein.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Protein), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Albumin.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Albumin), x.Albumin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Albumin), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Albumin.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Albumin), x.Albumin.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Albumin), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Leukocytes.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Leukocytes), x.Leukocytes.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Leukocytes), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Leukocytes.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Leukocytes), x.Leukocytes.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Leukocytes), typeof(ScoringRequestDto))}}}");
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
                        RuleFor(scoringRequest => (float)scoringRequest.Mchc.Value)
                        .LessThanOrEqualTo(x => ValidationHelpers.GetMaxValue(nameof(x.Mchc), x.Mchc.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Mchc), typeof(ScoringRequestDto))}}}");
                        
                        RuleFor(scoringRequest => (float)scoringRequest.Mchc.Value)
                        .GreaterThanOrEqualTo(x => ValidationHelpers.GetMinValue(nameof(x.Mchc), x.Mchc.UnitType, template))
                        .WithName($"{{{ValidationHelpers.GetJsonPropertyKeyName(nameof(ScoringRequestDto.Mchc), typeof(ScoringRequestDto))}}}");
                    });
                });
            });
        }
    }
}