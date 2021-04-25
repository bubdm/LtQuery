using System;
using System.Collections.Generic;

namespace LtQuery.ORM.SQL
{
    using SqlQueries;

    public interface IRepository<TEntity> : IDisposable
    {
        int Load(Count<TEntity> query);
        int Load<TParameter>(Count<TEntity> query, TParameter values);

        IEnumerable<TEntity> Load(SelectMany<TEntity> query);
        IEnumerable<TEntity> Load<TParameter>(SelectMany<TEntity> query, TParameter values);

        TEntity Load(SelectSingle<TEntity> query);
        TEntity Load<TParameter>(SelectSingle<TEntity> query, TParameter values);

        TEntity Load(SelectFirst<TEntity> query);
        TEntity Load<TParameter>(SelectFirst<TEntity> query, TParameter values);
    }
}
