using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace PeinearyDevelopment.Framework.Data.NpgSql
{
  internal class NpgSqlCommandData : DbCommandData<NpgsqlDbType>
	{
		//https://github.com/npgsql/npgsql/wiki/User-Manual#user-content-supported-data-types
		private static readonly Dictionary<string, NpgsqlDbType> NpgsqlCommandMapper = new Dictionary<string, NpgsqlDbType> {
			{ typeof(byte[]).FullName, NpgsqlDbType.Bytea },
			{ typeof(bool).FullName, NpgsqlDbType.Bit },
			{ typeof(DateTime).FullName, NpgsqlDbType.Date },
			//{ typeof(DateTimeOffset).FullName, NpgsqlDbType.DateTimeOffset },
			{ typeof(decimal).FullName, NpgsqlDbType.Double }, 
			{ typeof(Guid).FullName, NpgsqlDbType.Uuid }, 
			{ typeof(int).FullName, NpgsqlDbType.Integer },
			{ typeof(string).FullName, NpgsqlDbType.Varchar },
			{ typeof(TimeSpan).FullName, NpgsqlDbType.Interval }
																														 };
		internal NpgSqlCommandData()
			: base(NpgsqlCommandMapper)
		{
		}

		internal NpgSqlCommandData(string connectionString)
			: base(connectionString, NpgsqlCommandMapper)
		{
		}

		internal void AddCommandInformation(NpgsqlCommand command)
		{
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = CommandName;
			command.Parameters.AddRange(Parameters.ToArray());
		}

		public override void AddInputParameter(string parameterName, object value)
		{
			var npgsqlParameter = new NpgsqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				Value = value,
				Direction = ParameterDirection.Input
			};

			if (value != null)
			{
				npgsqlParameter.NpgsqlDbType = this.GetDbType(value);
			}

			Parameters.Add(npgsqlParameter);
		}

		public override void AddInputParameter<T>(string parameterName, T TValue)
		{
			Parameters.Add(new NpgsqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				NpgsqlDbType = this.GetDbType<NpgsqlDbType, T>(),
				Value = TValue,
				Direction = ParameterDirection.Input
			});
		}

		public override void AddOutputParameter<T>(string parameterName, T TValue)
		{
			Parameters.Add(new NpgsqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				NpgsqlDbType = this.GetDbType<NpgsqlDbType, T>(),
				Value = TValue,
				Direction = ParameterDirection.Output
			});
		}

		public override DbConnection CreateDbConnection()
		{
			return new NpgsqlConnection(ConnectionString);
		}
	}
}
