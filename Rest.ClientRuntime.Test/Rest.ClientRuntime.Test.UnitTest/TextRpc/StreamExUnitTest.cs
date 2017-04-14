using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rest.ClientRuntime.Test.TextRpc;
using Rest.ClientRuntime.Test.Utf8;
using System.IO;

namespace Rest.ClientRuntime.Test.UnitTest.TextRpc
{
    [TestClass]
    public class StreamExUnitTest
    {
        [TestMethod]
        public void TestWriteReadMessage()
        {
            var stream = new MemoryStream();
            var writer = new Writer(stream);
            writer.WriteMessage("Hello world!");
            stream.Seek(0, SeekOrigin.Begin);
            var message = new Reader(stream).ReadMessage();
            Assert.AreEqual("Hello world!", message);
        }
    }
}
