using System;

namespace LtQuery.ORM.Definitions
{
    public interface IColumnDefinition
    {
        Type Type { get; }
        string Name { get; }

    }
}
