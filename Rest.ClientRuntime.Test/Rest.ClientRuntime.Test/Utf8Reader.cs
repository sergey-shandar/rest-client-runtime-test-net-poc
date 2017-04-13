using System.IO;
using System.Text;

namespace Rest.ClientRuntime.Test
{
    public sealed class Utf8Reader
    {
        public Stream Stream { get; }

        public const int NoSymbol = -1;

        public int Cache { get; set; } = NoSymbol;

        public Utf8Reader(Stream stream)
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

        public string ReadLine()
        {
            var result = new MemoryStream();
            while (true)
            {
                var c = ReadByte();
                if (c == NoSymbol)
                {
                    break;
                }
                if (IsEol(c))
                {
                    var next = ReadByte();
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

        public string ReadBlock(int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }
            var buffer = new byte[length];
            var c = ReadByte();
            var offset = 0;
            if (c != NoSymbol)
            {
                buffer[0] = (byte)c;
                offset = 1;
                length--;
            }
            if (0 < length)
            {
                Stream.Read(buffer, offset, length);
            }
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Read a character from a stream or from a cache if it has a character.
        /// </summary>
        /// <returns></returns>
        public int ReadByte()
        {
            var result = Cache;
            if (result == NoSymbol)
            {
                return Stream.ReadByte();
            }
            Cache = NoSymbol;
            return result;
        }
    }
}
