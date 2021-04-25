using System;

namespace LtQuery.ORM.SQL.SqlQueries
{
    public class SelectSingle<TEntity> : Immutable<SelectSingle<TEntity>>, ISqlQuery<TEntity>
    {
        public Query<TEntity> Query { get; }
        public SelectSingle(Query<TEntity> query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        protected override int CreateHashCode() => Query.GetHashCode();
        public override bool Equals(SelectSingle<TEntity> other) => Equals(Query, other.Query);
    }
}
