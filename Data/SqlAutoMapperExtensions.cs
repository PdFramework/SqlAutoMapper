namespace PeinearyDevelopment.Framework.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using ObjectBuilder;

	public static class SqlAutoMapperExtensions
	{
		#region CommandExtensions
		public static SqlAutoMapper AddCommandName(this SqlAutoMapper sqlAutoMapper, string commandName)
		{
			sqlAutoMapper.SqlCommandData.CommandName = commandName;
			return sqlAutoMapper;
		}

		public static SqlAutoMapper AddCommandInputParameter(this SqlAutoMapper sqlAutoMapper, string parameterName, object value)
		{
			sqlAutoMapper.SqlCommandData.AddInputParameter(parameterName, value);
			return sqlAutoMapper;
		}

		public static SqlAutoMapper AddCommandInputParameter<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, T TValue)
		{
			sqlAutoMapper.SqlCommandData.AddInputParameter(parameterName, TValue);
			return sqlAutoMapper;
		}

		public static SqlAutoMapper AddCommandOutputParameter<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, T TValue)
		{
			sqlAutoMapper.SqlCommandData.AddOutputParameter(parameterName, TValue);
			return sqlAutoMapper;
		}

		public static SqlAutoMapper AddCommandInputDataTable<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, IEnumerable<T> TValues)
		{
			sqlAutoMapper.SqlCommandData.AddInputDataTable(parameterName, TValues);
			return sqlAutoMapper;
		}
		#endregion

		#region MappingExtensions
		public static SqlAutoMapper MapResultSet<T>(this SqlAutoMapper sqlAutoMapper, int resultSetIndex)
		{
			sqlAutoMapper.ResultSetIndexToObjectBuilder.Add(resultSetIndex, new ObjectBuilder<T>());
			return sqlAutoMapper;
		}

		public static SqlAutoMapper MapPropertyName(this SqlAutoMapper sqlAutoMapper, string propertyName, string dbResultColumnName, int resultSetIndex = 0)
		{
			sqlAutoMapper.ResultSetIndexToObjectBuilder[resultSetIndex].OutParameterMapper.Add(propertyName, dbResultColumnName);
			return sqlAutoMapper;
		}
		#endregion

		internal static void ExecuteWithoutResultsExtension(this SqlAutoMapper sqlAutoMapper)
		{
			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);
					command.ExecuteNonQuery();
				}
			}
		}

		internal static T ExecuteWithValueResultExtension<T>(this SqlAutoMapper sqlAutoMapper)
		{
			var value = default(T);

			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);
					using (var reader = command.ExecuteReader())
					{
						reader.Read();
						if (!reader.IsDBNull(0))
						{
							value = (T)reader[0];
						}
					}
				}
			}

			return value;
		}

		internal static IEnumerable<T> ExecuteWithValueResultsExtension<T>(this SqlAutoMapper sqlAutoMapper)
		{
			var values = new List<T>();
			var defaultT = default(T);
			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							if (ValueTypeHelper.IsNullable<T>() && reader.IsDBNull(0))
							{
								values.Add(defaultT);
							}
							else if (!reader.IsDBNull(0))
							{
								values.Add((T)reader[0]);
							}
							else
							{
								throw new ArgumentNullException(string.Format("Database is returning a null value. Object of type \"{0}\" can't be assigned a null value.", typeof(T).FullName));
							}
						}
					}
				}
			}

			return values;
		}

		internal static T ExecuteWithResultExtension<T>(this SqlAutoMapper sqlAutoMapper) where T : class
		{
			sqlAutoMapper.ResultSetIndexToObjectBuilder.Add(0, new ObjectBuilder<T>());

			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = command.ExecuteReader())
					{
						reader.Read();

						if (reader.HasRows)
						{
							var objectBuilder = sqlAutoMapper.ResultSetIndexToObjectBuilder[0] as ObjectBuilder<T>;
							objectBuilder.PopulateReaderToObjectMapper(reader);
							objectBuilder.CreatePropertyValue();
							return (T)objectBuilder.CreateMappedObject(objectBuilder.MappedProperties);
						}

						return null;
					}
				}
			}
		}

		internal static IEnumerable<T> ExecuteWithResultsExtension<T>(this SqlAutoMapper sqlAutoMapper) where T : class
		{
			sqlAutoMapper.ResultSetIndexToObjectBuilder.Add(0, new ObjectBuilder<T>());

			var objs = new List<T>();
			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = command.ExecuteReader())
					{
						var objectBuilder = sqlAutoMapper.ResultSetIndexToObjectBuilder[0] as ObjectBuilder<T>;
						objectBuilder.PopulateReaderToObjectMapper(reader);
						objectBuilder.CreatePropertyValue();

						while (reader.Read())
						{
							if (reader.HasRows)
							{

								objs.Add((T)objectBuilder.CreateMappedObject(objectBuilder.MappedProperties));
							}
							else
							{
								objs.Add(null);
							}
						}
					}
				}
			}

			return objs;
		}

		//public IEnumerable<object>[] ExecuteWithResults()
		//{
		//    return ExecuteWithResultsHelper().Result.ToArray();
		//}

		//internal async Task<IEnumerable<IEnumerable<object>>> ExecuteWithResultsHelper()
		//{
		//    var objs = new List<IEnumerable<object>>();

		//    using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
		//    {
		//        await connection.OpenAsync();

		//        using (var command = connection.CreateCommand())
		//        {
		//            SqlCommandData.AddCommandInformation(command);

		//            using (var reader = await command.ExecuteReaderAsync())
		//            {
		//                var index = 0;
		//                var numProcessed = 0;

		//                objs.Add(GetObjectSet(reader, ResultSetIndexToObjectBuilder[index]).Result);
		//                index++;
		//                numProcessed++;

		//                while (await reader.NextResultAsync() && numProcessed <= ResultSetIndexToObjectBuilder.Count)
		//                {
		//                    if (ResultSetIndexToObjectBuilder.ContainsKey(index))
		//                    {
		//                        objs.Add(GetObjectSet(reader, ResultSetIndexToObjectBuilder[index]).Result);
		//                        numProcessed++;
		//                    }

		//                    index++;
		//                }
		//            }
		//        }
		//    }

		//    return objs;
		//}

		//internal async Task<IEnumerable<object>> GetObjectSet(SqlDataReader reader, ObjectBuilderBase resultSetIndexToObjectBuilder)
		//{
		//    var temp = new List<object>();
		//    resultSetIndexToObjectBuilder.PopulateReaderToObjectMapper(reader);
		//    resultSetIndexToObjectBuilder.CreatePropertyValue();
		//    while (await reader.ReadAsync())
		//    {
		//        temp.Add(resultSetIndexToObjectBuilder.CreateMappedObject(resultSetIndexToObjectBuilder.MappedProperties));
		//    }

		//    return temp;
		//}
	}
}
