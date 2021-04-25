using System;
using System.Text;

namespace LtQuery.ORM.SQL.SqlBuilders
{
    using Definitions;
    using SqlQueries;

    public class SQLiteSqlBuilder : ISqlBuilder
    {
        private readonly ITableDefinitionResolver _tableResolver;

        private const string tableAlias = "t1";
        private const string nullString = "null";
        public SQLiteSqlBuilder(ITableDefinitionResolver tableResolver)
        {
            _tableResolver = tableResolver ?? throw new ArgumentNullException(nameof(tableResolver));
        }


        public string Build<TEntity>(ISqlQuery<TEntity> sqlQuery)
        {
            switch (sqlQuery)
            {
                case Count<TEntity> derivedSqlQuery:
                    return build(derivedSqlQuery);
                case SelectMany<TEntity> derivedSqlQuery:
                    return build(derivedSqlQuery);
                case SelectSingle<TEntity> derivedSqlQuery:
                    return build(derivedSqlQuery);
                case SelectFirst<TEntity> derivedSqlQuery:
                    return build(derivedSqlQuery);
                default:
                    throw new Exception();
            }
        }
        private string build<TEntity>(Count<TEntity> countQuery)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var keyColumn = table.PrimaryKeyColumn;
            var query = countQuery.Query;

            if (query.TakeCount == null && query.SkipCount != null)
                throw new InvalidOperationException("SQLite is not Support (TakeCount == null && SkipCount != null)");

            var strb = new StringBuilder();
            strb.Append("SELECT COUNT(").AppendSql(keyColumn).Append(") ");

            strb.Append(" FROM ").AppendSql(table);

            if (query.Where != null)
                strb.Append(" WHERE ").AppendSql(query.Where);

            if (query.TakeCount != null)
            {
                strb.Append(" LIMIT ").Append(query.TakeCount.Value);
                if (query.SkipCount != null)
                    strb.Append(" OFFSET ").Append(query.SkipCount.Value);
            }

            return strb.ToString();
        }

        private string build<TEntity>(SelectMany<TEntity> selectQuery)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var query = selectQuery.Query;

            if (query.TakeCount == null && query.SkipCount != null)
                throw new InvalidOperationException("SQLite is not Support (TakeCount == null && SkipCount != null)");

            var strb = new StringBuilder();
            strb.Append("SELECT ");
            append(strb, table.Columns);

            strb.Append(" FROM ").AppendSql(table);

            if (query.Where != null)
                strb.Append(" WHERE ").AppendSql(query.Where);

            if (query.TakeCount != null)
            {
                strb.Append(" LIMIT ").Append(query.TakeCount.Value);
                if (query.SkipCount != null)
                    strb.Append(" OFFSET ").Append(query.SkipCount.Value);
            }

            return strb.ToString();
        }

        private string build<TEntity>(SelectSingle<TEntity> selectQuery) => build(selectQuery.Query, 2);

        private string build<TEntity>(SelectFirst<TEntity> selectQuery) => build(selectQuery.Query, 1);

        private string build<TEntity>(Query<TEntity> query, int takeMax)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var strb = new StringBuilder();

            var take = query.TakeCount;
            if (take > takeMax)
                take = takeMax;
            strb.Append("SELECT ");
            append(strb, table.Columns);

            strb.Append(" FROM ").AppendSql(table);

            if (query.Where != null)
                strb.Append(" WHERE ").AppendSql(query.Where);

            strb.Append(" LIMIT ").Append(take ?? takeMax);
            if (query.SkipCount != null)
                strb.Append(" OFFSET ").Append(query.SkipCount.Value);

            return strb.ToString();
        }

        private StringBuilder append<TEntity>(StringBuilder strb, ImmutableList<ColumnDefinition<TEntity>> columns)
        {
            var isFirst = true;
            foreach (var column in columns)
            {
                if (!isFirst)
                    strb.Append(", ");
                strb.AppendSql(column);
                isFirst = false;
            }
            return strb;
        }
    }
}
