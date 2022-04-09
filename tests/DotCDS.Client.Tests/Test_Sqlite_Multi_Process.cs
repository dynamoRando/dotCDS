using DotCDS.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotCDS.Client.Tests
{
    public class Test_Sqlite_Multi_Process
    {
        [Fact(Skip ="code not written yet")]
        public void Test_Generate_Contract()
        {
            // ARRANGE
            string un = "test";
            string pw = "1234";
            string testDb = "testDb";
            string testTableName = "TestTable";
            uint sqlDbType = 3;
            int testValue = 999;

            var harness = new MultiHarness(true);
            harness.AddProcess("company");
            harness.AddProcess("customer");
            harness.StartAllProcesses(un, pw);

            var companyClient = harness.GetClient("company");

            // ACT
            bool clientIsOnline = companyClient.IsOnline();
            bool databaseCreated = companyClient.CreateDatabase(testDb, un, pw);

            string createTable = $@"CREATE TABLE IF NOT EXISTS {testTableName} 
            (
            COLUMN1 INT NOT NULL
            ); ";

            var createTableResult = companyClient.ExecuteSQLWrite(sqlDbType, createTable, testDb, un, pw);
            int totalRows = (int)createTableResult.TotalRowsAffected;

            var hasTableResult = companyClient.HasTable(sqlDbType, testTableName, testDb, un, pw);
            bool hasTable = hasTableResult.HasTable;

        }
    }
}
