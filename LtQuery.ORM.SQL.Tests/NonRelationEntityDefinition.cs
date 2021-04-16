using System.Collections.Generic;

namespace LtQuery.ORM.SQL.Tests
{
    using Definitions;

    class NonRelationEntityDefinition : TableDefinition<NonRelationEntity>
    {
        public override IEnumerable<ColumnDefinition<NonRelationEntity>> CreateColumn()
        {
            yield return new ColumnDefinition<NonRelationEntity>(nameof(NonRelationEntity.Id));
            yield return new ColumnDefinition<NonRelationEntity>(nameof(NonRelationEntity.Code));
            yield return new ColumnDefinition<NonRelationEntity>(nameof(NonRelationEntity.Name));
        }
    }
}
