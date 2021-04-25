using System;

namespace LtQuery.ORM.SQL.SqlQueries
{
    public class SelectMany<TEntity> : Immutable<SelectMany<TEntity>>, ISqlQuery<TEntity>
    {
        public Query<TEntity> Query { get; }
        public SelectMany(Query<TEntity> query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        protected override int CreateHashCode() => Query.GetHashCode();
        public override bool Equals(SelectMany<TEntity> other) => Equals(Query, other.Query);
    }
}
