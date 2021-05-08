using DryIoc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace LtQuery.ORM.SQL.Tests
{
    using QueryElements;
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
            var sqlBuilder = new SQLiteSqlBuilder(tableResolver);
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
        public void Select()
        {
            var actual = _connection.Select(_allQuery);
            Assert.Equal(_entities, actual);
        }

        [Fact]
        public void SelectWithTakeCount()
        {
            var count = 10;
            var expected = _entities.Take(count);
            var query = new Query<NonRelationEntity>(takeCount: count);
            var actual = _connection.Select(query);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SelectWithSkipCount()
        {
            var skipCount = 10;
            var expected = _entities.Skip(skipCount);
            var query = new Query<NonRelationEntity>(skipCount: skipCount);
            try
            {
                var actual = _connection.Select(query);
                throw new Exception("SQLite must use TakeCount when With SkipCount");
            }
            catch { }
        }

        [Fact]
        public void SelectWithTakeCountAndSkipCount()
        {
            var takeCount = 10;
            var skipCount = 20;
            var expected = _entities.Skip(skipCount).Take(takeCount);
            var query = new Query<NonRelationEntity>(skipCount: skipCount, takeCount: takeCount);
            var actual = _connection.Select(query);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Single()
        {
            var actual = _connection.Single(_singleQuery, new { Id = 1 });
            Assert.Equal(_entities.Single(_ => _.Id == 1), actual);
        }

        [Fact]
        public void First()
        {
            var actual = _connection.First(_singleQuery, new { Id = 1 });
            Assert.Equal(_entities.First(), actual);
        }
    }
}
