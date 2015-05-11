namespace PeinearyDevelopment.Framework.Data.MsSql
{
	using System.Collections.Generic;

	public static class MsSqlAutoMapperExtensions
	{
		public static MsSqlAutoMapper AddCommandInputDataTable<T>(this MsSqlAutoMapper sqlAutoMapper, string parameterName, IEnumerable<T> TValues)
		{
			((MsSqlCommandData)sqlAutoMapper.DbCommandData).AddInputDataTable(parameterName, TValues);
			return sqlAutoMapper;
		}
	}
}
