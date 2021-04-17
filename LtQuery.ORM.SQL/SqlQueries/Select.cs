using System;

namespace LtQuery.ORM.SQL.SqlQueries
{
    public class Select<TEntity> : Immutable<Select<TEntity>>, ISqlQuery<TEntity>
    {
        public Query<TEntity> Query { get; }
        public Select(Query<TEntity> query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        protected override int CreateHashCode() => Query.GetHashCode();
        public override bool Equals(Select<TEntity> other) => Equals(Query, other.Query);
    }
}
