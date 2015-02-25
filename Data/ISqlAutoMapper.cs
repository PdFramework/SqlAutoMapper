namespace PeinearyDevelopment.Framework.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ISqlAutoMapper
	{
		void ExecuteWithoutResults();
		Task ExecuteWithoutResultsAsync();
		T ExecuteWithValueResult<T>();
		Task<T> ExecuteWithValueResultAsync<T>();
		IEnumerable<T> ExecuteWithValueResults<T>();
		Task<IEnumerable<T>> ExecuteWithValueResultsAsync<T>();
		T ExecuteWithResult<T>() where T : class;
		Task<T> ExecuteWithResultAsync<T>() where T : class;
		IEnumerable<T> ExecuteWithResults<T>() where T : class;
		Task<IEnumerable<T>> ExecuteWithResultsAsync<T>() where T : class;
	}
}
