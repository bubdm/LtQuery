namespace LtQuery
{
    using QueryElements;
    using QueryElements.Values;

    public sealed class Query<TEntity> : Immutable<Query<TEntity>>
    {
        public IBoolValue Where { get; }
        public OrderBy<TEntity> OrderBy { get; }
        public int? SkipCount { get; }
        public int? TakeCount { get; }

        public Query(IBoolValue where = null, OrderBy<TEntity> orderBy = null, int? skipCount = null, int? takeCount = null)
        {
            Where = where;
            OrderBy = orderBy;
            SkipCount = skipCount;
            TakeCount = takeCount;
        }

        protected override int CreateHashCode()
        {
            var code = 0;
            AddHashCode(ref code, Where);
            AddHashCode(ref code, OrderBy);
            AddHashCode(ref code, SkipCount);
            AddHashCode(ref code, TakeCount);
            return code;
        }

        public override bool Equals(Query<TEntity> other)
        {
            if (other == null)
                return false;
            if (!Equals(Where, other.Where))
                return false;
            if (!Equals(OrderBy, other.OrderBy))
                return false;
            if (SkipCount != other.SkipCount)
                return false;
            if (SkipCount != other.SkipCount)
                return false;
            return true;
        }
    }
}
