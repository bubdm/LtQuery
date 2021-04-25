using System;
using System.Collections.Generic;
using System.Data;

namespace LtQuery.ORM.SQL
{
    using ORM;
    using Readers;
    using SqlQueries;

    public class LtConnection : ILtConnection, IDisposable
    {
        private readonly ITableDefinitionResolver _tableResolver;
        private readonly ISqlBuilder _sqlBuilder;
        public IDbConnection SqlConnection { get; }
        public LtConnection(ITableDefinitionResolver tableResolver, ISqlBuilder sqlBuilder, IDbConnection dbConnection)
        {
            _tableResolver = tableResolver ?? throw new ArgumentNullException(nameof(tableResolver));
            _sqlBuilder = sqlBuilder;
            SqlConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        public void Dispose()
        {
            foreach (IDisposable repository in repositories.Values)
                repository.Dispose();
        }

        internal void Open()
        {
            if (SqlConnection.State == ConnectionState.Closed)
                SqlConnection.Open();
        }


        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        private IRepository<TEntity> getRepository<TEntity>()
        {
            var type = typeof(TEntity);
            object repository;
            if (!repositories.TryGetValue(type, out repository))
            {
                repository = createRepository<TEntity>();
                repositories.Add(type, repository);
            }
            return (IRepository<TEntity>)repository;
        }
        private IRepository<TEntity> createRepository<TEntity>()
            => new Repository<TEntity>(this, _sqlBuilder, new TableReader<TEntity>(_tableResolver.Resolve<TEntity>()));


        public int Count<TEntity>(Query<TEntity> query)
            => getRepository<TEntity>().Load(new Count<TEntity>(query));
        public int Count<TEntity, TParameter>(Query<TEntity> query, TParameter values)
            => getRepository<TEntity>().Load(new Count<TEntity>(query), values);

        public IEnumerable<TEntity> Select<TEntity>(Query<TEntity> query)
            => getRepository<TEntity>().Load(new SelectMany<TEntity>(query));
        public IEnumerable<TEntity> Select<TEntity, TParameter>(Query<TEntity> query, TParameter values)
            => getRepository<TEntity>().Load(new SelectMany<TEntity>(query), values);

        public TEntity Single<TEntity>(Query<TEntity> query)
            => getRepository<TEntity>().Load(new SelectSingle<TEntity>(query));
        public TEntity Single<TEntity, TParameter>(Query<TEntity> query, TParameter values)
            => getRepository<TEntity>().Load(new SelectSingle<TEntity>(query), values);

        public TEntity First<TEntity>(Query<TEntity> query)
            => getRepository<TEntity>().Load(new SelectFirst<TEntity>(query));
        public TEntity First<TEntity, TParameter>(Query<TEntity> query, TParameter values)
            => getRepository<TEntity>().Load(new SelectFirst<TEntity>(query), values);
    }
}
