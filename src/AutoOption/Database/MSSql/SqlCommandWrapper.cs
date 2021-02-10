using AutoOption.Database.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace AutoOption.Database.MSSql
{

    internal class SqlCommandWrapper : ICommandWrapper
    {
        private readonly SqlCommand command;
        private bool disposedValue;

        public SqlCommandWrapper(SqlConnectionWrapper connection, string query)
        {
            command = new SqlCommand(query, connection.sqlConnection);
        }

        public List<T> ExecuteReaderOneColumn<T>(int columnId)
        {
            using SqlDataReader reader = command.ExecuteReader();
            List<T> entities = new List<T>();

            if (reader.HasRows)
                while (reader.Read())
                {
                    entities.Add(reader.GetFieldValue<T>(columnId));
                }

            return entities;
        }
        
        public List<T> ExecuteReaderEntity<T>() where T: new()
        {
            using SqlDataReader reader = command.ExecuteReader();
            Type type = typeof(T);

            PropertyInfo[] peroperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<T> entities = new List<T>();

            var relation = GetRelationPropertyColumn(peroperties, reader);

            if (reader.HasRows)
                while (reader.Read())
                {
                    T entity = new T();
                    for (int i = 0; i < peroperties.Length; i++)
                        peroperties[i].SetValue(entity, GetValue(relation[i], reader));
                    entities.Add(entity);
                }
            return entities;
        }

        public void Query(string query) => command.CommandText = query;

        public void ExecuteNonQuery() => command.ExecuteNonQuery();


        // Private Functions //

        private int[] GetRelationPropertyColumn(PropertyInfo[] peroperties, SqlDataReader reader)
        {
            int[] array = new int[peroperties.Length];
            for (int i = 0; i < peroperties.Length; i++)
                for (int j = 0; j < reader.FieldCount; j++)
                    if (peroperties[i].Name == reader.GetName(j))
                    {
                        array[i] = j;
                        break;
                    }
            return array;
        }

        private object GetValue(int j, SqlDataReader reader)
        {
            if (reader.GetFieldType(j) == typeof(string))
            {
                if (reader.IsDBNull(j))
                    return "";
                else
                    return reader.GetString(j).Trim();
            }
            else
                return reader.GetValue(j);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    command?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
