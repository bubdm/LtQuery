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
            //AddHashCode(ref code, Joins);
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
        //public ImmutableList<C
        //olumn<TEntity>> Projects { get; }

        //public Query<TEntity> Where(IBoolValue condition)
        //{
        //    if (condition == null)
        //        return this;
        //    IBoolValue newCondition;
        //    if (Condition == null)
        //        newCondition = condition;
        //    else
        //        newCondition = new AndOperator(new ImmutableList<IValue>(Condition, condition));
        //    return new Query<TEntity>(Table, newCondition, SortColumns, SkipCount, TakeCount);
        //}
        //public Query<TEntity> OrderBy(Column<TEntity> column)
        //{
        //    if (column == null)
        //        return this;
        //    return new Query<TEntity>(Table, Condition, new ImmutableList<OrderBy<TEntity>>(new OrderBy<TEntity>(column)), SkipCount, TakeCount);
        //}
        //public Query<TEntity> OrderByDescending(Column<TEntity> column)
        //{
        //    if (column == null)
        //        return this;
        //    return new Query<TEntity>(Table, Condition, new ImmutableList<OrderBy<TEntity>>(new OrderBy<TEntity>(column, OrderByDirect.Desc)), SkipCount, TakeCount);
        //}
        //public Query<TEntity> ThenBy(Column<TEntity> column)
        //{
        //    if (column == null)
        //        return this;

        //    var array = new OrderBy<TEntity>[SortColumns?.Count ?? 0 + 1];
        //    var i = 0;
        //    for (i = 0; i < array.Length - 1; i++)
        //        array[i] = SortColumns[i];
        //    array[i] = new OrderBy<TEntity>(column);
        //    var newSortColumns = new ImmutableList<OrderBy<TEntity>>(array);
        //    return new Query<TEntity>(Table, Condition, newSortColumns, SkipCount, TakeCount);
        //}
        //public Query<TEntity> ThenByDescending(Column<TEntity> column)
        //{
        //    if (column == null)
        //        return this;

        //    var array = new OrderBy<TEntity>[SortColumns?.Count ?? 0 + 1];
        //    var i = 0;
        //    for (i = 0; i < array.Length - 1; i++)
        //        array[i] = SortColumns[i];
        //    array[i] = new OrderBy<TEntity>(column, OrderByDirect.Desc);
        //    var newSortColumns = new ImmutableList<OrderBy<TEntity>>(array);
        //    return new Query<TEntity>(Table, Condition, newSortColumns, SkipCount, TakeCount);
        //}
        //public Query<TEntity> Skip(int? count)
        //{
        //    if (count == null)
        //        return this;
        //    return new Query<TEntity>(Table, Condition, SortColumns, SkipCount ?? 0 + count.Value, TakeCount);
        //}
        //public Query<TEntity> Take(int? count)
        //{
        //    if (count == null)
        //        return this;
        //    return new Query<TEntity>(Table, Condition, SortColumns, SkipCount, TakeCount ?? 0 + count.Value);
        //}
    }
}
