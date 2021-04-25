using System;
using System.Data;
using System.Linq.Expressions;

namespace LtQuery.ORM.SQL.Commands
{
    class AnonymousParameter<TDynamic, TProperty> : ICommandParameter<TDynamic>
    {
        public string Name { get; }

        public DbType DbType { get; }
        private readonly IDbDataParameter _inner;

        private readonly Func<TDynamic, TProperty> _getterFunc;
        public AnonymousParameter(IDbCommand dbCommand, string name)
        {
            Name = name;
            DbType = typeof(TProperty).GetDbType();

            _getterFunc = createGetter();
            _inner = createAndRegistDbDataParameter(dbCommand);
        }
        private IDbDataParameter createAndRegistDbDataParameter(IDbCommand dbCommand)
        {
            var commandParameter = dbCommand.CreateParameter();
            commandParameter.ParameterName = $"@{Name}";
            commandParameter.DbType = typeof(TProperty).GetDbType();
            dbCommand.Parameters.Add(commandParameter);
            return commandParameter;
        }
        private Func<TDynamic, TProperty> createGetter()
        {
            var param = Expression.Parameter(typeof(TDynamic));
            var exp = Expression.Lambda<Func<TDynamic, TProperty>>(Expression.Property(param, Name), param);
            return exp.Compile();
        }

        public void SetParameter(TDynamic values) => _inner.Value = _getterFunc(values);
    }
}
