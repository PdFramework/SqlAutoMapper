namespace PeinearyDevelopment.Framework.Data
{
    using System.Collections.Generic;

    public static class SqlAutoMapperExtensions
    {
        public static SqlAutoMapper AddCommandName(this SqlAutoMapper sqlAutoMapper, string commandName)
        {
            sqlAutoMapper.SqlCommandData.CommandName = commandName;
            return sqlAutoMapper;
        }

        public static SqlAutoMapper AddCommandInputParameter(this SqlAutoMapper sqlAutoMapper, string parameterName, object value)
        {
            sqlAutoMapper.SqlCommandData.AddInputParameter(parameterName, value);
            return sqlAutoMapper;
        }

        public static SqlAutoMapper AddCommandInputParameter<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, T TValue)
        {
            sqlAutoMapper.SqlCommandData.AddInputParameter(parameterName, TValue);
            return sqlAutoMapper;
        }

        public static SqlAutoMapper AddCommandOutputParameter<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, T TValue)
        {
            sqlAutoMapper.SqlCommandData.AddOutputParameter(parameterName, TValue);
            return sqlAutoMapper;
        }

        public static SqlAutoMapper AddCommandInputDataTable<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, IEnumerable<T> TValues)
        {
            sqlAutoMapper.SqlCommandData.AddInputDataTable(parameterName, TValues);
            return sqlAutoMapper;
        }

        public static SqlAutoMapper MapResultSet<T>(this SqlAutoMapper sqlAutoMapper, int resultSetIndex)
        {
            sqlAutoMapper.ResultSetIndexToObjectBuilder.Add(resultSetIndex, new ObjectBuilder<T>());
            return sqlAutoMapper;
        }

        public static SqlAutoMapper MapPropertyName(this SqlAutoMapper sqlAutoMapper, string propertyName, string dbResultColumnName, int resultSetIndex = 0)
        {
            sqlAutoMapper.ResultSetIndexToObjectBuilder[resultSetIndex].OutParameterMapper.Add(propertyName, dbResultColumnName);
            return sqlAutoMapper;
        }
    }
}
