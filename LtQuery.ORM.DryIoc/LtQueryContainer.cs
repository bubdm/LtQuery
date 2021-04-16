using DryIoc;
using System;

namespace LtQuery.ORM.DryIoc
{
    using Definitions;

    class LtQueryContainer : ITableDefinitionRegistrator, ITableDefinitionResolver
    {
        private readonly Container _container;
        public LtQueryContainer(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Register<TEntity>(Func<TableDefinition<TEntity>> factory)
            => _container.RegisterDelegate(factory, reuse: Reuse.Singleton);

        public TableDefinition<TEntity> Resolve<TEntity>() => _container.Resolve<TableDefinition<TEntity>>();
    }
}
