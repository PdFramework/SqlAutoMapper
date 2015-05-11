namespace PeinearyDevelopment.Framework.Data.MySql
{
	using Data;
	using global::MySql.Data.MySqlClient;

	public class MySqlAutoMapper : DbAutoMapper<MySqlDbType>
	{
		#region Properties & Constructors
		public MySqlAutoMapper() : base(new MySqlCommandData()) { }
		public MySqlAutoMapper(string connectionString) : base(new MySqlCommandData(connectionString)) { }
		internal MySqlAutoMapper(MySqlCommandData commandData)
			: base(commandData)
		{
		}
		#endregion
	}
}