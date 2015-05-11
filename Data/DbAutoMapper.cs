namespace PeinearyDevelopment.Framework.Data
{
	using ObjectBuilder;
	using System;
	using System.Collections.Generic;
	using System.Data.Common;
	using System.Threading.Tasks;

	public abstract class DbAutoMapper<TEnum> : IDbAutoMapper where TEnum : struct, IConvertible
	{
		internal Dictionary<int, ObjectBuilderBase> ResultSetIndexToObjectBuilder { get; set; }
		internal DbCommandData<TEnum> DbCommandData { get; set; }

		internal DbAutoMapper(DbCommandData<TEnum> commandData)
		{
			DbCommandData = commandData;
			ResultSetIndexToObjectBuilder = new Dictionary<int, ObjectBuilderBase>();
		}

		#region Command methods
		public IDbAutoMapper AddCommandName(string commandName)
		{
			DbCommandData.CommandName = commandName;
			return this;
		}

		public IDbAutoMapper AddCommandInputParameter(string parameterName, object value)
		{
			DbCommandData.AddInputParameter(parameterName, value);
			return this;
		}

		public IDbAutoMapper AddCommandInputParameter<T>(string parameterName, T TValue)
		{
			DbCommandData.AddInputParameter(parameterName, TValue);
			return this;
		}

		public IDbAutoMapper AddCommandOutputParameter<T>(string parameterName, T TValue)
		{
			DbCommandData.AddOutputParameter(parameterName, TValue);
			return this;
		}
		#endregion

		#region ExecuteWithoutResults
		public void ExecuteWithoutResults()
		{
			using (var connection = DbCommandData.CreateDbConnection())
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);
					command.ExecuteNonQuery();
				}
			}
		}

		public async Task ExecuteWithoutResultsAsync()
		{
			using (var connection = DbCommandData.CreateDbConnection())
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);
					await command.ExecuteNonQueryAsync();
				}
			}
		}
		#endregion

		#region ExecuteWithValueResult(s)
		public T ExecuteWithValueResult<T>()
		{
			var value = default(T);

			using (var connection = DbCommandData.CreateDbConnection())
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);
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

		public async Task<T> ExecuteWithValueResultAsync<T>()
		{
			var value = default(T);

			using (var connection = DbCommandData.CreateDbConnection())
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

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

		public IEnumerable<T> ExecuteWithValueResults<T>()
		{
			var values = new List<T>();
			var defaultT = default(T);
			using (var connection = DbCommandData.CreateDbConnection())
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

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
								throw new ArgumentNullException(String.Format("Database is returning a null value. Object of type \"{0}\" can't be assigned a null value.", typeof(T).FullName));
							}
						}
					}
				}
			}

			return values;
		}

		public async Task<IEnumerable<T>> ExecuteWithValueResultsAsync<T>()
		{
			var values = new List<T>();
			var defaultT = default(T);
			using (var connection = DbCommandData.CreateDbConnection())
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

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
								throw new ArgumentNullException(String.Format("Database is returning a null value. Object of type \"{0}\" can't be assigned a null value.", typeof(T).FullName));
							}
						}
					}
				}
			}

			return values;
		}
		#endregion

		#region ExecuteWithResult(s)
		public T ExecuteWithResult<T>() where T : class
		{
			var resultSetIndex = 0;
			T obj;
			ResultSetIndexToObjectBuilder.Add(resultSetIndex, new ObjectBuilder<T>());

			using (var connection = DbCommandData.CreateDbConnection())
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

					using (var reader = command.ExecuteReader())
					{
						var objectBuilder = HydrateObjectBuilder<T>(reader, resultSetIndex);

						reader.Read();

						obj = CreateMappedObject(reader, objectBuilder);
					}
				}
			}

			return obj;
		}

		public async Task<T> ExecuteWithResultAsync<T>() where T : class
		{
			var resultSetIndex = 0;
			T obj;
			ResultSetIndexToObjectBuilder.Add(resultSetIndex, new AsyncObjectBuilder<T>());

			using (var connection = DbCommandData.CreateDbConnection())
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

					using (var reader = await command.ExecuteReaderAsync())
					{
						var objectBuilder = HydrateAsyncObjectBuilder<T>(reader, resultSetIndex);

						await reader.ReadAsync();

						obj = await CreateMappedObject(reader, objectBuilder);
					}
				}
			}

			return obj;
		}

		public IEnumerable<T> ExecuteWithResults<T>() where T : class
		{
			var resultSetIndex = 0;
			var objs = new List<T>();

			ResultSetIndexToObjectBuilder.Add(resultSetIndex, new ObjectBuilder<T>());

			using (var connection = DbCommandData.CreateDbConnection())
			{
				connection.Open();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

					using (var reader = command.ExecuteReader())
					{
						var objectBuilder = HydrateObjectBuilder<T>(reader, resultSetIndex);

						while (reader.Read())
						{
							objs.Add(CreateMappedObject(reader, objectBuilder));
						}
					}
				}
			}

			return objs;
		}

		public async Task<IEnumerable<T>> ExecuteWithResultsAsync<T>() where T : class
		{
			var resultSetIndex = 0;
			var objs = new List<T>();

			ResultSetIndexToObjectBuilder.Add(resultSetIndex, new AsyncObjectBuilder<T>());

			using (var connection = DbCommandData.CreateDbConnection())
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					DbCommandData.AddCommandInformation(command);

					using (var reader = await command.ExecuteReaderAsync())
					{
						var objectBuilder = HydrateAsyncObjectBuilder<T>(reader, resultSetIndex);

						while (await reader.ReadAsync())
						{
							objs.Add(await CreateMappedObject(reader, objectBuilder));
						}
					}
				}
			}

			return objs;
		}
		#endregion

		#region private methods
		private ObjectBuilder<T> HydrateObjectBuilder<T>(DbDataReader reader, int resultSetIndex) where T : class
		{
			var objectBuilder = ResultSetIndexToObjectBuilder[resultSetIndex] as ObjectBuilder<T>;
			objectBuilder.PopulateReaderToObjectMapper(reader);
			objectBuilder.CreatePropertyValue();
			return objectBuilder;
		}

		private AsyncObjectBuilder<T> HydrateAsyncObjectBuilder<T>(DbDataReader reader, int resultSetIndex) where T : class
		{
			var objectBuilder = ResultSetIndexToObjectBuilder[resultSetIndex] as AsyncObjectBuilder<T>;
			objectBuilder.PopulateReaderToObjectMapperAsync(reader);
			objectBuilder.CreatePropertyValue();
			return objectBuilder;
		}

		private static T CreateMappedObject<T>(DbDataReader reader, ObjectBuilder<T> objectBuilder) where T : class
		{
			return reader.HasRows ? (T)objectBuilder.CreateMappedObject(objectBuilder.MappedProperties) : null;
		}

		private static async Task<T> CreateMappedObject<T>(DbDataReader reader, AsyncObjectBuilder<T> objectBuilder) where T : class
		{
			return reader.HasRows ? (T)await objectBuilder.CreateMappedObject(objectBuilder.MappedProperties) : null;
		}
		#endregion
	}
}
