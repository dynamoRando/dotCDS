using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    /// <summary>
    /// Represents an IP Address and a Port Number
    /// </summary>
    record struct PortSettings
    {
        public string IPAddress { get; set; }
        public int PortNumber { get; set; }
    }
}
