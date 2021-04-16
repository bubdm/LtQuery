namespace LtQuery.ORM.SQL.Readers
{
    interface IColumnReader<in TEntity>
    {
        void SetValue(TEntity entity, object value);
    }
}
