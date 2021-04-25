using BenchmarkDotNet.Attributes;
using Dapper;
using DryIoc;
using LtQuery;
using LtQuery.ORM.SQL;
using LtQuery.ORM.SQL.SqlBuilders;
using LtQuery.QueryElements;
using LtQuery.QueryElements.Values;
using LtQuery.QueryElements.Values.Operators;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OrmPerformanceTests.Benchmarks
{
    using EFCore;

    [Config(typeof(BenchmarkConfig))]
    public class InitialBenchmark : AbstractBenchmark, IBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {

        }

        [GlobalCleanup]
        public void Cleanup()
        {
        }

        [Benchmark]
        public int LtQuery()
        {
            var accum = 0;

            using (var container = new Container())
            {
                new global::LtQuery.ORM.DryIoc.Module().Register(container);

                var tableRegistrator = container.Resolve<global::LtQuery.ORM.ITableDefinitionRegistrator>();
                tableRegistrator.Register(() => new LtQuery.Tables.TestEntityDefinition());

                var tableResolver = container.Resolve<global::LtQuery.ORM.ITableDefinitionResolver>();
                var sqlBuilder = new SqlServerSqlBuilder(tableResolver);
                using (var dbConnection = new SqlConnectionFactory().Create())
                using (var connection = new LtConnection(tableResolver, sqlBuilder, dbConnection))
                {
                    var singleQuery = new Query<TestEntity>(where: new EqualOperator(new Property<TestEntity>(nameof(TestEntity.Id)), new Parameter("Id")));
                    var entity = connection.Single(singleQuery, new { Id = 1 });
                    AddHashCode(ref accum, entity.Id);
                }
                return accum;
            }
        }

        [Benchmark]
        public int Dapper()
        {
            var accum = 0;
            using (var connection = new SqlConnectionFactory().Create())
            {
                var entity = connection.QuerySingle<TestEntity>("SELECT * FROM TestEntity WHERE Id = @Id", new { Id = 1 });
                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }

        [Benchmark]
        public int EFCore()
        {
            var accum = 0;
            using (var connection = new SqlConnectionFactory().Create())
            using (var context = new TestContext(connection))
            {
                var entity = context.Set<TestEntity>().AsNoTracking().Single(_ => _.Id == 1);
                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }
    }
}
