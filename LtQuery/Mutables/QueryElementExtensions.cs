namespace LtQuery.Mutables
{
    using QueryElements;

    static class QueryElementExtensions
    {
        public static IValue ToImmutable(this IValue _this)
            => _this is IValueFluent ? ((IValueFluent)_this).ToImmutable() : _this;
    }
}
