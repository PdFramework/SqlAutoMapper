namespace PeinearyDevelopment.Framework.Data.Postgres
{
	using NpgsqlTypes;

	public class NpgSqlAutoMapper : DbAutoMapper<NpgsqlDbType>
	{
		#region Properties & Constructors
		public NpgSqlAutoMapper() : base(new NpgSqlCommandData()) { }
		public NpgSqlAutoMapper(string connectionString) : base(new NpgSqlCommandData(connectionString)) { }
		internal NpgSqlAutoMapper(NpgSqlCommandData commandData)
			: base(commandData)
		{
		}
		#endregion
	}
}