﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Query.Enum
{
    /// <summary>
    /// A type of SQL Statement: Data Definition Language (DDL) or Data Manipulation Language (DML)
    /// </summary>
    internal enum StatementType
    {
        Unknown,
        DML,
        DDL
    }
}
