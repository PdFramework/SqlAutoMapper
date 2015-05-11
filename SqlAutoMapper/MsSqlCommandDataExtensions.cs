namespace PeinearyDevelopment.Framework.Data.MsSql
{
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Globalization;

	internal static class MsSqlCommandDataExtensions
	{
		internal static void AddInputDataTable<T>(this MsSqlCommandData sqlCommandData, string parameterName, IEnumerable<T> TValues)
		{
			// TODO: update for more generic T scenarios
			var typeofT = typeof(T);
			var dataTable = new DataTable();
			dataTable.Columns.Add("Id", typeofT);
			foreach (var tValue in TValues)
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
