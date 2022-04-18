using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Query
{
    /// <summary>
    /// Represents the result of checking a SQL statement for any tables that are cooperative
    /// </summary>
    internal class CooperativeReferenceCheckResult
    {
        public string DatabaseName { get; set; }
        public bool HasCooperativeReferences { get; set; }
        public List<CooperativeReferenceCheck> References { get; set; }

        public CooperativeReferenceCheckResult()
        {
            References = new List<CooperativeReferenceCheck>();
        }
    }
}
