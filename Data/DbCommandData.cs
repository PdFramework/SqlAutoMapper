namespace PeinearyDevelopment.Framework.Data
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;

	internal abstract class DbCommandData<TDbEnumType> : IDbCommandData where TDbEnumType : struct, IConvertible
	{
		private const string DefaultDbConnectionStringAppSettingKey = "DbConnectionString";
		internal IDictionary<string, TDbEnumType> CommandMapper;
		internal string CommandName { get; set; }
		internal string ConnectionString { get; set; }
		internal List<DbParameter> Parameters { get; set; }

		internal DbCommandData()
			: this(DefaultDbConnectionStringAppSettingKey, new Dictionary<string, TDbEnumType>())
		{
		}

		internal DbCommandData(string connectionString)
			: this(connectionString, new Dictionary<string, TDbEnumType>())
		{
		}

		internal DbCommandData(IDictionary<string, TDbEnumType> commandMapper)
			: this(DefaultDbConnectionStringAppSettingKey, commandMapper)
		{
		}

		internal DbCommandData(string connectionString, IDictionary<string, TDbEnumType> commandMapper)
		{
			if (!typeof(TDbEnumType).IsEnum)
			{
				throw new ArgumentException("TDbEnumType must be an enumerated type");
			}

			Parameters = new List<DbParameter>();

			var connectionStringsSetting = ConfigurationManager.ConnectionStrings[connectionString];
			ConnectionString = connectionStringsSetting == null ? null : connectionStringsSetting.ConnectionString;
			CommandMapper = commandMapper;
		}

		internal void AddCommandInformation(DbCommand command)
		{
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = CommandName;
			command.Parameters.AddRange(Parameters.ToArray());
		}

		public abstract void AddInputParameter(string parameterName, object value);
		public abstract void AddInputParameter<T>(string parameterName, T TValue);
		public abstract void AddOutputParameter<T>(string parameterName, T TValue);
		public abstract DbConnection CreateDbConnection();
	}
}
