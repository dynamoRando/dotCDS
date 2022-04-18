using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Query
{
    /// <summary>
    /// Represents a SQL table in a database that we are checking to see if it is cooperative
    /// </summary>
    internal record struct CooperativeReferenceCheck
    {
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public bool IsCooperating { get; set; }
    }
}
