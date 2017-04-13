using System.IO;
using System.Text;

namespace Rest.ClientRuntime.Test
{
    public sealed class Utf8Writer
    {
        public Stream Stream { get; }

        public const string Eol = "\r\n"; 

        public Utf8Writer(Stream stream)
        {
            Stream = stream;
        }

        public Utf8Writer Write(string value)
        {
            var array = Encoding.UTF8.GetBytes(value);
            Stream.Write(array, 0, array.Length);
            return this;
        }

        public Utf8Writer WriteLine()
            => Write(Eol);

        public Utf8Writer WriteLine(string value)
            => Write(value).WriteLine();
    }
}
