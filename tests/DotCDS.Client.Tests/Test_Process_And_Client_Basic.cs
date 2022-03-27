using DotCDS.Tests;
using Xunit;
using System.IO;
using System.Linq;

namespace DotCDS.Client.Tests
{
    public class Test_Process_And_Client_Basic
    {
        [Fact]
        public void Test_Process_And_Client_Connection()
        {
            // ARRANGE
            var harness = new SingleHarness(true);
            harness.BringOnline();

            var client = new StoreClient();
            client.Configure("http://localhost", harness.SqlPortNumber);

            // ACT
            // ASSERT
            Assert.True(client.IsOnline());
        }

        [Fact]
        public void Test_Create_Db_And_Table()
        {
            // ARRANGE
            string un = "test";
            string pw = "1234";
            string testDb = "testDb";
            string testTableName = "TestTable";
            uint sqlDbType = 3;

            var harness = new SingleHarness(true);
            harness.BringOnline(un, pw);

            var client = new StoreClient();
            client.Configure("http://localhost", harness.SqlPortNumber);

            // ACT
            bool clientIsOnline = client.IsOnline();
            bool databaseCreated = client.CreateDatabase(testDb, un, pw);

            string createTable = $@"CREATE TABLE IF NOT EXISTS {testTableName} 
            (
            COLUMN1 VARCHAR(25) NOT NULL,
            COLUMN2 VARCHAR(25) NOT NULL
            ); ";

            var createTableResult = client.ExecuteSQLWrite(sqlDbType, createTable, testDb, un, pw);
            var isSuccessful = createTableResult.IsSuccessful;

            // ASSERT
            Assert.True(clientIsOnline);
            Assert.True(databaseCreated);
            Assert.True(isSuccessful);
        }
    }
}