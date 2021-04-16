using System;
using System.Data;

namespace LtQuery.ORM.SQL.Commands
{
    class AnonymousParameterCommand<TEntity> : IAnonymousParameterCommand<TEntity>
    {
        private readonly IAnonymousParameter[] _parameters;
        public IDbCommand Inner { get; }
        public AnonymousParameterCommand(IDbConnection connection, string sql, object values)
        {
            if (values == null)
                _parameters = null;
            else
                _parameters = createColumn(values);
            Inner = connection.CreateCommand();
            Inner.CommandText = sql;
            initParameters();
        }
        public void Dispose()
        {
            Inner.Dispose();
        }
        private IAnonymousParameter[] createColumn(object values)
        {
            var type = values.GetType();
            var properties = type.GetProperties();
            var array = new IAnonymousParameter[properties.Length];
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var paramType = typeof(AnonymousParameter<,>).MakeGenericType(type, property.PropertyType);
                array[i] = (IAnonymousParameter)Activator.CreateInstance(paramType, property.Name);
            }
            return array;
        }
        private void initParameters()
        {
            if (_parameters == null)
                return;
            for (var i = 0; i < _parameters.Length; i++)
            {
                var parameter = _parameters[i];
                var commandParameter = Inner.CreateParameter();
                commandParameter.ParameterName = $"@{parameter.Name}";
                commandParameter.DbType = parameter.DbType;
                Inner.Parameters.Add(commandParameter);
            }
        }
        public void SetParameters(object values)
        {
            if (_parameters == null)
                return;
            for (var i = 0; i < _parameters.Length; i++)
            {
                var parameter = _parameters[i];
                ((IDbDataParameter)Inner.Parameters[i]).Value = parameter.GetValue(values);
            }
        }
        public IDataReader ExecuteReader() => Inner.ExecuteReader();
    }
}
