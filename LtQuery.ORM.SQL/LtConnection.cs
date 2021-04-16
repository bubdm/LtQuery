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


        public int Count<TEntity>(Query<TEntity> query, object values = null)
            => getRepository<TEntity>().Count(new Count<TEntity>(query), values);
        public IEnumerable<TEntity> Query<TEntity>(Query<TEntity> query, object values = null)
            => getRepository<TEntity>().Query(new Select<TEntity>(query), values);
        public TEntity QuerySingle<TEntity>(Query<TEntity> query, object values = null)
            => getRepository<TEntity>().QuerySingle(new Select<TEntity>(query), values);
        public TEntity QueryFirst<TEntity>(Query<TEntity> query, object values = null)
            => getRepository<TEntity>().QueryFirst(new Select<TEntity>(query), values);
    }
}
