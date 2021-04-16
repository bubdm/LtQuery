namespace LtQuery.ORM
{
    using Definitions;

    public interface ITableDefinitionResolver
    {
        TableDefinition<TEntity> Resolve<TEntity>();
    }
}
