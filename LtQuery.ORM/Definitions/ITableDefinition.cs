using System;
using System.Collections.Generic;

namespace LtQuery.ORM.Definitions
{
    public interface ITableDefinition
    {
        Type EntityType { get; }
        string Name { get; }
        IReadOnlyList<IColumnDefinition> Columns { get; }
        IColumnDefinition PrimaryKeyColumn { get; }
        //IColumnDefinition DefaultSortColumn { get; }
    }
}
