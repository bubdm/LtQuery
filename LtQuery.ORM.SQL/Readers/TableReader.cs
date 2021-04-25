using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace LtQuery.ORM.SQL.Readers
{
    using Definitions;

    class TableReader<TEntity> : ITableReader<TEntity>
    {
        public TableDefinition<TEntity> Definition { get; }
        private readonly IColumnReader<TEntity>[] _columns;
        private readonly Func<TEntity> _createEntityFunc;
        public TableReader(TableDefinition<TEntity> definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _columns = definition.Columns.Select(_ => createColumn(_)).ToArray();
            _createEntityFunc = createCreateEntityFunc();
        }

        private IColumnReader<TEntity> createColumn(ColumnDefinition<TEntity> column)
        {
            var property = Definition.EntityType.GetProperty(column.Name);
            var propertyType = property.PropertyType;

            var type = typeof(ColumnReader<,>).MakeGenericType(typeof(TEntity), propertyType);

            return (IColumnReader<TEntity>)Activator.CreateInstance(type, column);
        }
        private Func<TEntity> createCreateEntityFunc()
        {
            var exp = Expression.Lambda<Func<TEntity>>(Expression.New(Definition.EntityType.GetConstructor(new Type[] { })));
            return exp.Compile();
        }

        public int ColumnCount => _columns.Length;

        public TEntity Read(IDataReader reader, int startIndex = 0)
        {
            var entity = _createEntityFunc();
            for (var i = 0; i < _columns.Length; i++)
            {
                var column = _columns[i];
                column.SetValue(entity, reader[startIndex + i]);
            }
            return entity;
        }
    }
}
