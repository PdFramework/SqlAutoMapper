namespace PeinearyDevelopment.Framework.Data
{
    using System;

    public static class ValueTypeHelper
    {
        public static bool IsNullable<T>(this T t) { return false; }
        public static bool IsNullable<T>(this T? t) where T : struct { return true; }

        public static bool IsNullable<T>()
        {
            var type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }
    }
}
