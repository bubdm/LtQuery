using System;

namespace LtQuery.QueryElements
{
    public sealed class Parameter : Immutable<Parameter>, IValue
    {
        public string Name { get; }
        public Parameter(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        protected override int CreateHashCode() => Name.GetHashCode();
        public override bool Equals(Parameter other) => Equals(Name, other.Name);
    }
}
