namespace PeinearyDevelopment.Framework.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    internal static class SqlCommandDataExtensions
    {
        internal static void AddInputParameter<T>(this SqlCommandData sqlCommandData, string parameterName, T TValue)
        {
            sqlCommandData.Parameters.Add(new SqlParameter
            {
                ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
                SqlDbType = sqlCommandData.CommandMapper[typeof(T).FullName],
                Value = TValue,
                Direction = ParameterDirection.Input
            });
        }

        internal static void AddOutputParameter<T>(this SqlCommandData sqlCommandData, string parameterName, T TValue)
        {
            sqlCommandData.Parameters.Add(new SqlParameter
            {
                ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
                SqlDbType = sqlCommandData.CommandMapper[typeof(T).FullName],
                Value = TValue,
                Direction = ParameterDirection.Output
            });
        }
    }
}
