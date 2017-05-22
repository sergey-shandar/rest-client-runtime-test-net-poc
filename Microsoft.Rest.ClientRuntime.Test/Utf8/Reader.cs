using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public sealed class Reader : IReader
    {
        public Stream Stream { get; }

        public const int NoSymbol = -1;

        public int Cache { get; set; } = NoSymbol;

        public Reader(Stream stream)
        {
            Stream = stream;
        }

        public static bool IsEol(int c)
            => c == '\n' || c == '\r';

        public static string ReadAll(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Read a character from a stream or from a cache if it has a character.
        /// </summary>
        /// <returns></returns>
        public async Task<int> ReadByteAsync()
        {
            var result = Cache;
            if (result == NoSymbol)
            {
                return await Stream.ReadByteAsync();
            }
            Cache = NoSymbol;
            return result;
        }

        public async Task<string> ReadLineAsync()
        {
            var result = new MemoryStream();
            while (true)
            {
                var c = await ReadByteAsync();
                if (c == NoSymbol)
                {
                    break;
                }
                if (IsEol(c))
                {
                    var next = await ReadByteAsync();
                    if (!IsEol(next) || next == c)
                    {
                        Cache = next;
                    }
                    break;
                }
                result.WriteByte((byte)c);
            }
            return ReadAll(result);
        }

        public async Task<string> ReadBlockAsync(int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }
            var buffer = new byte[length];
            var c = await ReadByteAsync();
            var offset = 0;
            if (c != NoSymbol)
            {
                buffer[0] = (byte)c;
                offset = 1;
                length--;
            }
            if (0 < length)
            {
                await Stream.ReadBufferAsync(buffer, offset, length);
            }
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
