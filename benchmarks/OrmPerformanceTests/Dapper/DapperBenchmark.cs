using Dapper;
using System.Data;

namespace OrmPerformanceTests.Dapper
{
    class DapperBenchmark
    {
        private IDbConnection _connection;

        private const string _allSql = "SELECT [Id], [Code], [Code2], [Name], [Date] FROM TestEntity";
        private const string _singleSql = "SELECT TOP (2) [Id], [Code], [Code2], [Name], [Date] FROM TestEntity WHERE Id = @Id";

        public void Setup()
        {
            _connection = new SqlConnectionFactory().Create();

            _connection.Query<TestEntity>(_allSql);
            _connection.QuerySingle<TestEntity>(_singleSql, new { Id = 1 });
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
                var entity = _connection.QuerySingle<TestEntity>(_singleSql, new { Id = 1 });

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
            var entities = _connection.Query<TestEntity>(_allSql);
            foreach (var entity in entities)
            {
                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }

        protected void AddHashCode(ref int code, object value) => code = unchecked((code * 5) ^ value?.GetHashCode() ?? 0);
    }
}
