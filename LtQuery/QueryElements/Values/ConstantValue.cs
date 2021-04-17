namespace LtQuery.QueryElements.Values
{
    public sealed class ConstantValue<TProperty> : Immutable<ConstantValue<TProperty>>, IValue
    {
        public TProperty Value { get; }
        public ConstantValue(TProperty value)
        {
            Value = value;
        }

        protected override int CreateHashCode() => Value.GetHashCode();

        public override bool Equals(ConstantValue<TProperty> other) => Equals(Value, other.Value);
    }
}
