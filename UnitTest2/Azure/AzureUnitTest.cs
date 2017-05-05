using Microsoft.Rest.ClientRuntime.Test.Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestServer;

namespace UnitTest2.Azure
{
    [TestClass]
    public class AzureUnitTest
    {
        [TestMethod]
        public async Task ProcessTest()
        {
            var location = typeof(Program).Assembly.Location;
            Environment.SetEnvironmentVariable(HttpSendMock.SdkRemoteServer, location);
            await HttpSendMock.SendAsync(new HttpRequestMessage()
            {
                Content = new StringContent("{}"),
            });
        }
    }
}
