using System;
using System.Collections.Generic;
using System.Data;

namespace LtQuery.ORM.SQL.Commands
{
    static class SqlDbTypeExtensions
    {
        private static Lazy<Dictionary<Type, DbType>> _dbTypesLazy = new Lazy<Dictionary<Type, DbType>>(createDbTypes);
        private static Dictionary<Type, DbType> _dbTypes = _dbTypesLazy.Value;
        private static Dictionary<Type, DbType> createDbTypes()
            => new Dictionary<Type, DbType>()
            {
                { typeof(int), DbType.Int32 },
                { typeof(int?), DbType.Int32 },
                { typeof(long), DbType.Int64 },
                { typeof(long?), DbType.Int64 },
                { typeof(double), DbType.Double },
                { typeof(double?), DbType.Double },
                { typeof(string), DbType.String },
                { typeof(DateTime), DbType.DateTime },
                { typeof(DateTime?), DbType.DateTime },
            };
        public static DbType GetDbType(this Type _this)
        {
            DbType result;
            if (_dbTypes.TryGetValue(_this, out result))
                return result;
            throw new NotSupportedException($"No Db matching [{_this}] type");
        }
    }
}
