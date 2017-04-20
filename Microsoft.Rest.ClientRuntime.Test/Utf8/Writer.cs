using System.IO;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public sealed class Writer : IWriter
    {
        public Stream Stream { get; }

        public const string Eol = "\r\n"; 

        public Writer(Stream stream)
        {
            Stream = stream;
        }

        public IWriter Write(string value)
        {
            Stream.WriteUtf8(value);
            return this;
        }

        public void Flush() => 
            Stream.Flush();
    }
}
