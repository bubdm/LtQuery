using System;

namespace LtQuery.Mutables
{
    using LtQuery.QueryElements;

    class OrderByFluent<TEntity>
    {
        private readonly string _column;
        private readonly OrderDirect _direct;
        private OrderByFluent<TEntity> _then;

        public OrderByFluent(string column, OrderDirect direct = OrderDirect.Asc)
        {
            _column = column ?? throw new ArgumentNullException(nameof(column));
            _direct = direct;
        }

        public void AddLast(OrderByFluent<TEntity> then)
        {
            var lastThen = this;
            while (lastThen._then != null)
                lastThen = lastThen._then;
            lastThen._then = then;
        }

        public OrderBy<TEntity> ToImmutable()
            => new OrderBy<TEntity>(new Property<TEntity>(_column), _direct, _then?.ToImmutable());
    }
}
