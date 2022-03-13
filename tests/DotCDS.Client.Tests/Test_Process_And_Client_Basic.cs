using DotCDS.Tests;
using Xunit;
using System.IO;

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
    }
}