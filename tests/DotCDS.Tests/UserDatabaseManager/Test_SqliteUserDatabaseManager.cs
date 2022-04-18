using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotCDS.Tests.UserDatabaseManager
{
    public class Test_SqliteUserDatabaseManager
    {
        [Fact]
        public void Test_Add_Has_Database()
        {
            // ARRANGE
            string rootFolder = SingleHarness.GetAndConfigureTempFolder();
            string databaseName = "TestDb1";
            SqliteUserDatabaseManager dbManager = new SqliteUserDatabaseManager(rootFolder);

            // ACT
            bool resultCreateDb = dbManager.CreateUserDatabase(databaseName).IsSuccessful;
            bool HasDb = dbManager.HasDatabase(databaseName);
            SqliteUserDatabase db = dbManager.GetSqliteUserDatabase(databaseName);

            // ASSERT
            Assert.True(resultCreateDb);
            Assert.True(HasDb);
            Assert.NotNull(db);
        }
    }
}
