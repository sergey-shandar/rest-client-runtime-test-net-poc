using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rest.ClientRuntime.Test.TextRpc;
using System.IO;

namespace Rest.ClientRuntime.Test.UnitTest.JsonRpc
{
    [TestClass]
    public class StreamExUnitTest
    {
        [TestMethod]
        public void TestWriteReadMessage()
        {
            var stream = new MemoryStream();
            var writer = new Utf8Writer(stream);
            writer.WriteMessage("Hello world!");
            stream.Seek(0, SeekOrigin.Begin);
            var message = new Utf8Reader(stream).ReadMessage();
            Assert.AreEqual("Hello world!", message);
        }
    }
}
