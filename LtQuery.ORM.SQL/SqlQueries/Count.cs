using System;

namespace LtQuery.ORM.SQL.SqlQueries
{
    public class Count<TEntity> : Immutable<Count<TEntity>>, ISqlQuery<TEntity>
    {
        public Query<TEntity> Query { get; }
        public Count(Query<TEntity> query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        protected override int CreateHashCode() => Query.GetHashCode();
        public override bool Equals(Count<TEntity> other) => Equals(Query, other.Query);

    }
}
