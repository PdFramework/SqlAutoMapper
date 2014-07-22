namespace PeinearyDevelopment.Framework.Data
{
    public static class SqlAutoMapperExtensions
    {
        public static SqlAutoMapper AddCommandName(this SqlAutoMapper sqlAutoMapper, string commandName)
        {
            sqlAutoMapper.SqlCommandData.CommandName = commandName;
            return sqlAutoMapper;
        }

        public static SqlAutoMapper AddCommandInputParameter<T>(this SqlAutoMapper sqlAutoMapper, string parameterName, T TValue)
        {
            sqlAutoMapper.SqlCommandData.AddInputParameter(parameterName, TValue);
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
