using System;
using System.Collections.Generic;
using System.Text;

namespace AutoOption.Database.Interface
{
    public interface IDatabaseWrapper
    {
        public IConnectionWrapper CreateConnection();
        public ICommandWrapper CreateCommand(string query);
    }
}
