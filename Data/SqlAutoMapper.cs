namespace PeinearyDevelopment.Framework.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using ObjectBuilder;

	public class SqlAutoMapper : ISqlAutoMapper
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
			this.ExecuteWithoutResultsExtension();
		}

		public async Task ExecuteWithoutResultsAsync()
		{
			await this.ExecuteWithoutResultsAsyncExtension();
		}
		#endregion

		#region ExecuteWithValueResult(s)
		public T ExecuteWithValueResult<T>()
		{
			return this.ExecuteWithValueResultExtension<T>();
		}

		public async Task<T> ExecuteWithValueResultAsync<T>()
		{
			return await this.ExecuteWithValueResultAsyncExtension<T>();
		}

		public IEnumerable<T> ExecuteWithValueResults<T>()
		{
			return this.ExecuteWithValueResultsExtension<T>();
		}

		public async Task<IEnumerable<T>> ExecuteWithValueResultsAsync<T>()
		{
			return await this.ExecuteWithValueResultsAsyncExtension<T>();
		}
		#endregion

		#region ExecuteWithResult(s)
		public T ExecuteWithResult<T>() where T : class
		{
			return this.ExecuteWithResultExtension<T>();
		}

		public async Task<T> ExecuteWithResultAsync<T>() where T : class
		{
			return await this.ExecuteWithResultAsyncExtension<T>();
		}

		public IEnumerable<T> ExecuteWithResults<T>() where T : class
		{
			return this.ExecuteWithResultsExtension<T>();
		}

		public async Task<IEnumerable<T>> ExecuteWithResultsAsync<T>() where T : class
		{
			return await this.ExecuteWithResultsAsyncExtension<T>();
		}
		#endregion
	}
}