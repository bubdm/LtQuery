namespace LtQuery.Mutables
{
    using QueryElements;

    public interface IValueFluent : IValue
    {
        IValue ToImmutable();
    }
}
