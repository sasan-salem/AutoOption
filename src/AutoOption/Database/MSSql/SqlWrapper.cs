using AutoOption.Database.Interface;

namespace AutoOption.Database.MSSql
{
    public class SqlWrapper : IDatabaseWrapper
    {
        private readonly string connectionString;
        public SqlWrapper(string ConnectionString)
        {
            connectionString = ConnectionString;
        }

        private SqlConnectionWrapper sqlConnectionWrapper;
        public IConnectionWrapper CreateConnection()
        {
            sqlConnectionWrapper = new SqlConnectionWrapper(connectionString);
            return sqlConnectionWrapper;
        }

        public ICommandWrapper CreateCommand(string query)
        {
            return new SqlCommandWrapper(sqlConnectionWrapper, query);
        }
    }
}
