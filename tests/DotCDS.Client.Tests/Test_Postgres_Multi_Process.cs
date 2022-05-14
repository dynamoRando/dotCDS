using DotCDS.Common;
using DotCDS.Common.Enum;
using DotCDS.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotCDS.Client.Tests
{
    public class Test_Postgres_Multi_Process
    {
        [Fact]
        public void Test_Generate_Contract()
        {
            // ARRANGE
            string un = "test";
            string pw = "1234";
            string testDb = "testDb";
            string testTableName = "TestTable";
            uint sqlDbType = (uint)DatabaseClientType.Postgres;
            int testValue = 999;
            string hostName = "company";
            string participantName = "customer";
            string contractDesc = "Test Contract";
            string getDataSql = $"SELECT COLUMN1 FROM {testTableName}";
            string createTableSql = $@"CREATE TABLE IF NOT EXISTS {testTableName} 
            (
            COLUMN1 INT NOT NULL
            ); ";
            string insertForParticipantSql = $@"INSERT INTO {testTableName} (COLUMN1) VALUES ({testValue})";

            var harness = new MultiHarness(true, sqlDbType);
            harness.AddProcess(hostName);
            harness.AddProcess(participantName);
            harness.StartAllProcesses(un, pw);
        }
    }
}
