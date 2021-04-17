using Dapper;
using System.Data;

namespace OrmPerformanceTests.Dapper
{
    class DapperBenchmark
    {
        private IDbConnection _connection;

        private const string singleSql = "SELECT [Id], [Code], [Code2], [Name], [Date] FROM TestEntity WHERE Id = @Id";
        private const string allSql = "SELECT [Id], [Code], [Code2], [Name], [Date] FROM TestEntity";

        public void Setup()
        {
            _connection = new SqlConnectionFactory().Create();

            _connection.QuerySingle<TestEntity>(singleSql, new { Id = 1 });
            _connection.Query<TestEntity>(allSql);
        }
        public void Cleanup()
        {
            _connection.Dispose();
        }

        public int SelectOne()
        {
            var accum = 0;
            //for (var i = 0; i <= 100; i++)
            {
                var entity = _connection.QuerySingle<TestEntity>(singleSql, new { Id = 1 });

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
            var entities = _connection.Query<TestEntity>(allSql);
            foreach (var entity in entities)
            {
                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }

        protected void AddHashCode(ref int code, object value) => code = unchecked((code * 5) ^ value?.GetHashCode() ?? 0);
    }
}
