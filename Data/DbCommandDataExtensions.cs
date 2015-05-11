namespace PeinearyDevelopment.Framework.Data
{
	using System;

	internal static class DbCommandDataExtensions
	{
		internal static void AddInputParameter<TEnum>(this DbCommandData<TEnum> dbCommandData, string parameterName, object value) where TEnum : struct, IConvertible
		{
			dbCommandData.AddInputParameter(parameterName, value);
		}

		internal static void AddInputParameter<TEnum, T>(this DbCommandData<TEnum> dbCommandData, string parameterName, T TValue) where TEnum : struct, IConvertible
		{
			dbCommandData.AddInputParameter(parameterName, TValue);
		}

		internal static void AddOutputParameter<TEnum, T>(this DbCommandData<TEnum> dbCommandData, string parameterName, T TValue) where TEnum : struct, IConvertible
		{
			dbCommandData.AddOutputParameter(parameterName, TValue);
		}

		internal static TEnum GetDbType<TEnum>(this DbCommandData<TEnum> dbCommandData, object value) where TEnum : struct, IConvertible
		{
			var type = value.GetType();
			return CreateDbEnumFromObjectType(dbCommandData, type);
		}

		internal static TEnum GetDbType<TEnum, T>(this DbCommandData<TEnum> dbCommandData) where TEnum : struct, IConvertible
		{
			var type = typeof(T);
			return CreateDbEnumFromObjectType(dbCommandData, type);
		}

		private static TEnum CreateDbEnumFromObjectType<TEnum>(DbCommandData<TEnum> dbCommandData, Type type) where TEnum : struct, IConvertible
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("TDbEnumType must be an enumerated type");
			}

			var propertyType = Nullable.GetUnderlyingType(type) ?? type;
			return dbCommandData.CommandMapper[propertyType.FullName];
		}
	}
}
