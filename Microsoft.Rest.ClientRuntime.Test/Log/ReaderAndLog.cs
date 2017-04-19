using System;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;

namespace Microsoft.Rest.ClientRuntime.Test.Log
{
    public sealed class ReaderAndLog : IReader
    {
        private readonly IReader _Reader;

        private readonly Action<string> _Log;

        public ReaderAndLog(IReader reader, Action<string> log)
        {
            _Reader = reader;
            _Log = log;
        }

        public ReaderAndLog(IReader reader, Stream stream): this(reader, stream.ToLog())
        { 
        }

        public string ReadBlock(int length)
        {
            var result = _Reader.ReadBlock(length);
            _Log(result);
            return result;
        }

        public string ReadLine()
        {
            var result = _Reader.ReadLine();
            _Log(result + Writer.Eol);
            return result;
        }
    }
}
