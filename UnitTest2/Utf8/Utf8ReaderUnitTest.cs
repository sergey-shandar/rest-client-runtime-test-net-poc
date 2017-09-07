using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest2.Utf8
{
    [TestClass]
    public class ReaderUnitTest
    {
        private static Reader CreateReader(string source)
            => new Reader(new MemoryStream(Encoding.UTF8.GetBytes(source)));

        private static async Task TestReadLine(string source, string[] expectedArray)
        {
            var r = CreateReader(source);
            foreach (var expected in expectedArray)
            {
                var line = await r.ReadLineAsync();
                Assert.AreEqual(expected, line);
            }
        }

        private static Task TestReadLine(string source)
            => TestReadLine(source, new[] { source });

        [TestMethod]
        public async Task TestReadLine()
        {
            await TestReadLine(string.Empty);
            await TestReadLine("Hello world!");
            await TestReadLine("Hello world! Привет мир!");

            await TestReadLine(
                "Hello world!\r\n Привет мир!",
                new[] { "Hello world!", " Привет мир!"});

            await TestReadLine(
                "Hello world!\r\n Привет мир!\n\na\r\nb\r\rc",
                new[] { "Hello world!", " Привет мир!", "", "a", "b", "", "c" });
        }

        private static async Task TestReadBlock(string source, string expected)
        {
            var r = CreateReader(source);
            var length = Encoding.UTF8.GetByteCount(expected);
            var line = await r.ReadBlockAsync(length);
            Assert.AreEqual(expected, line);
        }

        private static Task TestReadBlock(string source)
            => TestReadBlock(source, source);

        [TestMethod]
        public async Task TestReadBlock()
        {
            await TestReadBlock("Hello world!");
            await TestReadBlock("Привет мир!");

            await TestReadBlock("Привет мир! xxxx", "Привет мир!");
        }

        [TestMethod]
        public async Task TestReadLineAndBlock()
        {
            var r = CreateReader("Content-Length: 10\r\n\n0123456789");
            var s0 = await r.ReadLineAsync();
            Assert.AreEqual("Content-Length: 10", s0);
            var s1 = await r.ReadLineAsync();
            Assert.AreEqual(string.Empty, s1);
            var b = await r.ReadBlockAsync(10);
            Assert.AreEqual("0123456789", b);
        }

        [TestMethod]
        public async Task TestReadLineAndBlockCyrillic()
        {
            var r = CreateReader("Content-Length: 20\r\n\nПривет мир!");
            var s0 = await r.ReadLineAsync();
            Assert.AreEqual("Content-Length: 20", s0);
            var s1 = await r.ReadLineAsync();
            Assert.AreEqual(string.Empty, s1);
            var b = await r.ReadBlockAsync(20);
            Assert.AreEqual("Привет мир!", b);
        }
    }
}
