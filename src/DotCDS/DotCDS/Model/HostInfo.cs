﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Model
{
    internal class HostInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Token { get; set; }
    }
}
