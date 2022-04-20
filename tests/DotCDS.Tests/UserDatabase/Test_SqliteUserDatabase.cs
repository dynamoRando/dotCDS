using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using DotCDS.Common.Enum;
using DotCDS.Enum;

namespace DotCDS.Tests.UserDatabase
{
    public class Test_SqliteUserDatabase
    {
        [Fact]
        public void Test_Setup_New_Db()
        {
            // ARRANGE
            var rootFolder = SingleHarness.GetAndConfigureTempFolder();
            string dbName = "test_new_db";

            // ACT
            var db = new SqliteUserDatabase(rootFolder, dbName);
            bool dbExists = File.Exists(Path.Combine(rootFolder, dbName + ".db"));

            // ASSERT
            Assert.True(dbExists);
        }

        [Fact(Skip = "test not written yet")]
        public void Test_CreateTable_Configure_Cooperation()
        {
            // ARRANGE
            var rootFolder = SingleHarness.GetAndConfigureTempFolder();
            string dbName = "test_setup_coop";
            string tableName = "TESTFOO";
            LogicalStoragePolicy policy = LogicalStoragePolicy.Shared;
            bool dbExists = false;
            bool hasTable = false;
            bool enableCooperativeFeatures = false;
            bool lspConfigured = false;
            LogicalStoragePolicy returnedPolicy = LogicalStoragePolicy.None;

            string sqlCreateTable = @$"CREATE TABLE IF NOT EXISTS {tableName} 
            (
                COL1 INT
            ); ";

            // ACT
            var db = new SqliteUserDatabase(rootFolder, dbName);
            dbExists = File.Exists(Path.Combine(rootFolder, dbName + ".db"));

            if (dbExists)
            {
                db.ExecuteWrite(sqlCreateTable);
            }

            hasTable = db.HasTable(tableName);

            enableCooperativeFeatures = false;

            if (hasTable)
            {
                var status = db.EnableCooperativeFeatures().Status;
                if (status == ActionOptionalResultStatus.Success)
                {
                    enableCooperativeFeatures = true;
                }
            }

            if (enableCooperativeFeatures)
            {
                lspConfigured = db.SetLogicalStoragePolicy(tableName, policy);
                returnedPolicy = db.GetLogicalStoragePolicy(tableName);
            }

            // ASSERT
            Assert.True(dbExists);
            Assert.True(hasTable);
            Assert.True(lspConfigured);
            Assert.Equal(policy, returnedPolicy);

            throw new NotImplementedException();
        }
    }
}
