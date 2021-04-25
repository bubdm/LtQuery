﻿using System;

namespace LtQuery.ORM.SQL.SqlQueries
{
    public class SelectFirst<TEntity> : Immutable<SelectFirst<TEntity>>, ISqlQuery<TEntity>
    {
        public Query<TEntity> Query { get; }
        public SelectFirst(Query<TEntity> query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        protected override int CreateHashCode() => Query.GetHashCode();
        public override bool Equals(SelectFirst<TEntity> other) => Equals(Query, other.Query);
    }
}
