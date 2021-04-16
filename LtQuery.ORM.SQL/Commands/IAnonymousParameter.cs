using System.Data;

namespace LtQuery.ORM.SQL.Commands
{
    interface IAnonymousParameter
    {
        string Name { get; }
        DbType DbType { get; }
        object GetValue(object values);
    }
}
