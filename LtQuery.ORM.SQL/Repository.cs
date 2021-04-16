using System;
using System.Collections.Generic;

namespace LtQuery.ORM.SQL
{
    using Commands;
    using Readers;
    using SqlQueries;

    class Repository<TEntity> : IRepository<TEntity>
    {
        private readonly LtConnection _connection;
        private readonly ISqlBuilder _sqlBuilder;
        private readonly TableReader<TEntity> _tableReader;
        public Repository(LtConnection connection, ISqlBuilder sqlBuilder, TableReader<TEntity> tableReader)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _sqlBuilder = sqlBuilder ?? throw new ArgumentNullException(nameof(sqlBuilder));
            _tableReader = tableReader ?? throw new ArgumentNullException(nameof(connection));
        }
        public void Dispose()
        {
            foreach (IDisposable command in _commands.Values)
                command.Dispose();
        }


        private Dictionary<ISqlQuery<TEntity>, IAnonymousParameterCommand<TEntity>> _commands = new Dictionary<ISqlQuery<TEntity>, IAnonymousParameterCommand<TEntity>>();
        private IAnonymousParameterCommand<TEntity> getCommand(ISqlQuery<TEntity> query, object values)
        {
            IAnonymousParameterCommand<TEntity> command;
            if (!_commands.TryGetValue(query, out command))
            {
                command = createCommand(query, values);
                _commands.Add(query, command);
            }
            return command;
        }
        private IAnonymousParameterCommand<TEntity> createCommand(ISqlQuery<TEntity> query, object values)
            => new AnonymousParameterCommand<TEntity>(_connection.SqlConnection, _sqlBuilder.Build(query), values);


        public int Count(Count<TEntity> query, object values = null)
        {
            var command = getCommand(query, values);
            command.SetParameters(values);

            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    throw new LtQueryException("No record");
                switch (reader[0])
                {
                    case int count:
                        return count;
                    case long count:
                        return (int)count;
                    default:
                        throw new LtQueryException("Responsed count query is unknown type");
                }
            }
        }
        public IEnumerable<TEntity> Query(Select<TEntity> query, object values = null)
        {
            var command = getCommand(query, values);
            command.SetParameters(values);

            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return _tableReader.Read(reader, 0);
                }
            }
        }
        public TEntity QuerySingle(Select<TEntity> query, object values = null)
        {
            var command = getCommand(query, values);
            command.SetParameters(values);

            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    throw new LtQueryException("No record");
                var entity = _tableReader.Read(reader, 0);
                if (reader.Read())
                    throw new LtQueryException("Not Single");
                return entity;
            }
        }
        public TEntity QueryFirst(Select<TEntity> query, object values = null)
        {
            var command = getCommand(query, values);
            command.SetParameters(values);

            _connection.Open();

            using (var reader = command.
                ExecuteReader())
            {
                if (!reader.Read())
                    throw new LtQueryException("No record");
                return _tableReader.Read(reader, 0);
            }
        }
    }
}
