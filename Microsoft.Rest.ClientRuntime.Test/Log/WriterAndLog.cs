using System;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Log
{
    public sealed class WriterAndLog : IWriter
    {
        private readonly IWriter _Writer;

        private Func<string, Task> _Log;

        public WriterAndLog(IWriter writer, Func<string, Task> log)
        {
            _Writer = writer;
            _Log = log;
        }

        public WriterAndLog(IWriter writer, Stream stream): this(writer, stream.ToLog())
        {
        }

        public Task FlushAsync()
            => _Writer.FlushAsync();

        public async Task WriteAsync(string value)
        {
            await _Log(value);
            await _Writer.WriteAsync(value);
        }
    }
}
