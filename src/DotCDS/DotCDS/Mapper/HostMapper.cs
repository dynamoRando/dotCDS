using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common;
using DotCDS.Model;

namespace DotCDS.Mapper
{
    internal class HostMapper
    {
        public static Host Map(DatabaseHostInfo host)
        {
            var messageHost = new Host();
            messageHost.HostName = host.Name;
            messageHost.DatabasePortNumber = (uint)host.DataPortSettings.PortNumber;
            messageHost.Ip4Address = host.DataPortSettings.IPAddress;

            return messageHost;
        }
    }
}
