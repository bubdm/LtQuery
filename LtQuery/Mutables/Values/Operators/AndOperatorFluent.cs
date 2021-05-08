using System;
using System.Collections.Generic;
using System.Linq;

namespace LtQuery.Mutables
{
    using QueryElements;

    class AndOperatorFluent : IBoolValueFluent
    {
        private readonly List<IValue> _values;
        public AndOperatorFluent(params IValue[] values)
        {
            _values = values.ToList() ?? throw new ArgumentNullException(nameof(values));
        }

        public IReadOnlyList<IValue> Values => _values;
        public void AddValue(IValue value) => _values.Add(value);
        public void Union(AndOperatorFluent other) => _values.AddRange(other._values);
        public void Union(AndOperator other) => _values.AddRange(other.Values);

        public AndOperator ToImmutable()
            => new AndOperator(new ImmutableList<IValue>(_values.Select(_ => _.ToImmutable())));
        IValue IValueFluent.ToImmutable() => ToImmutable();
    }
}
