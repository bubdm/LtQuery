﻿using DryIoc;
using LtQuery;
using LtQuery.ORM.SQL;
using LtQuery.ORM.SQL.SqlBuilders;
using System.Data;

namespace OrmPerformanceTests.LtQuery
{
    class LtQueryBenchmark
    {
        private Container _container;
        private IDbConnection _dbConnection;
        private ILtConnection _connection;

        private Query<TestEntity> _query;
        private Query<TestEntity> _singleQuery;

        public void Setup()
        {
            _container = new Container();

            _dbConnection = new SqlConnectionFactory().Create();

            new global::LtQuery.ORM.DryIoc.Module().Register(_container);

            var tableRegistrator = _container.Resolve<global::LtQuery.ORM.ITableDefinitionRegistrator>();
            tableRegistrator.Register(() => new Tables.TestEntityDefinition());

            var resolver = _container.Resolve<global::LtQuery.ORM.ITableDefinitionResolver>();
            var sqlBuilder = new SqlServerSqlBuilder(resolver);
            _connection = new LtConnection(resolver, sqlBuilder, _dbConnection);

            _query = Lt.Query<TestEntity>().ToImmutable();
            _singleQuery = Lt.Query<TestEntity>().Where(_ => _.Id == Lt.Arg<int>()).ToImmutable();

            _connection.Select(_query);
            _connection.Single(_singleQuery, new { Id = 1 });
        }
        public void Cleanup()
        {
            _connection?.Dispose();
            _dbConnection?.Dispose();
            _container?.Dispose();
        }

        public int SelectOne()
        {
            var accum = 0;
            //for (var i = 0; i <= 100; i++)
            {
                var entity = _connection.Single(_singleQuery, new { Id = 1 });

                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }

        public int SelectMany()
        {
            var accum = 0;
            return accum;
        }

        public int SelectAll()
        {
            var accum = 0;
            var entities = _connection.Select(_query);
            foreach (var entity in entities)
            {
                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }
        protected void AddHashCode(ref int code, object value) => code = unchecked((code * 5) ^ value?.GetHashCode() ?? 0);
    }
}
