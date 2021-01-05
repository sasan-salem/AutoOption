using System;
using System.Collections.Generic;

namespace AutoOption.Database.Interface
{
    public interface ICommandWrapper : IDisposable
    {
        void Query(string query);
        void ExecuteNonQuery();
        List<T> ExecuteReaderOneColumn<T>(int columnId);
        List<T> ExecuteReaderEntity<T>() where T : new();
    }
}
