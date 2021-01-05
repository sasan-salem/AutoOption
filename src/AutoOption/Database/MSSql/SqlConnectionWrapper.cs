using AutoOption.Database.Interface;
using System.Data.SqlClient;

namespace AutoOption.Database.MSSql
{
    internal class SqlConnectionWrapper : IConnectionWrapper
    {
        public SqlConnection sqlConnection;
        private bool disposedValue;

        public SqlConnectionWrapper(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            sqlConnection.Open();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sqlConnection.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
