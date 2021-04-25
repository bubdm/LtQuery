using System;
using System.Collections.Generic;
using System.Text;

namespace LtQuery.ORM.SQL.SqlBuilders
{
    using Definitions;
    using QueryElements;
    using QueryElements.Values;
    using QueryElements.Values.Operators;

    public static class QueryElementExtensions
    {
        private const string tableAlias = "t1";
        private const string nullString = "null";

        public static StringBuilder AppendSql<TEntity>(this StringBuilder _this, TableDefinition<TEntity> table)
            => _this.Append('[').Append(table.Name).Append("] AS [").Append(tableAlias).Append(']');

        public static StringBuilder AppendSql<TEntity>(this StringBuilder _this, ColumnDefinition<TEntity> column)
            => _this.Append('[').Append(tableAlias).Append("].[").Append(column.Name).Append("]");

        public static StringBuilder AppendSql<TEntity>(this StringBuilder _this, OrderBy<TEntity> orderBy)
        {
            _this.AppendSql(orderBy.Column);
            if (orderBy.Direct == OrderDirect.Desc)
                _this.Append(" DESC");
            return _this;
        }
        public static StringBuilder AppendSql<TEntity>(this StringBuilder _this, IEnumerable<OrderBy<TEntity>> orderBys)
        {
            _this.Append("ORDER BY ");
            var isFirst = true;
            foreach (var orderBy in orderBys)
            {
                if (!isFirst)
                    _this.Append(", ");
                _this.AppendSql(orderBy);
                isFirst = false;
            }
            return _this;
        }
        public static StringBuilder AppendSql(this StringBuilder _this, IValue value)
        {
            switch (value)
            {
                case IProperty value2:
                    return _this.Append('[').Append(tableAlias).Append("].[").Append(value2.Name).Append("]");
                case IConstantValue value2:
                    return _this.Append(value2.Value ?? nullString);
                case Parameter value2:
                    return _this.Append('@').Append(value2.Name);
                case EqualOperator value2:
                    return _this.AppendSql(value2.Left).Append(" = ").AppendSql(value2.Right);
                case AndOperator value2:
                    var isFirst = true;
                    foreach (var subValue in value2.Values)
                    {
                        if (!isFirst)
                            _this.Append(" AND ");
                        _this.AppendSql(subValue);
                        isFirst = false;
                    }
                    return _this;
                default:
                    throw new InvalidOperationException($"[{value.GetType()}]Type can't convert Sql");
            }
        }
    }
}
