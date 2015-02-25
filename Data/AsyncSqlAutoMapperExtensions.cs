namespace PeinearyDevelopment.Framework.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data.Common;
	using System.Data.SqlClient;
	using System.Threading.Tasks;
	using ObjectBuilder;

	public static class AsyncSqlAutoMapperExtensions
	{
		internal static async Task ExecuteWithoutResultsAsyncExtension(this SqlAutoMapper sqlAutoMapper)
		{
			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		internal static async Task<T> ExecuteWithValueResultAsyncExtension<T>(this SqlAutoMapper sqlAutoMapper)
		{
			var value = default(T);

			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = await command.ExecuteReaderAsync())
					{
						await reader.ReadAsync();

						if (!await reader.IsDBNullAsync(0))
						{
							value = await reader.GetFieldValueAsync<T>(0);
						}
					}
				}
			}

			return value;
		}

		internal static async Task<IEnumerable<T>> ExecuteWithValueResultsAsyncExtension<T>(this SqlAutoMapper sqlAutoMapper)
		{
			var values = new List<T>();
			var defaultT = default(T);
			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							if (ValueTypeHelper.IsNullable<T>() && await reader.IsDBNullAsync(0))
							{
								values.Add(defaultT);
							}
							else if (!await reader.IsDBNullAsync(0))
							{
								values.Add(await reader.GetFieldValueAsync<T>(0));
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

		internal static async Task<T> ExecuteWithResultAsyncExtension<T>(this SqlAutoMapper sqlAutoMapper) where T : class
		{
			T obj;
			sqlAutoMapper.ResultSetIndexToObjectBuilder.Add(0, new AsyncObjectBuilder<T>());

			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = await command.ExecuteReaderAsync())
					{
						var objectBuilder = HydrateAsyncObjectBuilder<T>(sqlAutoMapper, reader);

						await reader.ReadAsync();

						obj = CreateMappedObject(reader, objectBuilder);
					}
				}
			}

			return obj;
		}

		internal static async Task<IEnumerable<T>> ExecuteWithResultsAsyncExtension<T>(this SqlAutoMapper sqlAutoMapper) where T : class
		{
			var objs = new List<T>();

			sqlAutoMapper.ResultSetIndexToObjectBuilder.Add(0, new AsyncObjectBuilder<T>());

			using (var connection = new SqlConnection(sqlAutoMapper.SqlCommandData.ConnectionString))
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					sqlAutoMapper.SqlCommandData.AddCommandInformation(command);

					using (var reader = await command.ExecuteReaderAsync())
					{
						var objectBuilder = HydrateAsyncObjectBuilder<T>(sqlAutoMapper, reader);

						while (await reader.ReadAsync())
						{
							objs.Add(CreateMappedObject(reader, objectBuilder));
						}
					}
				}
			}

			return objs;
		}

		private static AsyncObjectBuilder<T> HydrateAsyncObjectBuilder<T>(SqlAutoMapper sqlAutoMapper, SqlDataReader reader) where T : class
		{
			var objectBuilder = sqlAutoMapper.ResultSetIndexToObjectBuilder[0] as AsyncObjectBuilder<T>;
			objectBuilder.PopulateReaderToObjectMapperAsync(reader);
			objectBuilder.CreatePropertyValue();
			return objectBuilder;
		}

		private static T CreateMappedObject<T>(DbDataReader reader, AsyncObjectBuilder<T> objectBuilder) where T : class
		{
			return reader.HasRows ? (T) objectBuilder.CreateMappedObject(objectBuilder.MappedProperties) : null;
		}
	}
}
