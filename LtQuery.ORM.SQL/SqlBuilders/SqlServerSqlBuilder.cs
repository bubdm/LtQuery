﻿using System;
using System.Text;

namespace LtQuery.ORM.SQL.SqlBuilders
{
    using Definitions;
    using SqlQueries;

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

        private string build<TEntity>(SelectMany<TEntity> selectQuery)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var query = selectQuery.Query;


            var strb = new StringBuilder();
            strb.Append("SELECT ");
            append(strb, table.Columns);

            strb.Append(" FROM ").AppendSql(table);

            if (query.Where != null)
                strb.Append(" WHERE ").AppendSql(query.Where);

            if (query.SkipCount != null || query.TakeCount != null)
                strb.Append(" OFFSET ").Append(query.SkipCount ?? 0).Append(" ROWS");
            if (query.TakeCount != null)
                strb.Append(" FETCH NEXT ").Append(query.TakeCount.Value).Append(" ROWS ONLY");

            return strb.ToString();
        }

        private string build<TEntity>(SelectSingle<TEntity> selectQuery) => build(selectQuery.Query, 2);

        private string build<TEntity>(SelectFirst<TEntity> selectQuery) => build(selectQuery.Query, 1);

        private string build<TEntity>(Query<TEntity> query, int takeMax)
        {
            var table = _tableResolver.Resolve<TEntity>();
            var strb = new StringBuilder();

            if (query.SkipCount == null)
            {
                strb.Append("SELECT TOP(").Append(takeMax).Append(") ");
                append(strb, table.Columns);

                strb.Append(" FROM ").AppendSql(table);

                if (query.Where != null)
                    strb.Append(" WHERE ").AppendSql(query.Where);
            }
            else
            {
                var take = query.TakeCount;
                if (take > takeMax)
                    take = takeMax;
                strb.Append("SELECT ");
                append(strb, table.Columns);

                strb.Append(" FROM ").AppendSql(table);

                if (query.Where != null)
                    strb.Append(" WHERE ").AppendSql(query.Where);

                strb.Append(" OFFSET ").Append(query.SkipCount ?? 0).Append(" ROWS");
                strb.Append(" FETCH NEXT ").Append(take ?? 2).Append(" ROWS ONLY");
            }

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
