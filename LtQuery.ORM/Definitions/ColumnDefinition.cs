using System;

namespace LtQuery.ORM.Definitions
{
    public class ColumnDefinition<TEntity> : Immutable<ColumnDefinition<TEntity>>, IColumnDefinition
    {
        public string Name { get; }
        public ColumnDefinition(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Type Type => typeof(TEntity);


        protected override int CreateHashCode() => Name.GetHashCode();

        public override bool Equals(ColumnDefinition<TEntity> other) => Name == other.Name;
    }
}
