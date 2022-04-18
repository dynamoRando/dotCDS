using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotCDS.Tests.DatabaseClient
{
    public class Test_SqliteClient
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
        
        /// <summary>
        /// Tests setting up an administrator login
        /// </summary>
        [Fact]
        public void Test_SetupAdmin()
        {
            // ARRANGE
            string rootFolder = Path.Combine(TestConstants.TEST_TEMP_FOLDER, "Test_SetupAdmin");
            var testHarness = new SingleHarness(rootFolder);
            testHarness.SetupTempFolder();
        
            // ACT
            Process _process = new Process(rootFolder);
            _process.Start();
            _process.Test_SetupAdmin("tester", "1234");

            // ASSERT
            Assert.True(_process.IsAdminLogin("tester"));
        }

        /// <summary>
        /// Tests setting up an admin login and that the pw is correct
        /// </summary>
        [Fact]
        public void Test_AdminIsValid()
        {
            // ARRANGE
            string rootFolder = Path.Combine(TestConstants.TEST_TEMP_FOLDER, "Test_AdminIsValid");
            var testHarness = new SingleHarness(rootFolder);
            testHarness.SetupTempFolder();

            // ACT
            Process _process = new Process(rootFolder);
            _process.Start();
            _process.Test_SetupAdmin("tester", "1234");

            // ASSERT
            Assert.True(_process.IsValidLogin("tester", "1234"));
        }

    }
}
