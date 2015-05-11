namespace PeinearyDevelopment.Framework.Data.MsSql
{
	using System.Data;

	public class MsSqlAutoMapper : DbAutoMapper<SqlDbType>
	{
		#region Properties & Constructors
		public MsSqlAutoMapper()
			: base(new MsSqlCommandData())
		{
		}

		public MsSqlAutoMapper(string connectionString)
			: base(new MsSqlCommandData(connectionString))
		{
		}

		internal MsSqlAutoMapper(MsSqlCommandData commandData)
			: base(commandData)
		{
		}
		#endregion
	}
}