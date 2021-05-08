using System;

namespace LtQuery.Mutables
{
    using QueryElements;

    public class QueryFluent<TEntity>
    {
        private IBoolValue _where;
        private OrderByFluent<TEntity> _orderBy;
        private int? _skipCount;
        private int? _takeCount;


        public Query<TEntity> ToImmutable() => new Query<TEntity>((IBoolValue)_where?.ToImmutable(), _orderBy?.ToImmutable(), _skipCount, _takeCount);

        public QueryFluent<TEntity> Where(IBoolValue value)
        {
            switch (_where)
            {
                case null:
                    _where = value;
                    break;
                case AndOperatorFluent op:
                    switch (value)
                    {
                        case AndOperator value2:
                            op.Union(value2);
                            break;
                        case AndOperatorFluent value2:
                            op.Union(value2);
                            break;
                        default:
                            op.AddValue(value);
                            break;
                    }
                    break;
                default:
                    switch (value)
                    {
                        case AndOperator value2:
                            var op = new AndOperatorFluent(_where);
                            op.Union(value2);
                            _where = op;
                            break;
                        case AndOperatorFluent value2:
                            op = new AndOperatorFluent(_where);
                            op.Union(value2);
                            _where = op;
                            break;
                        default:
                            _where = new AndOperatorFluent(_where, value);
                            break;
                    }
                    break;
            }
            return this;
        }
        public QueryFluent<TEntity> OrderBy(string property)
        {
            _orderBy = new OrderByFluent<TEntity>(property, OrderDirect.Asc);
            return this;
        }

        public QueryFluent<TEntity> OrderByDescending(string property)
        {
            _orderBy = new OrderByFluent<TEntity>(property, OrderDirect.Desc);
            return this;
        }

        public QueryFluent<TEntity> ThenBy(string property)
        {
            if (_orderBy == null)
                throw new InvalidOperationException("must invoke ThenBy() after OrderBy()");
            _orderBy.AddLast(new OrderByFluent<TEntity>(property, OrderDirect.Asc));
            return this;
        }

        public QueryFluent<TEntity> ThenByDescending(string property)
        {
            if (_orderBy == null)
                throw new InvalidOperationException("must invoke ThenByDescending() after OrderBy()");
            _orderBy.AddLast(new OrderByFluent<TEntity>(property, OrderDirect.Desc));
            return this;
        }


        public QueryFluent<TEntity> Skip(int count)
        {
            _skipCount = (_skipCount ?? 0) + count;
            return this;
        }
        public QueryFluent<TEntity> Take(int count)
        {
            _takeCount = (_takeCount ?? 0) + count;
            return this;
        }
    }
}
