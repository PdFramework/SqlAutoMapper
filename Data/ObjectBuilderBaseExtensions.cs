namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class ObjectBuilderBaseExtensions
    {
        internal static object CreateMappedObject(this ObjectBuilderBase objBuilder, IEnumerable<object> objs)
        {
            return objBuilder.ObjectConstructWithAssignments(objs.ToArray());
        }

        internal static void PopulateReaderToObjectMapper(this ObjectBuilderBase objBuilder, SqlDataReader reader)
        {
            foreach (var property in objBuilder.ObjectProperties)
            {
                var effectivePropertyName = objBuilder.OutParameterMapper.ContainsKey(property.Name) ? objBuilder.OutParameterMapper[property.Name] : property.Name;

                var pair = new KeyValuePair<string, Func<object>>(effectivePropertyName, null);
                if (reader.HasColumn(effectivePropertyName))
                {
                    if (!objBuilder.ReaderToObjectMapper.ContainsKey(effectivePropertyName))
                    {
                        var propertyOrdinal = reader.GetOrdinal(effectivePropertyName);
                        pair = propertyOrdinal >= 0 ? new KeyValuePair<string, Func<object>>(effectivePropertyName,() => reader.IsDBNull(propertyOrdinal) ? null : reader.GetValue(propertyOrdinal)) : new KeyValuePair<string, Func<object>>(effectivePropertyName, null);
                    }
                }

                if (!objBuilder.ReaderToObjectMapper.ContainsKey(effectivePropertyName))
                {
                    objBuilder.ReaderToObjectMapper.Add(pair.Key, pair.Value);
                }
            }
        }

        internal static void PopulateReaderToObjectMapperAsync(this ObjectBuilderBase objBuilder, SqlDataReader reader)
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
                            pair = new KeyValuePair<string, Func<Task<object>>>(effectivePropertyName, async () => await reader.IsDBNullAsync(propertyOrdinal) ? null: await reader.GetFieldValueAsync<object>(propertyOrdinal));
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

        internal static void CreatePropertyValue(this ObjectBuilderBase objBuilder)
        {
            objBuilder.MappedProperties = objBuilder.ObjectProperties.Select(property =>
            {
                var effectivePropertyName = objBuilder.OutParameterMapper.ContainsKey(property.Name) ? objBuilder.OutParameterMapper[property.Name] : property.Name;
                return objBuilder.ReaderToObjectMapper[effectivePropertyName] != null ? objBuilder.ReaderToObjectMapper[effectivePropertyName]() : property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
            });
        }
    }
}
