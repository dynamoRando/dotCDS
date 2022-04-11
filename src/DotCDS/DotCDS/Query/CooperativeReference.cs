using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Query
{
    internal record struct CooperativeReference
    {
        public string DatbaseName { get; init; }
        public string TableName { get; init; }
        public List<string> Columns { get; init; }
    }
}
