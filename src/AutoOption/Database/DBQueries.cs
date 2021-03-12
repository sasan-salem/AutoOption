using AutoOption.Database.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoOption.Database
{
    public class DBQueries : IDBQueries
    {
        private readonly string tableName;
        private readonly BaseQuery baseQuery;

        public DBQueries(IDatabaseWrapper dbWrapper, string tableName)
        {
            this.tableName = tableName;
            baseQuery = new BaseQuery(dbWrapper);
        }

        public void GroupAdd(List<OptionEntity> rows)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"INSERT INTO {tableName} ([Key], Display, Type, Value) values");
            foreach (var item in rows)
            {
                query.Append($" ('{item.Key}', '{item.Display}', '{item.Type}', '{item.DefaultValue}'),");
            }
            query.Length--;

            baseQuery.Execute(query.ToString());
        }

        public List<string> GetOneColumn(string columnName)
        {
            string query = $"SELECT {columnName} FROM {tableName}";
            return baseQuery.ExecuteAndReadOneColumn<string>(query);
        }

        public void GroupDeleteByKeys(List<string> Keys, string conditionalColumn)
        {
            StringBuilder deleteIdsQuery = new StringBuilder($"DELETE FROM {tableName} WHERE ");
            foreach (var item in Keys)
            {
                deleteIdsQuery.Append($"{conditionalColumn} = '{item}' OR ");
            }

            deleteIdsQuery.Length -= 4;

            baseQuery.Execute(deleteIdsQuery.ToString());
        }

        public void CreateTable()
        {
            string query = $@"
                CREATE TABLE {tableName}
                (
	                [Key] NVARCHAR(100) NOT NULL PRIMARY KEY,
	                Value NVARCHAR(255),
	                Display NVARCHAR(255) NOT NULL,
                    Type NVARCHAR(255) NOT NULL
                );
            ";

            baseQuery.Execute(query);
        }

        public bool IsTableExist()
        {
            string query = $@"
                SELECT COUNT(*) 
                FROM information_schema.tables 
                WHERE TABLE_NAME = '{tableName}'
            ";

            var isExist = baseQuery.ExecuteAndReadOneColumn<int>(query);
            return (isExist[0] == 1);
        }

        public List<T> GetAll<T>() where T : new()
        {
            string query = $"SELECT * FROM {tableName}";
            return baseQuery.ExecuteAndReadEntity<T>(query);
        }

        public void Update(List<OptionEntity> options)
        {
            StringBuilder query = new StringBuilder();
            foreach (var item in options)
            {
                query.Append($"UPDATE {tableName} SET Value = '{item.Value}' WHERE [Key] = '{item.Key}' ");
            }
            baseQuery.Execute(query.ToString());
        }
    }
}
