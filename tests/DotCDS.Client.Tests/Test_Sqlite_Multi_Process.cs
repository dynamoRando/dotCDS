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
    public class Test_Sqlite_Multi_Process
    {
        [Fact]
        public void Test_Generate_Contract()
        {
            // ARRANGE
            string un = "test";
            string pw = "1234";
            string testDb = "testDb";
            string testTableName = "TestTable";
            uint sqlDbType = (uint)DatabaseClientType.Sqlite;
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

            var harness = new MultiHarness(true);
            harness.AddProcess(hostName);
            harness.AddProcess(participantName);
            harness.StartAllProcesses(un, pw);

            StoreClient hostClient = harness.GetClient(hostName);
            StoreClient participantClient = harness.GetClient(participantName);
            ProcessContainer participantContainer = harness.GetProcessContainer(participantName);

            // ACT
            bool clientIsOnline = hostClient.IsOnline();
            bool databaseCreated = hostClient.CreateDatabase(testDb, un, pw);

            var createTableResult = hostClient.ExecuteSQLWrite(sqlDbType, createTableSql, testDb, un, pw);
            int totalRows = (int)createTableResult.TotalRowsAffected;

            var hasTableResult = hostClient.HasTable(sqlDbType, testTableName, testDb, un, pw);
            bool hasTable = hasTableResult.HasTable;

            // cooperative actions

            // -- first, configure the company to have a contract
            // and then add the customer as a participant
            var enableCooperation = hostClient.EnableCooperativeFeatures(testDb, un, pw);
            var setPolicy = hostClient.SetLogicalStoragePolicy(testDb, testTableName, (uint)LogicalStoragePolicy.ParticipantOwned, un, pw);
            var generateContract = hostClient.GenerateContract(hostName, contractDesc, testDb, un, pw);
            var addParticipant = hostClient.AddParticipant(participantName, participantContainer.Address, string.Empty, (uint)participantContainer.DatabasePortNumber, testDb, un, pw);

            // -- on the customer side, view and accept the contract
            var pendingContracts = participantClient.ViewPendingContracts(un, pw);
            var acceptContract = participantClient.AcceptPendingContract(hostName, un, pw);

            // -- back on the host side, do a test insert
            var insertCooperatively = hostClient.ExecuteSQLCooperativeWrite(sqlDbType, participantName, Guid.Empty, insertForParticipantSql, testDb, un, pw);

            // -- on the participant side, lets verify that we actually have the value
            var getDataAtParticipant = participantClient.ExecuteSQLRead(sqlDbType, getDataSql, testDb, un, pw);
            int totalRowsReturnedParticipant = getDataAtParticipant.Results.First().Rows.Count();

            var returnedValueParticipant = getDataAtParticipant.Results.First().Rows[0].Values[0];
            byte[] binaryValueParticipant = returnedValueParticipant.Value.ToArray();

            var actualValueFromParticipant = DbBinaryConvert.BinaryToInt(binaryValueParticipant);

            // -- now make sure we can get the data back from the participant from the host
            var getDataResult = hostClient.ExecuteSQLRead(sqlDbType, getDataSql, testDb, un, pw);
            int totalRowsReturned = getDataResult.Results.First().Rows.Count();

            var returnedValue = getDataResult.Results.First().Rows[0].Values[0];
            byte[] binaryValue = returnedValue.Value.ToArray();

            var actualValueFromHost = DbBinaryConvert.BinaryToInt(binaryValue);

            // ASSERT
            Assert.Equal(testValue, actualValueFromHost);
            Assert.Equal(testValue, actualValueFromParticipant);
        }
    }
}
