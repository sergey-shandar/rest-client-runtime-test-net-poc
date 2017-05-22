using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Rest.ClientRuntime.Test.TextRpc;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;
using System.Threading.Tasks;

namespace UnitTest2.TextRpc
{
    [TestClass]
    public class StreamExUnitTest
    {
        [TestMethod]
        public async Task TestWriteReadMessage()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream);
            await writer.WriteMessageAsync("Hello world!");
            stream.Seek(0, SeekOrigin.Begin);
            var message = await new Reader(stream).ReadMessageAsync();
            Assert.AreEqual("Hello world!", message);
        }
    }
}
