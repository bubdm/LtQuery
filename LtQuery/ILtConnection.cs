using System;
using System.Collections.Generic;

namespace LtQuery
{
    public interface ILtConnection : IDisposable
    {
        int Count<TEntity>(Query<TEntity> query, object values = null);
        IEnumerable<TEntity> Query<TEntity>(Query<TEntity> query, object values = null);
        TEntity QuerySingle<TEntity>(Query<TEntity> query, object values = null);
        TEntity QueryFirst<TEntity>(Query<TEntity> query, object values = null);
        //object Add<TEntity>(TEntity entity);
        //void Update<TEntity>(TEntity entity);
        //void Remove<TEntity>(object id);
    }
}
