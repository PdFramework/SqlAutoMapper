namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    // http://msmvps.com/blogs/jon_skeet/archive/2008/08/09/making-reflection-fly-and-exploring-delegates.aspx
    public class SqlAutoMapper
    {
        #region Properties & Constructors
        internal Dictionary<int, ObjectBuilderBase> ResultSetIndexToObjectBuilder { get; set; }
        internal SqlCommandData SqlCommandData { get; set; }

        public SqlAutoMapper() : this(new SqlCommandData()) { }

        public SqlAutoMapper(string connectionString) : this(new SqlCommandData(connectionString)) { }

        internal SqlAutoMapper(SqlCommandData commandData)
        {
            SqlCommandData = commandData;
            ResultSetIndexToObjectBuilder = new Dictionary<int, ObjectBuilderBase>();
        }
        #endregion

        #region ExecuteWithoutResults
        public void ExecuteWithoutResults()
        {
            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void ExecuteWithoutResultsAsync()
        {
            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region ExecuteWithValueResult(s)
        public T ExecuteWithValueResult<T>()
        {
            var value = default(T);

            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);
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

            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);
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

        // http://blogs.msdn.com/b/adonet/archive/2012/07/15/using-sqldatareader-s-new-async-methods-in-net-4-5-beta-part-2-examples.aspx
        internal IEnumerable<T> ExecuteWithValueResults<T>()
        {
            var values = new List<T>();
            var defaultT = default(T);
            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);

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

        public async Task<IEnumerable<T>> ExecuteWithValueResultsAsync<T>()
        {
            var values = new List<T>();
            var defaultT = default(T);
            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);

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
        #endregion

        #region ExecuteWithResult(s)
        public T ExecuteWithResult<T>() where  T : class
        {
            ResultSetIndexToObjectBuilder.Add(0, new ObjectBuilder<T>());

            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();

                        if (reader.HasRows)
                        {
                            ResultSetIndexToObjectBuilder[0].PopulateReaderToObjectMapper(reader);
                            ResultSetIndexToObjectBuilder[0].CreatePropertyValue();
                            return (T)ResultSetIndexToObjectBuilder[0].CreateMappedObject(ResultSetIndexToObjectBuilder[0].MappedProperties);
                        }

                        return null;
                    }
                }
            }
        }

        public async Task<T> ExecuteWithResultAsync<T>() where T : class
        {
            ResultSetIndexToObjectBuilder.Add(0, new ObjectBuilder<T>());

            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();

                        if (reader.HasRows)
                        {
                            ResultSetIndexToObjectBuilder[0].PopulateReaderToObjectMapperAsync(reader);
                            ResultSetIndexToObjectBuilder[0].CreatePropertyValue();
                            return (T)ResultSetIndexToObjectBuilder[0].CreateMappedObject(ResultSetIndexToObjectBuilder[0].MappedProperties);
                        }

                        return null;
                    }
                }
            }
        }

        public IEnumerable<T> ExecuteWithResults<T>() where T : class
        {
            ResultSetIndexToObjectBuilder.Add(0, new ObjectBuilder<T>());

            var objs = new List<T>();
            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);

                    using (var reader = command.ExecuteReader())
                    {
                        ResultSetIndexToObjectBuilder[0].PopulateReaderToObjectMapper(reader);
                        ResultSetIndexToObjectBuilder[0].CreatePropertyValue();

                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {

                                objs.Add((T)ResultSetIndexToObjectBuilder[0].CreateMappedObject(ResultSetIndexToObjectBuilder[0].MappedProperties));
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

        public async Task<IEnumerable<T>> ExecuteWithResultsAsync<T>() where T : class
        {
            ResultSetIndexToObjectBuilder.Add(0, new ObjectBuilder<T>());

            var objs = new List<T>();
            using (var connection = new SqlConnection(SqlCommandData.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    SqlCommandData.AddCommandInformation(command);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        ResultSetIndexToObjectBuilder[0].PopulateReaderToObjectMapper(reader);
                        ResultSetIndexToObjectBuilder[0].CreatePropertyValue();

                        while (await reader.ReadAsync())
                        {
                            if (reader.HasRows)
                            {

                                objs.Add((T)ResultSetIndexToObjectBuilder[0].CreateMappedObject(ResultSetIndexToObjectBuilder[0].MappedProperties));
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
        #endregion

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
