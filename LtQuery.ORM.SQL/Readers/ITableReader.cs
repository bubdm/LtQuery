using System.Data;

namespace LtQuery.ORM.SQL.Readers
{
    public interface ITableReader<out TEntity>
    {
        TEntity Read(IDataReader reader, int startIndex = 0);
    }
}
