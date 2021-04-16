using LtQuery.ORM.Definitions;
using System.Collections.Generic;

namespace OrmPerformanceTests.LtQuery.Tables
{
    class TestEntityDefinition : TableDefinition<TestEntity>
    {
        public override IEnumerable<ColumnDefinition<TestEntity>> CreateColumn()
        {
            yield return new ColumnDefinition<TestEntity>(nameof(TestEntity.Id));
            yield return new ColumnDefinition<TestEntity>(nameof(TestEntity.Code));
            yield return new ColumnDefinition<TestEntity>(nameof(TestEntity.Code2));
            yield return new ColumnDefinition<TestEntity>(nameof(TestEntity.Name));
            yield return new ColumnDefinition<TestEntity>(nameof(TestEntity.Date));
        }
    }
}
