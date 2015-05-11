namespace PeinearyDevelopment.Framework.Data
{
	using ObjectBuilder;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public static class DbAutoMapperExtensions
	{
		public static IDbAutoMapper AddCommandName<TEnum>(this DbAutoMapper<TEnum> dbAutoMapper, string commandName) where TEnum : struct, IConvertible
		{
			dbAutoMapper.DbCommandData.CommandName = commandName;
			return dbAutoMapper;
		}

		public static IDbAutoMapper AddCommandInputParameter<TEnum>(this DbAutoMapper<TEnum> dbAutoMapper, string parameterName, object value) where TEnum : struct, IConvertible
		{
			dbAutoMapper.DbCommandData.AddInputParameter(parameterName, value);
			return dbAutoMapper;
		}

		public static IDbAutoMapper AddCommandInputParameter<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper, string parameterName, T TValue) where TEnum : struct, IConvertible
		{
			dbAutoMapper.DbCommandData.AddInputParameter(parameterName, TValue);
			return dbAutoMapper;
		}

		public static IDbAutoMapper AddCommandOutputParameter<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper, string parameterName, T TValue) where TEnum : struct, IConvertible
		{
			dbAutoMapper.DbCommandData.AddOutputParameter(parameterName, TValue);
			return dbAutoMapper;
		}

		public static IDbAutoMapper MapResultSet<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper, int resultSetIndex) where TEnum : struct, IConvertible
		{
			dbAutoMapper.ResultSetIndexToObjectBuilder.Add(resultSetIndex, new ObjectBuilder<T>());
			return dbAutoMapper;
		}

		public static IDbAutoMapper MapPropertyName<TEnum>(this DbAutoMapper<TEnum> dbAutoMapper, string propertyName, string dbResultColumnName, int resultSetIndex = 0) where TEnum : struct, IConvertible
		{
			dbAutoMapper.ResultSetIndexToObjectBuilder[resultSetIndex].OutParameterMapper.Add(propertyName, dbResultColumnName);
			return dbAutoMapper;
		}

		internal static void ExecuteWithoutResults<TEnum>(this DbAutoMapper<TEnum> dbAutoMapper) where TEnum : struct, IConvertible
		{
			dbAutoMapper.ExecuteWithoutResults();
		}

		internal static T ExecuteWithValueResult<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper) where TEnum : struct, IConvertible
		{
			return dbAutoMapper.ExecuteWithValueResult<T>();
		}

		internal static IEnumerable<T> ExecuteWithValueResults<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper) where TEnum : struct, IConvertible
		{
			return dbAutoMapper.ExecuteWithValueResults<T>();
		}

		internal static T ExecuteWithResult<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper)
			where T : class
			where TEnum : struct, IConvertible
		{
			return dbAutoMapper.ExecuteWithResult<T>();
		}

		internal static IEnumerable<T> ExecuteWithResultsExtension<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper)
			where T : class
			where TEnum : struct, IConvertible
		{
			return dbAutoMapper.ExecuteWithResults<T>();
		}

		internal static async Task ExecuteWithoutResultsAsync<TEnum>(this DbAutoMapper<TEnum> dbAutoMapper) where TEnum : struct, IConvertible
		{
			await dbAutoMapper.ExecuteWithoutResultsAsync();
		}

		internal static async Task<T> ExecuteWithValueResultAsync<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper) where TEnum : struct, IConvertible
		{
			return await dbAutoMapper.ExecuteWithValueResultAsync<T>();
		}

		internal static async Task<IEnumerable<T>> ExecuteWithValueResultsAsync<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper) where TEnum : struct, IConvertible
		{
			return await dbAutoMapper.ExecuteWithValueResultsAsync<T>();
		}

		internal static async Task<T> ExecuteWithResultAsync<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper)
			where T : class
			where TEnum : struct, IConvertible
		{
			return await dbAutoMapper.ExecuteWithResultAsync<T>();
		}

		internal static async Task<IEnumerable<T>> ExecuteWithResultsAsync<TEnum, T>(this DbAutoMapper<TEnum> dbAutoMapper)
			where T : class
			where TEnum : struct, IConvertible
		{
			return await dbAutoMapper.ExecuteWithResultsAsync<T>();
		}
	}
}
