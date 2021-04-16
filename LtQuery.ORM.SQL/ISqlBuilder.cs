namespace LtQuery.ORM.SQL
{
    public interface ISqlBuilder
    {
        string Build<TEntity>(ISqlQuery<TEntity> query);

    }
}
