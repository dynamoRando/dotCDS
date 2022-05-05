using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Query
{
    /// <summary>
    /// Represents a remote data table and the columns referenced by it
    /// </summary>
    internal record CooperativeReference
    {
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public List<string> Columns { get; set; }

        public bool IsDatabaseNameSet()
        {
            return string.IsNullOrEmpty(DatabaseName);
        }

        public bool IsTableNameSet()
        {
            return string.IsNullOrEmpty(TableName);
        }

        public bool AreColumnsSet()
        {
            return Columns != null && Columns.Count > 0;
        }

        public bool IsSet()
        {
            return AreColumnsSet() && IsTableNameSet() && IsDatabaseNameSet();
        }
    }
}
