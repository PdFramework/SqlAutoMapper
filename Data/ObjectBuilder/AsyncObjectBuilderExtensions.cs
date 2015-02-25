namespace PeinearyDevelopment.Framework.Data.ObjectBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Threading.Tasks;

	internal static class AsyncObjectBuilderExtensions
	{
		internal static object CreateMappedObject<T>(this AsyncObjectBuilder<T> objBuilder, IEnumerable<Task<object>> objs)
		{
			return objBuilder.ObjectConstructWithAssignments(objs.ToArray());
		}

		internal static void PopulateReaderToObjectMapperAsync<T>(this AsyncObjectBuilder<T> objBuilder, SqlDataReader reader)
		{
			foreach (var property in objBuilder.ObjectProperties)
			{
				var effectivePropertyName = objBuilder.OutParameterMapper.ContainsKey(property.Name) ? objBuilder.OutParameterMapper[property.Name] : property.Name;

				var pair = new KeyValuePair<string, Func<Task<object>>>(effectivePropertyName, null);
				if (reader.HasColumn(effectivePropertyName))
				{
					if (!objBuilder.ReaderToObjectMapper.ContainsKey(effectivePropertyName))
					{
						var propertyOrdinal = reader.GetOrdinal(effectivePropertyName);
						if (propertyOrdinal >= 0)
						{
							pair = new KeyValuePair<string, Func<Task<object>>>(effectivePropertyName, async () => await reader.IsDBNullAsync(propertyOrdinal) ? null : await reader.GetFieldValueAsync<object>(propertyOrdinal));
						}
						else
						{
							pair = new KeyValuePair<string, Func<Task<object>>>(effectivePropertyName, null);
						}
					}
				}

				if (!objBuilder.ReaderToObjectMapper.ContainsKey(effectivePropertyName))
				{
					objBuilder.ReaderToObjectMapper.Add(pair.Key, pair.Value);
				}
			}
		}

		internal static void CreatePropertyValue<T>(this AsyncObjectBuilder<T> objBuilder)
		{
			objBuilder.MappedProperties = objBuilder.ObjectProperties.Select(async property =>
			{
				var effectivePropertyName = objBuilder.OutParameterMapper.ContainsKey(property.Name) ? objBuilder.OutParameterMapper[property.Name] : property.Name;
				return objBuilder.ReaderToObjectMapper[effectivePropertyName] != null ? await objBuilder.ReaderToObjectMapper[effectivePropertyName]() : property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
			});
		}
	}
}
