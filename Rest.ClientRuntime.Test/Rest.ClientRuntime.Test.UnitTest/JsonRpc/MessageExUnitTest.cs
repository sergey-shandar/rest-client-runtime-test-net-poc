using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Rest.ClientRuntime.Test.JsonRpc;

namespace Rest.ClientRuntime.Test.UnitTest.JsonRpc
{
    [TestClass]
    public class MessageExUnitTest
    {
        [TestMethod]
        public void TestWriteReadMessage()
        {
            var stream = new MemoryStream();
            stream.WriteMessage("Hello world!");
            stream.Seek(0, SeekOrigin.Begin);
            var message = new Utf8Reader(stream).ReadMessage();
            Assert.AreEqual("Hello world!", message);
        }
    }
}
