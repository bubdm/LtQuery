using System;

namespace LtQuery.QueryElements
{
    public class Property<TEntity> : Immutable<Property<TEntity>>, IProperty
    {
        public string Name { get; }
        public Property(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }


        protected override int CreateHashCode() => Name.GetHashCode();
        public override bool Equals(Property<TEntity> other) => Name == other.Name;
    }
}
