using DryIoc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace LtQuery.ORM.SQL.Tests
{
    using QueryElements;
    using QueryElements.Values;
    using QueryElements.Values.Operators;
    using SqlBuilders;

    public class LtConnectionTests : IDisposable
    {
        private Container _container;
        private readonly LtConnection _connection;
        private Query<NonRelationEntity> _singleQuery;
        private Query<NonRelationEntity> _allQuery;
        private IEnumerable<NonRelationEntity> _entities;
        public LtConnectionTests()
        {
            var rand = new RandomEx(0);

            _container = new Container();

            _container.RegisterDelegate<IDbConnection>(() => new SqlConnectionFactory().Create(), reuse: Reuse.Scoped);

            new DryIoc.Module().Register(_container);
            var tableRegistrator = _container.Resolve<ITableDefinitionRegistrator>();

            tableRegistrator.Register(() => new NonRelationEntityDefinition());


            _entities = new SqlConnectionFactory().GetEntities();

            var tableResolver = _container.Resolve<ITableDefinitionResolver>();
            var dbConnection = new SqlConnectionFactory().Create();
            var sqlBuilder = new SqlServerSqlBuilder(tableResolver);
            _connection = new LtConnection(tableResolver, sqlBuilder, dbConnection);

            _allQuery = new Query<NonRelationEntity>();
            _singleQuery = new Query<NonRelationEntity>(where: new EqualOperator(new Property<NonRelationEntity>(nameof(NonRelationEntity.Id)), new Parameter("Id")));
        }
        public void Dispose()
        {
            _container?.Dispose();
        }

        [Fact]
        public void Count()
        {
            var actual = _connection.Count(_allQuery);
            Assert.Equal(_entities.Count(), actual);
        }

        [Fact]
        public void Query()
        {
            var actual = _connection.Query(_allQuery);
            Assert.Equal(_entities, actual);
        }

        [Fact]
        public void QuerySingle()
        {
            var actual = _connection.QuerySingle(_singleQuery, new { Id = 1 });
            Assert.Equal(_entities.First(), actual);
        }

        [Fact]
        public void QueryFirst()
        {
            var actual = _connection.QueryFirst(_singleQuery, new { Id = 1 });
            Assert.Equal(_entities.First(), actual);
        }
    }
}
