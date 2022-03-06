using Xunit;

namespace DotCDS.Tests
{
    public class Test_Startup
    {
        /// <summary>
        /// Tests that we can read the config file correctly
        /// </summary>
        [Fact]
        public void Test_LoadConfig()
        {
            // ARRANGE
            var _process = new Process();

            // ACT
            _process.Start();

            // ASSERT
            Assert.Equal(5016, _process.Settings.DatabaseServicePort);

            string connectionString = "data source=example;initial catalog=example;persist security info=True;user id=example;password=example";
            Assert.Equal(_process.TestConnectionString(), connectionString);
        }
    }
}