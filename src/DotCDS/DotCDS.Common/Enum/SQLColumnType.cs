using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Common
{
    public enum SQLColumnType
    {
        Int,
        Bit,
        Char,
        DateTime,
        Decimal,
        Varchar,
        Binary,
        Varbinary,
        Unknown
    }

    public static class SQLColumnTypeHelper
    {
        public static SQLColumnType GetType(string type)
        {
            if (type.Contains("Int32", StringComparison.OrdinalIgnoreCase))
            { 
                return SQLColumnType.Int;
            }

            if (type.Contains("integer", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Int;
            }

            if (type.Contains("int", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Int;
            }

            if (type.Contains("Boolean", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Bit;
            }

            if (type.Contains("bit", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Bit;
            }

            if (type.Contains("Char", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Char;
            }

            if (type.Contains("DateTime", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.DateTime;
            }

            if (type.Contains("Decimal", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Decimal;
            }

            if (type.Contains("String", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Varchar;
            }

            if (type.Contains("Byte", StringComparison.OrdinalIgnoreCase))
            {
                return SQLColumnType.Binary;
            }

            return SQLColumnType.Unknown;
        }
    }
}
