using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rest.ClientRuntime.Test.UnitTest.JsonRpc
{
    [TestClass]
    public class RemoteServerUnitTest
    {
        [TestMethod]
        public async Task TestCall()
        {
            var marshalling = new Marshalling(
                new JsonSerializerSettings(), new JsonSerializerSettings());
            var response = 
                "Content-Length:16\n" +
                "\n\r" +
                "{\"result\":\"abc\"}";
            var reader = new Reader(new MemoryStream(Encoding.UTF8.GetBytes(response)));
            var writeStream = new MemoryStream();
            var writer = new Writer(writeStream);
            var server = new RemoteServer(new Io(reader, writer), marshalling);

            var @params = new Dictionary<string, object>();
            var result = await server.Call<string>("somemethod", @params);

            Assert.AreEqual("abc", result);

            var outputBuffer = Encoding.UTF8.GetString(writeStream.ToArray());
            var lines = outputBuffer.Split('\n');
            Assert.AreEqual("Content-Length", lines[0].Split(':')[0]);
            Assert.AreEqual("\r", lines[1]);
            Assert.AreEqual('{', lines[2][0]);
        }
    }
}
