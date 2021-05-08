namespace LtQuery.QueryElements
{
    public sealed class ConstantValue<TProperty> : Immutable<ConstantValue<TProperty>>, IConstantValue
    {
        public TProperty Value { get; }
        public ConstantValue(TProperty value)
        {
            Value = value;
        }

        object IConstantValue.Value => Value;

        protected override int CreateHashCode() => Value.GetHashCode();

        public override bool Equals(ConstantValue<TProperty> other) => Equals(Value, other.Value);
    }
}
