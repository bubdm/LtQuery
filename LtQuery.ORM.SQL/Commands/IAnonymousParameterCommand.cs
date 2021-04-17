using System;
using System.Data;

namespace LtQuery.ORM.SQL.Commands
{
    interface IAnonymousParameterCommand<TEntity> : IDisposable
    {
        void SetParameters(object values);
        IDataReader ExecuteReader();
    }
}
