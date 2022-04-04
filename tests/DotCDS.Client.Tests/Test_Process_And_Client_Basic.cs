using DotCDS.Tests;
using Xunit;
using System.IO;
using System.Linq;
using System;
using DotCDS.Common;

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
            int totalRows = (int)createTableResult.TotalRowsAffected;

            var hasTableResult = client.HasTable(sqlDbType, testTableName, testDb, un, pw);
            bool hasTable = hasTableResult.HasTable;

            // ASSERT
            Assert.True(clientIsOnline);
            Assert.True(databaseCreated);
            Assert.InRange(totalRows, 0, 0);
            Assert.True(hasTable);
        }

        [Fact]
        public void Test_Create_Db_Table_And_Crud()
        {
            // ARRANGE
            string un = "test";
            string pw = "1234";
            string testDb = "testDb";
            string testTableName = "TestTable";
            uint sqlDbType = 3;
            int testValue = 999;

            var harness = new SingleHarness(true);
            harness.BringOnline(un, pw);

            var client = new StoreClient();
            client.Configure("http://localhost", harness.SqlPortNumber);

            // ACT
            bool clientIsOnline = client.IsOnline();
            bool databaseCreated = client.CreateDatabase(testDb, un, pw);

            string createTable = $@"CREATE TABLE IF NOT EXISTS {testTableName} 
            (
            COLUMN1 INT NOT NULL
            ); ";

            var createTableResult = client.ExecuteSQLWrite(sqlDbType, createTable, testDb, un, pw);
            int totalRows = (int)createTableResult.TotalRowsAffected;

            var hasTableResult = client.HasTable(sqlDbType, testTableName, testDb, un, pw);
            bool hasTable = hasTableResult.HasTable;

            string insertTable = $@"INSERT INTO {testTableName} (COLUMN1) VALUES ({testValue})";

            var insertTableResult = client.ExecuteSQLWrite(sqlDbType, insertTable, testDb, un, pw);
            int totalRowsInsert = (int)insertTableResult.TotalRowsAffected;

            string getData = $"SELECT COLUMN1 FROM {testTableName}";

            var getDataResult = client.ExecuteSQLRead(sqlDbType, getData, testDb, un, pw);
            int totalRowsReturned = getDataResult.Results.First().Rows.Count();

            var returnedValue = getDataResult.Results.First().Rows[0].Values[0];
            byte[] binaryValue = returnedValue.Value.ToArray();

            var actualValue = DbBinaryConvert.BinaryToInt(binaryValue);

            // ASSERT
            Assert.True(clientIsOnline);
            Assert.True(databaseCreated);
            Assert.InRange(totalRows, 0, 0);
            Assert.True(hasTable);
            Assert.InRange(totalRowsInsert, 1, 1);
            Assert.InRange(totalRowsReturned, 1, 1);
            Assert.Equal(testValue, actualValue);

        }
    }
}