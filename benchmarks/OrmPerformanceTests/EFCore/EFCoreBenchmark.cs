using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OrmPerformanceTests.EFCore
{
    class EFCoreBenchmark
    {
        private TestContext _context;

        public void Setup()
        {
            var connection = new SqlConnectionFactory().Create();
            _context = new TestContext(connection);
            _context.Set<TestEntity>().AsNoTracking().ToList();

        }
        public void Cleanup()
        {
            _context.Dispose();
        }

        public int SelectOne()
        {
            var accum = 0;
            //for (var i = 0; i <= 100; i++)
            {
                var entity = _context.Set<TestEntity>().AsNoTracking().Single(_ => _.Id == 1);

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
            var entities = _context.Set<TestEntity>().AsNoTracking();
            foreach (var entity in entities)
            {
                AddHashCode(ref accum, entity.Id);
            }
            return accum;
        }

        protected void AddHashCode(ref int code, object value) => code = unchecked((code * 5) ^ value?.GetHashCode() ?? 0);
    }
}
