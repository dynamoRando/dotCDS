using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Common.Enum
{
    /// <summary>
    /// Represents the primary database that is backing dotCDS. This db has the dotCDS database 
    /// and other metadata used by the service.
    /// </summary>
    public enum DatabaseClientType
    {
        Unknown,
        SQLServer,
        Postgres,
        Sqlite
    }
}
