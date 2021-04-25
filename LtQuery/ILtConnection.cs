using System;
using System.Collections.Generic;

namespace LtQuery
{
    public interface ILtConnection : IDisposable
    {
        int Count<TEntity>(Query<TEntity> query);
        int Count<TEntity, TParameter>(Query<TEntity> query, TParameter values);

        IEnumerable<TEntity> Select<TEntity>(Query<TEntity> query);
        IEnumerable<TEntity> Select<TEntity, TParameter>(Query<TEntity> query, TParameter values);

        TEntity Single<TEntity>(Query<TEntity> query);
        TEntity Single<TEntity, TParameter>(Query<TEntity> query, TParameter values);

        TEntity First<TEntity>(Query<TEntity> query);
        TEntity First<TEntity, TParameter>(Query<TEntity> query, TParameter values);



        //object Add<TEntity>(TEntity entity);
        //void Update<TEntity>(TEntity entity);
        //void Remove<TEntity>(object id);
    }
}
