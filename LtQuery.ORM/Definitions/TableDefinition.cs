using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LtQuery.ORM.Definitions
{
    public class TableDefinition<TEntity> : Immutable<TableDefinition<TEntity>>, ITableDefinition
    {
        public ImmutableList<ColumnDefinition<TEntity>> Columns { get; }
        public TableDefinition()
        {
            Columns = new ImmutableList<ColumnDefinition<TEntity>>(CreateColumn());
        }

        public virtual IEnumerable<ColumnDefinition<TEntity>> CreateColumn()
        {
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
            return properties.Select(_ => new ColumnDefinition<TEntity>(_.Name));
        }

        public virtual string Name => EntityType.Name;
        public virtual ColumnDefinition<TEntity> PrimaryKeyColumn => Columns[0];
        //public virtual ColumnDefinition<TEntity> DefaultSortColumn { get; }
        public Type EntityType => typeof(TEntity);

        IReadOnlyList<IColumnDefinition> ITableDefinition.Columns => Columns;
        IColumnDefinition ITableDefinition.PrimaryKeyColumn => PrimaryKeyColumn;
        //IColumnDefinition ITableDefinition.DefaultSortColumn => DefaultSortColumn;

        protected override int CreateHashCode() => Columns.GetHashCode();

        public override bool Equals(TableDefinition<TEntity> other) => Equals(Columns, other.Columns);
    }
}
