using System.Collections;
using System.Globalization;
using System.Text.Json.Serialization;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;

namespace CE_API_V2.Utility
{
    public static class DataTransferUtility
    {
        public static string CreateQueryString(AiDto patientDataDto, string[]? featuresToDrop = null, string separator = ",")
        {
            // Get all properties on the object
            var properties = patientDataDto.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).LongLength == 0)
                .Where(x => x.CanRead)
                .Where(x => featuresToDrop == null ? true : !featuresToDrop.Contains(x.Name))

                .ToDictionary(x => x.Name, x => x.GetValue(patientDataDto, null) ?? "");

            //Todo - aus altem code
            // FIX q_Zacken_0_nein_1_ja (ECG path, Q-wave) when sending to MAX
            if (properties["q_Zacken_0_nein_1_ja"] != null && (float?)properties["q_Zacken_0_nein_1_ja"] == 2)
            {
                properties["q_Zacken_0_nein_1_ja"] = 0;
            }
            //
            // //Todo - aus altem code
            // if (properties["units"] != null)
            // {
            //     properties.Remove("units");
            // }

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                    ? valueType.GetGenericArguments()[0]
                    : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            var returnString = string.Join("&", properties
                .Select(x => string.Concat(
                    /*Uri.EscapeDataString(*/x.Key/*)*/, "=",
                    /*Uri.EscapeDataString(*/x.Value is DateTime dateTime ? dateTime.ToString(CultureInfo.InvariantCulture) : x.Value.ToString())))/*)*/;   //wenn x.Value = DateTime ToString mit InvariantCulture
            
            return returnString;
        }

        public static ScoringResponseModel ToScoringResponse(string jsonResponse)
        {
            var deserializedResponse = JsonSerializationHelper.DeserializeObject<ScoringResponseModel>(jsonResponse);

            return deserializedResponse;
        }

        public static ScoringResponseModel? FormatResponse(string jsonResponse)
        {
            var scoringResponse = ToScoringResponse(jsonResponse);

            return scoringResponse;
        }
    }
}
