using System;

namespace LtQuery.QueryElements.Values.Operators
{
    public sealed class AndOperator : Immutable<AndOperator>, IBoolValue
    {
        public ImmutableList<IValue> Values { get; }
        public AndOperator(ImmutableList<IValue> values)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        protected override int CreateHashCode() => Values.GetHashCode();

        public override bool Equals(AndOperator other) => Equals(Values, other.Values);
    }
}
