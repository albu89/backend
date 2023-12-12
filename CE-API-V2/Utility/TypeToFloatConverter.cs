namespace CE_API_V2.Utility
{
    public static class TypeToFloatConverter
    {
        public static float MapEnumValueToFloat(object? enumValue)
        {
            if (enumValue == null)
            {
                return 0;
            }

            var type = enumValue.GetType();
            object value = type.GetField("value__").GetValue(enumValue);

            if (value is not int intValue)
            {
                return 0;
            }

            return intValue;
        }

        public static float MapBoolToFloat(bool? boolValue) => boolValue is null ? 0f : float.Parse((bool)boolValue ? "1" : "0");
    }
}
