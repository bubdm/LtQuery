using System;
using System.Collections.Generic;

namespace LtQuery.ORM.SQL
{
    using SqlQueries;

    public interface IRepository<TEntity> : IDisposable
    {
        int Count(Count<TEntity> query, object values = null);
        IEnumerable<TEntity> Query(Select<TEntity> query, object values = null);
        TEntity QuerySingle(Select<TEntity> query, object values = null);
        TEntity QueryFirst(Select<TEntity> query, object values = null);
    }
}
