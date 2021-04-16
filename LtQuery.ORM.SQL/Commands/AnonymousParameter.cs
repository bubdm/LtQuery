using System;
using System.Data;
using System.Linq.Expressions;

namespace LtQuery.ORM.SQL.Commands
{
    class AnonymousParameter<TDynamic, TProperty> : IAnonymousParameter
    {
        public string Name { get; }

        public DbType DbType { get; }

        private readonly Func<TDynamic, TProperty> _getterFunc;
        public AnonymousParameter(string name)
        {
            Name = name;
            DbType = typeof(TProperty).GetDbType();
            _getterFunc = createGetter();
        }
        private Func<TDynamic, TProperty> createGetter()
        {
            var param = Expression.Parameter(typeof(TDynamic));
            var exp = Expression.Lambda<Func<TDynamic, TProperty>>(Expression.Property(param, Name), param);
            return exp.Compile();
        }

        public object GetValue(object values) => _getterFunc((TDynamic)values);
    }
}
