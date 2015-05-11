namespace PeinearyDevelopment.Framework.Data.MsSql
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	using System.Globalization;

	internal class MsSqlCommandData : DbCommandData<SqlDbType>
	{
		private static readonly Dictionary<string, SqlDbType> MssqlCommandMapper = new Dictionary<string, SqlDbType> {
                                                                 { typeof(byte[]).FullName, SqlDbType.Binary },
                                                                 { typeof(bool).FullName, SqlDbType.Bit },
                                                                 { typeof(DateTime).FullName, SqlDbType.DateTime2 },
                                                                 { typeof(DateTimeOffset).FullName, SqlDbType.DateTimeOffset },
                                                                 { typeof(decimal).FullName, SqlDbType.Decimal }, 
                                                                 { typeof(Guid).FullName, SqlDbType.UniqueIdentifier }, 
                                                                 { typeof(int).FullName, SqlDbType.Int },
                                                                 { typeof(string).FullName, SqlDbType.NVarChar },
                                                                 { typeof(TimeSpan).FullName, SqlDbType.Time }
                                                             };

		internal MsSqlCommandData()
			: base(MssqlCommandMapper)
		{
		}

		internal MsSqlCommandData(string connectionString)
			: base(connectionString, MssqlCommandMapper)
		{
		}

		internal void AddCommandInformation(SqlCommand command)
		{
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = CommandName;
			command.Parameters.AddRange(Parameters.ToArray());
		}

		public override void AddInputParameter(string parameterName, object value)
		{
			var sqlParameter = new SqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				Value = value,
				Direction = ParameterDirection.Input
			};

			if (value != null)
			{
				sqlParameter.SqlDbType = this.GetDbType(value);
			}

			Parameters.Add(sqlParameter);
		}

		public override void AddInputParameter<T>(string parameterName, T TValue)
		{
			Parameters.Add(new SqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				SqlDbType = this.GetDbType<SqlDbType, T>(),
				Value = TValue,
				Direction = ParameterDirection.Input
			});
		}

		public override void AddOutputParameter<T>(string parameterName, T TValue)
		{
			Parameters.Add(new SqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				SqlDbType = this.GetDbType<SqlDbType, T>(),
				Value = TValue,
				Direction = ParameterDirection.Output
			});
		}

		public override DbConnection CreateDbConnection()
		{
			return new SqlConnection(ConnectionString);
		}
	}
}
