using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Model
{
    internal class DatabaseHostInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Token { get; set; }
        public PortSettings DataPortSettings { get; set; }
        public PortSettings SQLPortSettings { get; set; }
    }
}
