namespace PeinearyDevelopment.Framework.Data.MySql
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Globalization;
	using global::MySql.Data.MySqlClient;

	internal class MySqlCommandData : DbCommandData<MySqlDbType>
	{
		//https://github.com/npgsql/npgsql/wiki/User-Manual#user-content-supported-data-types
		private static readonly Dictionary<string, MySqlDbType> NpgsqlCommandMapper = new Dictionary<string, MySqlDbType> {
			{ typeof(byte[]).FullName, MySqlDbType.Binary },
			{ typeof(bool).FullName, MySqlDbType.Bit },
			{ typeof(DateTime).FullName, MySqlDbType.DateTime },
			//{ typeof(DateTimeOffset).FullName, MySqlDbType.DateTimeOffset },
			{ typeof(decimal).FullName, MySqlDbType.Decimal }, 
			{ typeof(Guid).FullName, MySqlDbType.Guid }, 
			{ typeof(int).FullName, MySqlDbType.Int32 },
			{ typeof(string).FullName, MySqlDbType.VarChar },
			{ typeof(TimeSpan).FullName, MySqlDbType.Time }
																														 };
		internal MySqlCommandData()
			: base(NpgsqlCommandMapper)
		{
		}

		internal MySqlCommandData(string connectionString)
			: base(connectionString, NpgsqlCommandMapper)
		{
		}

		internal void AddCommandInformation(MySqlCommand command)
		{
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = CommandName;
			command.Parameters.AddRange(Parameters.ToArray());
		}

		public override void AddInputParameter(string parameterName, object value)
		{
			var npgsqlParameter = new MySqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				Value = value,
				Direction = ParameterDirection.Input
			};

			if (value != null)
			{
				npgsqlParameter.MySqlDbType = this.GetDbType(value);
			}

			Parameters.Add(npgsqlParameter);
		}

		public override void AddInputParameter<T>(string parameterName, T TValue)
		{
			Parameters.Add(new MySqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				MySqlDbType = this.GetDbType<MySqlDbType, T>(),
				Value = TValue,
				Direction = ParameterDirection.Input
			});
		}

		public override void AddOutputParameter<T>(string parameterName, T TValue)
		{
			Parameters.Add(new MySqlParameter
			{
				ParameterName = string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName),
				MySqlDbType = this.GetDbType<MySqlDbType, T>(),
				Value = TValue,
				Direction = ParameterDirection.Output
			});
		}

		public override DbConnection CreateDbConnection()
		{
			return new MySqlConnection(ConnectionString);
		}
	}
}
