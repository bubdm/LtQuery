using System;
using System.Linq.Expressions;

namespace LtQuery.ORM.SQL.Readers
{
    using Definitions;

    class ColumnReader<TEntity, TProperty> : IColumnReader<TEntity>
    {
        private readonly ColumnDefinition<TEntity> _definition;
        private readonly Action<TEntity, TProperty> _setter;
        public ColumnReader(ColumnDefinition<TEntity> definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _setter = createSetter();
        }

        public string Name => _definition.Name;

        private Action<TEntity, TProperty> createSetter()
        {
            var param1 = Expression.Parameter(typeof(TEntity));
            var param2 = Expression.Parameter(typeof(TProperty));
            var exp = Expression.Lambda<Action<TEntity, TProperty>>(
                Expression.Assign(Expression.Property(param1, Name),
                param2),
                param1, param2);
            return exp.Compile();
        }
        public void SetValue(TEntity entity, object value) => _setter(entity, (TProperty)value);
    }
}