namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Data;

    internal static class DataRecordExtenstions
    {
        internal static bool HasColumn(this IDataRecord dataRecord, string columnName)
        {
            for (var i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
