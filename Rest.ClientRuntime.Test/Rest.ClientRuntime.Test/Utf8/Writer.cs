using System.IO;
using System.Text;

namespace Rest.ClientRuntime.Test.Utf8
{
    public sealed class Writer
    {
        public Stream Stream { get; }

        public const string Eol = "\r\n"; 

        public Writer(Stream stream)
        {
            Stream = stream;
        }

        public Writer Write(string value)
        {
            var array = Encoding.UTF8.GetBytes(value);
            Stream.Write(array, 0, array.Length);
            return this;
        }

        public Writer WriteLine()
            => Write(Eol);

        public Writer WriteLine(string value)
            => Write(value).WriteLine();

        public void Flush() => 
            Stream.Flush();
    }
}
