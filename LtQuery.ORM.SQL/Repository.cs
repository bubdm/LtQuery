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
        private readonly ITableReader<TEntity> _tableReader;
        public Repository(LtConnection connection, ISqlBuilder sqlBuilder, ITableReader<TEntity> tableReader)
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


        private Dictionary<ISqlQuery<TEntity>, ICommand> _commands = new Dictionary<ISqlQuery<TEntity>, ICommand>();
        private ICommand getParameterCommand<TDynamic>(ISqlQuery<TEntity> query)
        {
            ICommand command;
            if (!_commands.TryGetValue(query, out command))
            {
                command = createParameterCommand<TDynamic>(query);
                _commands.Add(query, command);
            }
            return command;
        }
        private ICommand createParameterCommand<TDynamic>(ISqlQuery<TEntity> query)
            => new AnonymousParameterCommand<TDynamic>(_connection.SqlConnection, _sqlBuilder.Build(query));
        private ICommand getCommand(ISqlQuery<TEntity> query)
        {
            ICommand command;
            if (!_commands.TryGetValue(query, out command))
            {
                command = createCommand(query);
                _commands.Add(query, command);
            }
            return command;
        }
        private ICommand createCommand(ISqlQuery<TEntity> query)
            => new WithoutParameterCommand(_connection.SqlConnection, _sqlBuilder.Build(query));


        public int Load(Count<TEntity> query)
        {
            var command = getCommand(query);
            return count(command);
        }
        public int Load<TParameter>(Count<TEntity> query, TParameter values)
        {
            var command = getParameterCommand<TParameter>(query);
            command.SetParameters(values);
            return count(command);
        }
        private int count(ICommand command)
        {
            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Sequence contains no elements");
                switch (reader[0])
                {
                    case int count:
                        return count;
                    case long count:
                        return (int)count;
                    default:
                        throw new InvalidOperationException("Responsed count query is unknown type");
                }
            }
        }


        public IEnumerable<TEntity> Load(SelectMany<TEntity> query)
        {
            var command = getCommand(query);
            return select(command);
        }
        public IEnumerable<TEntity> Load<TParameter>(SelectMany<TEntity> query, TParameter values)
        {
            var command = getParameterCommand<TParameter>(query);
            command.SetParameters(values);
            return select(command);
        }
        private IEnumerable<TEntity> select(ICommand command)
        {
            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return _tableReader.Read(reader, 0);
                }
            }
        }


        public TEntity Load(SelectSingle<TEntity> query)
        {
            var command = getCommand(query);
            return querySingle(command);
        }
        public TEntity Load<TParameter>(SelectSingle<TEntity> query, TParameter values)
        {
            var command = getParameterCommand<TParameter>(query);
            command.SetParameters(values);
            return querySingle(command);
        }
        private TEntity querySingle(ICommand command)
        {
            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Sequence contains no elements");
                var entity = _tableReader.Read(reader, 0);
                if (reader.Read())
                    throw new InvalidOperationException("Sequence contains more than one element");
                return entity;
            }
        }

        public TEntity Load(SelectFirst<TEntity> query)
        {
            var command = getCommand(query);
            return queryFirst(command);
        }
        public TEntity Load<TParameter>(SelectFirst<TEntity> query, TParameter values)
        {
            var command = getParameterCommand<TParameter>(query);
            command.SetParameters(values);
            return queryFirst(command);
        }
        private TEntity queryFirst(ICommand command)
        {
            _connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Sequence contains no elements");
                return _tableReader.Read(reader, 0);
            }
        }
    }
}
