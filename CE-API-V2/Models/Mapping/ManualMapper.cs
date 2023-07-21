using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                if(prop is null)
                {
                    continue;
                }
                prop!.SetValue(result, new BiomarkerOrderEntry{ OrderNumber = entry.OrderNumber, PreferredUnit = entry.PreferredUnit});
            }

            return result;
        }
    }
}