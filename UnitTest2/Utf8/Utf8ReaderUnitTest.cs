using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;
using System.Text;

namespace Rest.ClientRuntime.Test.UnitTest.Utf8
{
    [TestClass]
    public class ReaderUnitTest
    {
        private static Reader CreateReader(string source)
            => new Reader(new MemoryStream(Encoding.UTF8.GetBytes(source)));

        private static void TestReadLine(string source, string[] expectedArray)
        {
            var r = CreateReader(source);
            foreach (var expected in expectedArray)
            {
                var line = r.ReadLine();
                Assert.AreEqual(expected, line);
            }
        }

        private static void TestReadLine(string source)
        {
            TestReadLine(source, new[] { source });
        }

        [TestMethod]
        public void TestReadLine()
        {
            TestReadLine(string.Empty);
            TestReadLine("Hello world!");
            TestReadLine("Hello world! Привет мир!");

            TestReadLine(
                "Hello world!\r\n Привет мир!",
                new[] { "Hello world!", " Привет мир!"});

            TestReadLine(
                "Hello world!\r\n Привет мир!\n\na\r\nb\r\rc",
                new[] { "Hello world!", " Привет мир!", "", "a", "b", "", "c" });
        }

        private static void TestReadBlock(string source, string expected)
        {
            var r = CreateReader(source);
            var length = Encoding.UTF8.GetByteCount(expected);
            var line = r.ReadBlock(length);
            Assert.AreEqual(expected, line);
        }

        private static void TestReadBlock(string source)
        {
            TestReadBlock(source, source);
        }

        [TestMethod]
        public void TestReadBlock()
        {
            TestReadBlock("Hello world!");
            TestReadBlock("Привет мир!");

            TestReadBlock("Привет мир! xxxx", "Привет мир!");
        }

        [TestMethod]
        public void TestReadLineAndBlock()
        {
            var r = CreateReader("Content-Length: 10\r\n\n0123456789");
            var s0 = r.ReadLine();
            Assert.AreEqual("Content-Length: 10", s0);
            var s1 = r.ReadLine();
            Assert.AreEqual(string.Empty, s1);
            var b = r.ReadBlock(10);
            Assert.AreEqual("0123456789", b);
        }

        [TestMethod]
        public void TestReadLineAndBlockCyrillic()
        {
            var r = CreateReader("Content-Length: 20\r\n\nПривет мир!");
            var s0 = r.ReadLine();
            Assert.AreEqual("Content-Length: 20", s0);
            var s1 = r.ReadLine();
            Assert.AreEqual(string.Empty, s1);
            var b = r.ReadBlock(20);
            Assert.AreEqual("Привет мир!", b);
        }
    }
}
