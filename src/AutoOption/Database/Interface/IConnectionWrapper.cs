using System;

namespace AutoOption.Database.Interface
{
    public interface IConnectionWrapper : IDisposable
    {
        void Open();
    }
}
