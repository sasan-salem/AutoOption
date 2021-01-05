using System.Collections.Generic;

namespace AutoOption.Database.Interface
{
    public interface IDBQueries
    {
        void CreateTable();
        List<T> GetAll<T>() where T : new();
        List<string> GetOneColumn(string columnName);
        void GroupAdd(Dictionary<string, string> rows);
        void GroupDeleteByKeys(List<string> Keys, string conditionalColumn);
        bool IsTableExist();
        void Update(List<OptionEntity> options);
    }
}