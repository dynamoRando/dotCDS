using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotCDS.Tests.DatabaseClient
{
    public class Test_Sqlite
    {
        /// <summary>
        /// Tests the creation of a new SQLite db
        /// </summary>
        [Fact]
        public void Test_CreateNewDb()
        {
            // ARRANGE
            string rootFolder = Path.Combine(TestConstants.TEST_TEMP_FOLDER, "Test_CreateNewDb");
            var testHarness = new SingleHarness(rootFolder);
            testHarness.SetupTempFolder();
            string dbName = "dotCDS.db";
            string dbLocation = Path.Combine(rootFolder, dbName);

            // ACT
            Process _process = new Process(rootFolder);
            _process.Start();

            // ASSERT
            Assert.True(File.Exists(dbLocation));
        }  
    }
}
