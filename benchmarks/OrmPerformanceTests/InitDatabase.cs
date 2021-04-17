using Microsoft.EntityFrameworkCore;
using System;

namespace OrmPerformanceTests
{
    using EFCore;

    class InitDatabase
    {
        private readonly RandomEx _random = new RandomEx(1234);

        private int next() => _random.Next();
        private double nextDouble() => _random.NextDouble();
        private char nextChar() => _random.NextChar();
        private string nextString() => _random.NextString();
        private DateTime nextDateTime() => _random.NextDateTime();
        //private Vector3 nextVector3() => _random.NextVector3();
        private int nextEntityId<TEntity>(DbContext context) where TEntity : class => _random.NextEntityId<TEntity>(context);
        private TEntity nextEntity<TEntity>(DbContext context) where TEntity : class => _random.NextEntity<TEntity>(context);



        public void Init()
        {
            using (var connection = new SqlConnectionFactory().Create())
            using (var context = new TestContext(connection))
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    // TestEntity
                    var count = Constants.TotalCount;
                    for (var i = 0; i < count; i++)
                    {
                        var test = new TestEntity();
                        test.Code = next();
                        test.Code2 = next();
                        test.Name = nextString();
                        test.Date = nextDateTime();
                        context.Set<TestEntity>().Add(test);
                    }
                    context.SaveChanges();
                    context.Dispose();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.StackTrace);
                    tran.Rollback();
                }
            }
        }
    }
}
