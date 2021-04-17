using System;

namespace LtQuery.ORM
{
    using Definitions;

    public interface ITableDefinitionRegistrator
    {
        void Register<TEntity>(Func<TableDefinition<TEntity>> factory);
    }
}
