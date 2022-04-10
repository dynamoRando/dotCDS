using DotCDS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.TestHelpers
{
    public class ProcessContainer
    {
        public Process Process;
        public string Name;
        public string Folder;
        public string Address;
        public int Index;
        public int AdminPortNumber;
        public int DatabasePortNumber;
        public int SqlPortNumber;
        public StoreClient Client;
    }
}
