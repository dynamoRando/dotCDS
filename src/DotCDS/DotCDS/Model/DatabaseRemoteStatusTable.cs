﻿using DotCDS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Model
{
    internal class DatabaseRemoteStatusTable
    {
        public string TableName { get; set; }
        public LogicalStoragePolicy StoragePolicy { get; set; }
    }
}
