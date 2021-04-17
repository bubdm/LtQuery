using System;

namespace LtQuery.QueryElements
{
    public sealed class OrderBy<TEntity> : Immutable<OrderBy<TEntity>>
    {
        public Property<TEntity> Column { get; }
        public OrderDirect Direct { get; }
        public OrderBy<TEntity> Then { get; }
        public OrderBy(Property<TEntity> column, OrderDirect? direct = null, OrderBy<TEntity> then = null)
        {
            Column = column ?? throw new ArgumentNullException(nameof(column));
            Direct = direct ?? OrderDirect.Asc;
            Then = then;
        }

        protected override int CreateHashCode()
        {
            var code = 0;
            AddHashCode(ref code, Column);
            AddHashCode(ref code, Direct);
            if (Then != null)
                AddHashCode(ref code, Then);
            return code;
        }

        public override bool Equals(OrderBy<TEntity> other)
        {
            if (other == null)
                return false;
            if (!Equals(Column, other.Column))
                return false;
            if (Direct != other.Direct)
                return false;
            if (!Equals(Then, other.Then))
                return false;
            return Direct == other.Direct;
        }
    }
}
