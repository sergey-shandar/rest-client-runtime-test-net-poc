using System.IO;
using System.Text;

namespace Rest.ClientRuntime.Test
{
    public sealed class Utf8Reader
    {
        private readonly Stream _Stream;

        private const int _NoSymbol = -1;

        private int _Cache = _NoSymbol;

        public Utf8Reader(Stream stream)
        {
            _Stream = stream;
        }

        private static bool IsEol(int c)
            => c == '\n' || c == '\r';

        private static string ReadAll(MemoryStream stream)
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
                if (c == _NoSymbol)
                {
                    break;
                }
                if (IsEol(c))
                {
                    var next = ReadByte();
                    if (!IsEol(next) || next == c)
                    {
                        _Cache = next;
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
            if (c != _NoSymbol)
            {
                buffer[0] = (byte)c;
                offset = 1;
                length--;
            }
            if (0 < length)
            {
                _Stream.Read(buffer, offset, length);
            }
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Read a character from a stream or from a cache if it has a character.
        /// </summary>
        /// <returns></returns>
        private int ReadByte()
        {
            var result = _Cache;
            if (result == _NoSymbol)
            {
                return _Stream.ReadByte();
            }
            _Cache = _NoSymbol;
            return result;
        }
    }
}
