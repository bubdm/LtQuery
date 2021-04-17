using System;
using System.Text;

namespace LtQuery.ORM.SQL.SqlBuilders
{
    using SqlQueries;
    using SqlServer;

    public class SqlServerSqlBuilder : ISqlBuilder
    {
        private readonly ITableDefinitionResolver _tableResolver;

        private const string tableAlias = "t1";
        private const string nullString = "null";
        public SqlServerSqlBuilder(ITableDefinitionResolver tableResolver)
        {
            _tableResolver = tableResolver ?? throw new ArgumentNullException(nameof(tableResolver));
        }


        public string Build<TEntity>(ISqlQuery<TEntity> sqlQuery)
        {
            switch (sqlQuery)
            {
                case Select<TEntity> query2:
                    return build(query2);
                case Count<TEntity> query2:
                    return build(query2);
                default:
                    throw new Exception();
            }
        }
        private string build<TEntity>(Count<TEntity> countQuery)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var keyColumn = table.PrimaryKeyColumn;
            var query = countQuery.Query;

            var strb = new StringBuilder();
            strb.Append("SELECT COUNT(").AppendSql(keyColumn).Append(") ");

            strb.Append(" FROM ").AppendSql(table);

            if (query.Where != null)
                strb.Append(" WHERE ").AppendSql(query.Where);

            if (query.SkipCount != null || query.TakeCount != null)
                strb.Append(" OFFSET ").Append(query.SkipCount ?? 0).Append(" ROWS");
            if (query.TakeCount != null)
                strb.Append(" FETCH NEXT ").Append(query.TakeCount.Value).Append(" ROWS ONLY");

            return strb.ToString();
        }

        private string build<TEntity>(Select<TEntity> selectQuery)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var query = selectQuery.Query;


            var strb = new StringBuilder();
            strb.Append("SELECT ");
            var columns = table.Columns;
            var isFirst = true;
            foreach (var column in columns)
            {
                if (!isFirst)
                    strb.Append(", ");
                strb.AppendSql(column);
                isFirst = false;
            }

            strb.Append(" FROM ").AppendSql(table);

            if (query.Where != null)
                strb.Append(" WHERE ").AppendSql(query.Where);

            if (query.SkipCount != null || query.TakeCount != null)
                strb.Append(" OFFSET ").Append(query.SkipCount ?? 0).Append(" ROWS");
            if (query.TakeCount != null)
                strb.Append(" FETCH NEXT ").Append(query.TakeCount.Value).Append(" ROWS ONLY");

            return strb.ToString();
        }
    }
}
