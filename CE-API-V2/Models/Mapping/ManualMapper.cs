using CE_API_V2.Models.DTO;
using CE_API_V2.Utility;

namespace CE_API_V2.Models.Mapping
{
    public static class ManualMapper
    {
        public static List<BiomarkerOrderModel> ToBiomarkerOrderModels(BiomarkerOrder order)
        {
            var resultList = new List<BiomarkerOrderModel>();

            var properties = typeof(BiomarkerOrder).GetProperties();

            foreach (var property in properties)
            {
                var keyName = ValidationHelpers.GetJsonPropertyKeyName(property.Name, typeof(BiomarkerOrder));
                if (string.IsNullOrEmpty(keyName))
                {
                    continue;
                }

                resultList.Add(new BiomarkerOrderModel
                {
                    BiomarkerId = keyName,
                    OrderNumber = (property.GetValue(order) as BiomarkerOrderEntry)?.OrderNumber ?? 0,
                    PreferredUnit = (property.GetValue(order) as BiomarkerOrderEntry)?.PreferredUnit ?? "SI"
                });
            }

            return resultList;
        }

        public static BiomarkerOrder ToBiomarkerOrder(IEnumerable<BiomarkerOrderModel> order)
        {
            var result = new BiomarkerOrder();

            foreach (var entry in order)
            {
                var prop = ValidationHelpers.GetPropertyByJsonKey(entry.BiomarkerId, typeof(BiomarkerOrder));
                if (prop is null)
                {
                    continue;
                }

                prop!.SetValue(result,
                    new BiomarkerOrderEntry { OrderNumber = entry.OrderNumber, PreferredUnit = entry.PreferredUnit });
            }

            return result;
        }

        public static BiomarkerReturnValue<object>[] MapFromBiomarkersToValues(Biomarkers biomarkers)
        {
            var result = new List<BiomarkerReturnValue<object>>
            {
                new ()
                {
                    Id = "prior_CAD",
                    Value = biomarkers.PriorCAD,
                    DisplayValue = biomarkers.PriorCADDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "age",
                    Value = biomarkers.Age,
                    DisplayValue = biomarkers.AgeDisplayValue,
                    Unit = biomarkers.AgeUnit
                },
                new ()
                {
                    Id = "height",
                    Value = biomarkers.Height,
                    DisplayValue = biomarkers.HeightDisplayValue,
                    Unit = biomarkers.HeightUnit
                },
                new ()
                {
                    Id = "weight",
                    Value = biomarkers.Weight,
                    DisplayValue = biomarkers.WeightDisplayValue,
                    Unit = biomarkers.WeightUnit
                },
                new ()
                { Id = "sex", Value = biomarkers.Sex, DisplayValue = biomarkers.SexDisplayValue, Unit = "SI" },
                new ()
                {
                    Id = "chest_pain",
                    Value = biomarkers.Chestpain,
                    DisplayValue = biomarkers.ChestpainDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "nicotine",
                    Value = biomarkers.Nicotine,
                    DisplayValue = biomarkers.NicotineDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "diabetes",
                    Value = biomarkers.Diabetes,
                    DisplayValue = biomarkers.DiabetesDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "statin",
                    Value = biomarkers.Statin,
                    DisplayValue = biomarkers.StatinDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "tc_agg_inhibitor",
                    Value = biomarkers.Tcagginhibitor,
                    DisplayValue = biomarkers.TcagginhibitorDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "ace_inhibitor",
                    Value = biomarkers.Aceinhibitor,
                    DisplayValue = biomarkers.AceinhibitorDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "calcium_ant",
                    Value = biomarkers.Calciumant,
                    DisplayValue = biomarkers.CalciumantDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "betablocker",
                    Value = biomarkers.Betablocker,
                    DisplayValue = biomarkers.BetablockerDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "diuretic",
                    Value = biomarkers.Diuretic,
                    DisplayValue = biomarkers.DiureticDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "nitrate",
                    Value = biomarkers.Nitrate,
                    DisplayValue = biomarkers.NitrateDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "systolic_bp",
                    Value = biomarkers.Systolicbp,
                    DisplayValue = biomarkers.SystolicbpDisplayValue,
                    Unit = biomarkers.SystolicbpUnit
                },
                new ()
                {
                    Id = "diastolic_bp",
                    Value = biomarkers.Diastolicbp,
                    DisplayValue = biomarkers.DiastolicbpDisplayValue,
                    Unit = biomarkers.DiastolicbpUnit
                },
                new ()
                {
                    Id = "q_wave",
                    Value = biomarkers.Qwave,
                    DisplayValue = biomarkers.QwaveDisplayValue,
                    Unit = "SI"
                },
                new ()
                {
                    Id = "amylase_p",
                    Value = biomarkers.Amylasep,
                    DisplayValue = biomarkers.AmylasepDisplayValue,
                    Unit = biomarkers.AmylasepUnit
                },
                new ()
                {
                    Id = "alkaline",
                    Value = biomarkers.Alkaline,
                    DisplayValue = biomarkers.AlkalineDisplayValue,
                    Unit = biomarkers.AlkalineUnit
                },
                new ()
                {
                    Id = "hs_troponin_t",
                    Value = biomarkers.Hstroponint,
                    DisplayValue = biomarkers.HstroponintDisplayValue,
                    Unit = biomarkers.HstroponintUnit
                },
                new ()
                {
                    Id = "alat",
                    Value = biomarkers.Alat,
                    DisplayValue = biomarkers.AlatDisplayValue,
                    Unit = biomarkers.AlatUnit
                },
                new ()
                {
                    Id = "glucose",
                    Value = biomarkers.Glucose,
                    DisplayValue = biomarkers.GlucoseDisplayValue,
                    Unit = biomarkers.GlucoseUnit
                },
                new ()
                {
                    Id = "bilirubin",
                    Value = biomarkers.Bilirubin,
                    DisplayValue = biomarkers.BilirubinDisplayValue,
                    Unit = biomarkers.BilirubinUnit
                },
                new ()
                {
                    Id = "urea",
                    Value = biomarkers.Urea,
                    DisplayValue = biomarkers.UreaDisplayValue,
                    Unit = biomarkers.UreaUnit
                },
                new ()
                {
                    Id = "uric_acid",
                    Value = biomarkers.Uricacid,
                    DisplayValue = biomarkers.UricacidDisplayValue,
                    Unit = biomarkers.UricacidUnit
                },
                new ()
                {
                    Id = "cholesterol",
                    Value = biomarkers.Cholesterol,
                    DisplayValue = biomarkers.CholesterolDisplayValue,
                    Unit = biomarkers.CholesterolUnit
                },
                new ()
                {
                    Id = "hdl",
                    Value = biomarkers.Hdl,
                    DisplayValue = biomarkers.HdlDisplayValue,
                    Unit = biomarkers.HdlUnit
                },
                new ()
                {
                    Id = "ldl",
                    Value = biomarkers.Ldl,
                    DisplayValue = biomarkers.LdlDisplayValue,
                    Unit = biomarkers.LdlUnit
                },
                new ()
                {
                    Id = "protein",
                    Value = biomarkers.Protein,
                    DisplayValue = biomarkers.ProteinDisplayValue,
                    Unit = biomarkers.ProteinUnit
                },
                new ()
                {
                    Id = "albumin",
                    Value = biomarkers.Albumin,
                    DisplayValue = biomarkers.AlbuminDisplayValue,
                    Unit = biomarkers.AlbuminUnit
                },
                new ()
                {
                    Id = "leukocyte",
                    Value = biomarkers.Leukocyte,
                    DisplayValue = biomarkers.LeukocyteDisplayValue,
                    Unit = biomarkers.LeukocyteUnit
                },
                new ()
                {
                    Id = "mchc",
                    Value = biomarkers.Mchc,
                    DisplayValue = biomarkers.MchcDisplayValue,
                    Unit = biomarkers.MchcUnit
                },
                new ()
                {
                Id = "clinical_setting",
                Value = biomarkers.ClinicalSetting,
                DisplayValue = biomarkers.ClinicalSettingDisplayValue,
                Unit = biomarkers.ClinicalSettingUnit
                }
            };

            return result.ToArray();
        }

        public static BiomarkersDraft MapToBiomarkersDraft(ScoringRequestDraft model)
        {
            var scoringRequest = new ScoringRequestModel();
            return new()
            {
                Request = scoringRequest,
                RequestId = scoringRequest.Id,
                PriorCAD = model.prior_CAD.Value,
                PriorCADDisplayValue = model.prior_CAD.DisplayValue,
                Age = model.Age.Value,
                AgeUnit = model.Age.UnitType,
                AgeDisplayValue = model.Age.DisplayValue,
                Sex = model.Sex.Value,
                SexDisplayValue = model.Sex.DisplayValue,
                Height = model.Height.Value,
                HeightUnit = model.Height.UnitType,
                HeightDisplayValue = model.Height.DisplayValue,
                Weight = model.Weight.Value,
                WeightUnit = model.Weight.UnitType,
                WeightDisplayValue = model.Weight.DisplayValue,
                Chestpain = model.ChestPain.Value,
                ChestpainDisplayValue = model.ChestPain.DisplayValue,
                Nicotine = model.NicotineConsumption.Value,
                NicotineDisplayValue = model.NicotineConsumption.DisplayValue,
                Diabetes = model.Diabetes.Value,
                DiabetesDisplayValue = model.Diabetes.DisplayValue,
                Statin = model.CholesterolLowering_Statin.Value,
                StatinDisplayValue = model.CholesterolLowering_Statin.DisplayValue,
                Tcagginhibitor = model.TCAggregationInhibitor.Value,
                TcagginhibitorDisplayValue = model.TCAggregationInhibitor.DisplayValue,
                Aceinhibitor = model.ACEInhibitor.Value,
                AceinhibitorDisplayValue = model.ACEInhibitor.DisplayValue,
                Calciumant = model.CaAntagonist.Value,
                CalciumantDisplayValue = model.CaAntagonist.DisplayValue,
                Betablocker = model.Betablocker.Value,
                BetablockerDisplayValue = model.Betablocker.DisplayValue,
                Diuretic = model.Diuretic.Value,
                DiureticDisplayValue = model.Diuretic.DisplayValue,
                Nitrate = model.OrganicNitrate.Value,
                NitrateDisplayValue = model.OrganicNitrate.DisplayValue,
                Systolicbp = model.SystolicBloodPressure.Value,
                SystolicbpUnit = model.SystolicBloodPressure.UnitType,
                SystolicbpDisplayValue = model.SystolicBloodPressure.DisplayValue,
                Diastolicbp = model.DiastolicBloodPressure.Value,
                DiastolicbpUnit = model.DiastolicBloodPressure.UnitType,
                DiastolicbpDisplayValue = model.DiastolicBloodPressure.DisplayValue,
                Qwave = model.RestingECG.Value,
                QwaveDisplayValue = model.RestingECG.DisplayValue,
                Amylasep = model.PancreaticAmylase.Value,
                AmylasepUnit = model.PancreaticAmylase.UnitType,
                AmylasepDisplayValue = model.PancreaticAmylase.DisplayValue,
                Alkaline = model.AlkalinePhosphatase.Value,
                AlkalineUnit = model.AlkalinePhosphatase.UnitType,
                AlkalineDisplayValue = model.AlkalinePhosphatase.DisplayValue,
                Hstroponint = model.HsTroponinT.Value,
                HstroponintUnit = model.HsTroponinT.UnitType,
                HstroponintDisplayValue = model.HsTroponinT.DisplayValue,
                Alat = model.Alat.Value,
                AlatUnit = model.Alat.UnitType,
                AlatDisplayValue = model.Alat.DisplayValue,
                Glucose = model.GlucoseFasting.Value,
                GlucoseUnit = model.GlucoseFasting.UnitType,
                GlucoseDisplayValue = model.GlucoseFasting.DisplayValue,
                Bilirubin = model.Bilirubin.Value,
                BilirubinUnit = model.Bilirubin.UnitType,
                BilirubinDisplayValue = model.Bilirubin.DisplayValue,
                Urea = model.Urea.Value,
                UreaUnit = model.Urea.UnitType,
                UreaDisplayValue = model.Urea.DisplayValue,
                Uricacid = model.UricAcid.Value,
                UricacidUnit = model.UricAcid.UnitType,
                UricacidDisplayValue = model.UricAcid.DisplayValue,
                Cholesterol = model.Cholesterol.Value,
                CholesterolUnit = model.Cholesterol.UnitType,
                CholesterolDisplayValue = model.Cholesterol.DisplayValue,
                Hdl = model.Hdl.Value,
                HdlUnit = model.Hdl.UnitType,
                HdlDisplayValue = model.Hdl.DisplayValue,
                Ldl = model.Ldl.Value,
                LdlUnit = model.Ldl.UnitType,
                LdlDisplayValue = model.Ldl.DisplayValue,
                Protein = model.Protein.Value,
                ProteinUnit = model.Protein.UnitType,
                ProteinDisplayValue = model.Protein.DisplayValue,
                Albumin = model.Albumin.Value,
                AlbuminUnit = model.Albumin.UnitType,
                AlbuminDisplayValue = model.Albumin.DisplayValue,
                Leukocyte = model.Leukocytes.Value,
                LeukocyteUnit = model.Leukocytes.UnitType,
                LeukocyteDisplayValue = model.Leukocytes.DisplayValue,
                Mchc = model.Mchc.Value,
                MchcUnit = model.Mchc.UnitType,
                MchcDisplayValue = model.Mchc.DisplayValue,
            };
        }

        public static BiomarkerReturnValue<object>[] MapFromDraftBiomarkersToValues(BiomarkersDraft biomarkers)
        {
            var result = new List<BiomarkerReturnValue<object>>
            {
                new()
                {
                    Id = "prior_CAD",
                    Value = biomarkers.PriorCAD,
                    DisplayValue = biomarkers.PriorCADDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "age",
                    Value = biomarkers.Age,
                    DisplayValue = biomarkers.AgeDisplayValue,
                    Unit = biomarkers.AgeUnit
                },
                new()
                {
                    Id = "height",
                    Value = biomarkers.Height,
                    DisplayValue = biomarkers.HeightDisplayValue,
                    Unit = biomarkers.HeightUnit
                },
                new()
                {
                    Id = "weight",
                    Value = biomarkers.Weight,
                    DisplayValue = biomarkers.WeightDisplayValue,
                    Unit = biomarkers.WeightUnit
                },
                new()
                    { Id = "sex", Value = biomarkers.Sex, DisplayValue = biomarkers.SexDisplayValue, Unit = "SI" },
                new()
                {
                    Id = "chest_pain",
                    Value = biomarkers.Chestpain,
                    DisplayValue = biomarkers.ChestpainDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "nicotine",
                    Value = biomarkers.Nicotine,
                    DisplayValue = biomarkers.NicotineDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "diabetes",
                    Value = biomarkers.Diabetes,
                    DisplayValue = biomarkers.DiabetesDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "statin",
                    Value = biomarkers.Statin,
                    DisplayValue = biomarkers.StatinDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "tc_agg_inhibitor",
                    Value = biomarkers.Tcagginhibitor,
                    DisplayValue = biomarkers.TcagginhibitorDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "ace_inhibitor",
                    Value = biomarkers.Aceinhibitor,
                    DisplayValue = biomarkers.AceinhibitorDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "calcium_ant",
                    Value = biomarkers.Calciumant,
                    DisplayValue = biomarkers.CalciumantDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "betablocker",
                    Value = biomarkers.Betablocker,
                    DisplayValue = biomarkers.BetablockerDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "diuretic",
                    Value = biomarkers.Diuretic,
                    DisplayValue = biomarkers.DiureticDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "nitrate",
                    Value = biomarkers.Nitrate,
                    DisplayValue = biomarkers.NitrateDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "systolic_bp",
                    Value = biomarkers.Systolicbp,
                    DisplayValue = biomarkers.SystolicbpDisplayValue,
                    Unit = biomarkers.SystolicbpUnit
                },
                new()
                {
                    Id = "diastolic_bp",
                    Value = biomarkers.Diastolicbp,
                    DisplayValue = biomarkers.DiastolicbpDisplayValue,
                    Unit = biomarkers.DiastolicbpUnit
                },
                new()
                {
                    Id = "q_wave",
                    Value = biomarkers.Qwave,
                    DisplayValue = biomarkers.QwaveDisplayValue,
                    Unit = "SI"
                },
                new()
                {
                    Id = "amylase_p",
                    Value = biomarkers.Amylasep,
                    DisplayValue = biomarkers.AmylasepDisplayValue,
                    Unit = biomarkers.AmylasepUnit
                },
                new()
                {
                    Id = "alkaline",
                    Value = biomarkers.Alkaline,
                    DisplayValue = biomarkers.AlkalineDisplayValue,
                    Unit = biomarkers.AlkalineUnit
                },
                new()
                {
                    Id = "hs_troponin_t",
                    Value = biomarkers.Hstroponint,
                    DisplayValue = biomarkers.HstroponintDisplayValue,
                    Unit = biomarkers.HstroponintUnit
                },
                new()
                {
                    Id = "alat",
                    Value = biomarkers.Alat,
                    DisplayValue = biomarkers.AlatDisplayValue,
                    Unit = biomarkers.AlatUnit
                },
                new()
                {
                    Id = "glucose",
                    Value = biomarkers.Glucose,
                    DisplayValue = biomarkers.GlucoseDisplayValue,
                    Unit = biomarkers.GlucoseUnit
                },
                new()
                {
                    Id = "bilirubin",
                    Value = biomarkers.Bilirubin,
                    DisplayValue = biomarkers.BilirubinDisplayValue,
                    Unit = biomarkers.BilirubinUnit
                },
                new()
                {
                    Id = "urea",
                    Value = biomarkers.Urea,
                    DisplayValue = biomarkers.UreaDisplayValue,
                    Unit = biomarkers.UreaUnit
                },
                new()
                {
                    Id = "uric_acid",
                    Value = biomarkers.Uricacid,
                    DisplayValue = biomarkers.UricacidDisplayValue,
                    Unit = biomarkers.UricacidUnit
                },
                new()
                {
                    Id = "cholesterol",
                    Value = biomarkers.Cholesterol,
                    DisplayValue = biomarkers.CholesterolDisplayValue,
                    Unit = biomarkers.CholesterolUnit
                },
                new()
                {
                    Id = "hdl",
                    Value = biomarkers.Hdl,
                    DisplayValue = biomarkers.HdlDisplayValue,
                    Unit = biomarkers.HdlUnit
                },
                new()
                {
                    Id = "ldl",
                    Value = biomarkers.Ldl,
                    DisplayValue = biomarkers.LdlDisplayValue,
                    Unit = biomarkers.LdlUnit
                },
                new()
                {
                    Id = "protein",
                    Value = biomarkers.Protein,
                    DisplayValue = biomarkers.ProteinDisplayValue,
                    Unit = biomarkers.ProteinUnit
                },
                new()
                {
                    Id = "albumin",
                    Value = biomarkers.Albumin,
                    DisplayValue = biomarkers.AlbuminDisplayValue,
                    Unit = biomarkers.AlbuminUnit
                },
                new()
                {
                    Id = "leukocyte",
                    Value = biomarkers.Leukocyte,
                    DisplayValue = biomarkers.LeukocyteDisplayValue,
                    Unit = biomarkers.LeukocyteUnit
                },
                new()
                {
                    Id = "mchc",
                    Value = biomarkers.Mchc,
                    DisplayValue = biomarkers.MchcDisplayValue,
                    Unit = biomarkers.MchcUnit
                },
                new()
                {
                    Id = "clinical_setting",
                    Value = biomarkers.ClinicalSetting,
                    DisplayValue = biomarkers.ClinicalSettingDisplayValue,
                    Unit = biomarkers.ClinicalSettingUnit
                }
            };

            return result.ToArray();
        }

        public static BillingTemplate MapFromBillingModelToBillingTemplate(BillingModel billingModel, BillingTemplate billingTemplate)
        {
            billingTemplate.BillingCity = billingModel.BillingCity;
            billingTemplate.BillingName = billingModel.BillingName;
            billingTemplate.BillingCountryCode = billingModel.BillingCountryCode;
            billingTemplate.BillingCountry = billingModel.BillingCountry;
            billingTemplate.BillingAddress = billingModel.BillingAddress;
            billingTemplate.BillingZip = billingModel.BillingZip;
            billingTemplate.BillingPhone = billingModel.BillingPhone;

            return billingTemplate;
        }

        public static void UpdateLatestBiomarkers(ScoringRequestDraft value, BiomarkersDraft latestBiomarkersDraft)
        {
            latestBiomarkersDraft.PriorCAD = value.prior_CAD.Value;
            latestBiomarkersDraft.PriorCADDisplayValue = value.prior_CAD.DisplayValue;
            latestBiomarkersDraft.Age = value.Age.Value;
            latestBiomarkersDraft.AgeUnit = value.Age.UnitType;
            latestBiomarkersDraft.AgeDisplayValue = value.Age.DisplayValue;
            latestBiomarkersDraft.Sex = value.Sex.Value;
            latestBiomarkersDraft.SexDisplayValue = value.Sex.DisplayValue;
            latestBiomarkersDraft.Height = value.Height.Value;
            latestBiomarkersDraft.HeightUnit = value.Height.UnitType;
            latestBiomarkersDraft.HeightDisplayValue = value.Height.DisplayValue;
            latestBiomarkersDraft.Weight = value.Weight.Value;
            latestBiomarkersDraft.WeightUnit = value.Weight.UnitType;
            latestBiomarkersDraft.WeightDisplayValue = value.Weight.DisplayValue;
            latestBiomarkersDraft.Chestpain = value.ChestPain.Value;
            latestBiomarkersDraft.ChestpainDisplayValue = value.ChestPain.DisplayValue;
            latestBiomarkersDraft.Nicotine = value.NicotineConsumption.Value;
            latestBiomarkersDraft.NicotineDisplayValue = value.NicotineConsumption.DisplayValue;
            latestBiomarkersDraft.Diabetes = value.Diabetes.Value;
            latestBiomarkersDraft.DiabetesDisplayValue = value.Diabetes.DisplayValue;
            latestBiomarkersDraft.Statin = value.CholesterolLowering_Statin.Value;
            latestBiomarkersDraft.StatinDisplayValue = value.CholesterolLowering_Statin.DisplayValue;
            latestBiomarkersDraft.Tcagginhibitor = value.TCAggregationInhibitor.Value;
            latestBiomarkersDraft.TcagginhibitorDisplayValue = value.TCAggregationInhibitor.DisplayValue;
            latestBiomarkersDraft.Aceinhibitor = value.ACEInhibitor.Value;
            latestBiomarkersDraft.AceinhibitorDisplayValue = value.ACEInhibitor.DisplayValue;
            latestBiomarkersDraft.Calciumant = value.CaAntagonist.Value;
            latestBiomarkersDraft.CalciumantDisplayValue = value.CaAntagonist.DisplayValue;
            latestBiomarkersDraft.Betablocker = value.Betablocker.Value;
            latestBiomarkersDraft.BetablockerDisplayValue = value.Betablocker.DisplayValue;
            latestBiomarkersDraft.Diuretic = value.Diuretic.Value;
            latestBiomarkersDraft.DiureticDisplayValue = value.Diuretic.DisplayValue;
            latestBiomarkersDraft.Nitrate = value.OrganicNitrate.Value;
            latestBiomarkersDraft.NitrateDisplayValue = value.OrganicNitrate.DisplayValue;
            latestBiomarkersDraft.Systolicbp = value.SystolicBloodPressure.Value;
            latestBiomarkersDraft.SystolicbpUnit = value.SystolicBloodPressure.UnitType;
            latestBiomarkersDraft.SystolicbpDisplayValue = value.SystolicBloodPressure.DisplayValue;
            latestBiomarkersDraft.Diastolicbp = value.DiastolicBloodPressure.Value;
            latestBiomarkersDraft.DiastolicbpUnit = value.DiastolicBloodPressure.UnitType;
            latestBiomarkersDraft.DiastolicbpDisplayValue = value.DiastolicBloodPressure.DisplayValue;
            latestBiomarkersDraft.Qwave = value.RestingECG.Value;
            latestBiomarkersDraft.QwaveDisplayValue = value.RestingECG.DisplayValue;
            latestBiomarkersDraft.Amylasep = value.PancreaticAmylase.Value;
            latestBiomarkersDraft.AmylasepUnit = value.PancreaticAmylase.UnitType;
            latestBiomarkersDraft.AmylasepDisplayValue = value.PancreaticAmylase.DisplayValue;
            latestBiomarkersDraft.Alkaline = value.AlkalinePhosphatase.Value;
            latestBiomarkersDraft.AlkalineUnit = value.AlkalinePhosphatase.UnitType;
            latestBiomarkersDraft.AlkalineDisplayValue = value.AlkalinePhosphatase.DisplayValue;
            latestBiomarkersDraft.Hstroponint = value.HsTroponinT.Value;
            latestBiomarkersDraft.HstroponintUnit = value.HsTroponinT.UnitType;
            latestBiomarkersDraft.HstroponintDisplayValue = value.HsTroponinT.DisplayValue;
            latestBiomarkersDraft.Alat = value.Alat.Value;
            latestBiomarkersDraft.AlatUnit = value.Alat.UnitType;
            latestBiomarkersDraft.AlatDisplayValue = value.Alat.DisplayValue;
            latestBiomarkersDraft.Glucose = value.GlucoseFasting.Value;
            latestBiomarkersDraft.GlucoseUnit = value.GlucoseFasting.UnitType;
            latestBiomarkersDraft.GlucoseDisplayValue = value.GlucoseFasting.DisplayValue;
            latestBiomarkersDraft.Bilirubin = value.Bilirubin.Value;
            latestBiomarkersDraft.BilirubinUnit = value.Bilirubin.UnitType;
            latestBiomarkersDraft.BilirubinDisplayValue = value.Bilirubin.DisplayValue;
            latestBiomarkersDraft.Urea = value.Urea.Value;
            latestBiomarkersDraft.UreaUnit = value.Urea.UnitType;
            latestBiomarkersDraft.UreaDisplayValue = value.Urea.DisplayValue;
            latestBiomarkersDraft.Uricacid = value.UricAcid.Value;
            latestBiomarkersDraft.UricacidUnit = value.UricAcid.UnitType;
            latestBiomarkersDraft.UricacidDisplayValue = value.UricAcid.DisplayValue;
            latestBiomarkersDraft.Cholesterol = value.Cholesterol.Value;
            latestBiomarkersDraft.CholesterolUnit = value.Cholesterol.UnitType;
            latestBiomarkersDraft.CholesterolDisplayValue = value.Cholesterol.DisplayValue;
            latestBiomarkersDraft.Hdl = value.Hdl.Value;
            latestBiomarkersDraft.HdlUnit = value.Hdl.UnitType;
            latestBiomarkersDraft.HdlDisplayValue = value.Hdl.DisplayValue;
            latestBiomarkersDraft.Ldl = value.Ldl.Value;
            latestBiomarkersDraft.LdlUnit = value.Ldl.UnitType;
            latestBiomarkersDraft.LdlDisplayValue = value.Ldl.DisplayValue;
            latestBiomarkersDraft.Protein = value.Protein.Value;
            latestBiomarkersDraft.ProteinUnit = value.Protein.UnitType;
            latestBiomarkersDraft.ProteinDisplayValue = value.Protein.DisplayValue;
            latestBiomarkersDraft.Albumin = value.Albumin.Value;
            latestBiomarkersDraft.AlbuminUnit = value.Albumin.UnitType;
            latestBiomarkersDraft.AlbuminDisplayValue = value.Albumin.DisplayValue;
            latestBiomarkersDraft.Leukocyte = value.Leukocytes.Value;
            latestBiomarkersDraft.LeukocyteUnit = value.Leukocytes.UnitType;
            latestBiomarkersDraft.LeukocyteDisplayValue = value.Leukocytes.DisplayValue;
            latestBiomarkersDraft.Mchc = value.Mchc.Value;
            latestBiomarkersDraft.MchcUnit = value.Mchc.UnitType;
            latestBiomarkersDraft.MchcDisplayValue = value.Mchc.DisplayValue;
            latestBiomarkersDraft.UpdatedOn = DateTimeOffset.Now;
        }
    }
}