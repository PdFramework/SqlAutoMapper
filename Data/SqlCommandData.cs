namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    internal class SqlCommandData
    {
        internal readonly Dictionary<string, SqlDbType> CommandMapper = new Dictionary<string, SqlDbType> {
                                                                 { typeof(byte[]).FullName, SqlDbType.Binary },
                                                                 { typeof(bool).FullName, SqlDbType.Bit },
                                                                 { typeof(DateTime).FullName, SqlDbType.DateTime2 },
                                                                 { typeof(DateTimeOffset).FullName, SqlDbType.DateTimeOffset },
                                                                 { typeof(decimal).FullName, SqlDbType.Decimal }, 
                                                                 { typeof(Guid).FullName, SqlDbType.UniqueIdentifier }, 
                                                                 { typeof(int).FullName, SqlDbType.Int },
                                                                 { typeof(string).FullName, SqlDbType.NVarChar },
                                                                 { typeof(TimeSpan).FullName, SqlDbType.Time }
                                                             };

        internal string ConnectionString { get; set; }
        internal List<SqlParameter> Parameters { get; set; }
        internal string CommandName { get; set; }

        internal SqlCommandData() : this("DbConnectionString")
        {
        }

        internal SqlCommandData(string connectionString)
        {
            Parameters = new List<SqlParameter>();

            var connectionStringsSetting = ConfigurationManager.ConnectionStrings[connectionString];
            ConnectionString = connectionStringsSetting == null ? null : connectionStringsSetting.ConnectionString;
        }

        internal void AddCommandInformation(SqlCommand command)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = CommandName;
            command.Parameters.AddRange(Parameters.ToArray());
        }
    }
}
