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
    internal record struct CooperativeReference
    {
        public string DatabaseName { get; init; }
        public string TableName { get; init; }
        public List<string> Columns { get; init; }
    }
}
