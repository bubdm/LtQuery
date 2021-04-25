using System.Data;

namespace LtQuery.ORM.SQL.Commands
{
    class WithoutParameterCommand : ICommand
    {
        public IDbCommand Inner { get; }
        public WithoutParameterCommand(IDbConnection connection, string sql)
        {
            Inner = connection.CreateCommand();
            Inner.CommandText = sql;
        }
        public void Dispose()
        {
            Inner.Dispose();
        }

        public IDataReader ExecuteReader() => Inner.ExecuteReader();

        public void SetParameters(object values) { }
    }
}
