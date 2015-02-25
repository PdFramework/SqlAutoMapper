namespace PeinearyDevelopment.Framework.Data.ObjectBuilder
{
	using System;
	using System.Data;

	internal static class DataRecordExtensions
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
