using System;
using System.Data;

namespace LtQuery.ORM.SQL.Commands
{
    interface ICommand : IDisposable
    {
        void SetParameters(object values);
        IDataReader ExecuteReader();
    }
}
