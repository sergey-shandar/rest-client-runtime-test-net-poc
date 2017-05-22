using System;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Log
{
    public sealed class ReaderAndLog : IReader
    {
        private readonly IReader _Reader;

        private readonly Func<string, Task> _Log;

        public ReaderAndLog(IReader reader, Func<string, Task> log)
        {
            _Reader = reader;
            _Log = log;
        }

        public ReaderAndLog(IReader reader, Stream stream): this(reader, stream.ToLog())
        { 
        }

        public async Task<string> ReadBlockAsync(int length)
        {
            var result = await _Reader.ReadBlockAsync(length);
            await _Log(result);
            return result;
        }

        public async Task<string> ReadLineAsync()
        {
            var result = await _Reader.ReadLineAsync();
            await _Log(result + Writer.Eol);
            return result;
        }
    }
}
