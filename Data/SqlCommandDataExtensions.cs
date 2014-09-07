namespace PeinearyDevelopment.Framework.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;

    internal static class SqlCommandDataExtensions
    {
        internal static void AddInputParameter(this SqlCommandData sqlCommandData, string parameterName, object value)
        {
            var sqlParameter = new SqlParameter
                                   {
                                       ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
                                       Value = value,
                                       Direction = ParameterDirection.Input
                                   };

            if (value != null)
            {
                sqlParameter.SqlDbType = sqlCommandData.CommandMapper[value.GetType().FullName];
            }

            sqlCommandData.Parameters.Add(sqlParameter);
        }

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

        internal static void AddInputDataTable<T>(this SqlCommandData sqlCommandData, string parameterName, IEnumerable<T> TValues)
        {
            // TODO: update for more generic T scenarios
            var typeofT = typeof(T);
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeofT);
            foreach (var tValue in TValues)//.Where(donorId => donorId != null))
            {
                var row = dataTable.NewRow();
                row["Id"] = tValue;
                dataTable.Rows.Add(row);
            }

            sqlCommandData.Parameters.Add(new SqlParameter
                                              {
                                                  ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
                                                  SqlDbType = SqlDbType.Structured,
                                                  Value = dataTable,
                                                  Direction = ParameterDirection.Input,
                                                  TypeName = string.Format("[dbo].[{0}List]", typeofT.Name)
                                              });
        }
    }
}
