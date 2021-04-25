using System;
using System.Data;
using System.Reflection;

namespace LtQuery.ORM.SQL.Commands
{
    class AnonymousParameterCommand<TDynamic> : ICommand
    {
        private readonly ICommandParameter<TDynamic>[] _parameters;
        public IDbCommand Inner { get; }
        public AnonymousParameterCommand(IDbConnection connection, string sql)
        {
            Inner = connection.CreateCommand();
            Inner.CommandText = sql;
            _parameters = createColumn();
        }
        public void Dispose()
        {
            Inner.Dispose();
        }
        private ICommandParameter<TDynamic>[] createColumn()
        {
            var type = typeof(TDynamic);
            var properties = type.GetProperties();
            var array = new ICommandParameter<TDynamic>[properties.Length];
            for (var i = 0; i < properties.Length; i++)
            {
                array[i] = createParameter(properties[i]);
            }
            return array;
        }
        private ICommandParameter<TDynamic> createParameter(PropertyInfo property)
        {
            var paramType = typeof(AnonymousParameter<,>).MakeGenericType(typeof(TDynamic), property.PropertyType);
            return (ICommandParameter<TDynamic>)Activator.CreateInstance(paramType, Inner, property.Name);
        }
        public void SetParameters(object values)
        {
            if (!(values is TDynamic))
                throw new ArgumentException("invalid type", nameof(values));
            if (_parameters == null)
                return;
            var anonymousValue = (TDynamic)values;
            for (var i = 0; i < _parameters.Length; i++)
            {
                var parameter = _parameters[i];
                parameter.SetParameter(anonymousValue);
            }
        }
        public IDataReader ExecuteReader() => Inner.ExecuteReader();
    }
}
