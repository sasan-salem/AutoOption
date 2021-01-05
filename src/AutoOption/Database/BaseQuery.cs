using AutoOption.Database.Interface;
using System.Collections.Generic;

namespace AutoOption.Database
{
    internal class BaseQuery
    {
        private readonly IDatabaseWrapper dbWrapper;

        internal BaseQuery(IDatabaseWrapper dbWrapper)
        {
            this.dbWrapper = dbWrapper;
        }

        internal void Execute(string query)
        {
            using IConnectionWrapper conn = dbWrapper.CreateConnection();
            conn.Open();
            using ICommandWrapper comm = dbWrapper.CreateCommand(query.ToString());
            comm.ExecuteNonQuery();
        }

        internal List<T> ExecuteAndReadOneColumn<T>(string query)
        {
            using IConnectionWrapper conn = dbWrapper.CreateConnection();
            conn.Open();
            using ICommandWrapper comm = dbWrapper.CreateCommand(query);
            var entities = comm.ExecuteReaderOneColumn<T>(0);
            return entities;
        }

        internal List<T> ExecuteAndReadEntity<T>(string query) where T : new()
        {
            using IConnectionWrapper conn = dbWrapper.CreateConnection();
            conn.Open();
            using ICommandWrapper comm = dbWrapper.CreateCommand(query);
            var entities = comm.ExecuteReaderEntity<T>();
            return entities;
        }
    }
}
