using System.IO;
using System.Threading.Tasks;

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

        public Task WriteAsync(string value)
            => Stream.WriteUtf8Async(value);

        public Task FlushAsync()
            => Stream.FlushAsync();
    }
}
