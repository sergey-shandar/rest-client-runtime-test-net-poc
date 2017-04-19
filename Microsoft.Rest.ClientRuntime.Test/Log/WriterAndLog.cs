using System;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.IO;

namespace Microsoft.Rest.ClientRuntime.Test.Log
{
    public sealed class WriterAndLog : IWriter
    {
        private readonly IWriter _Writer;

        private Action<string> _Log;

        public WriterAndLog(IWriter writer, Action<string> log)
        {
            _Writer = writer;
            _Log = log;
        }

        public WriterAndLog(IWriter writer, Stream stream): this(writer, stream.ToLog())
        {
        }

        public void Flush()
        {
            _Writer.Flush();
        }

        public IWriter Write(string value)
        {
            _Log(value);
            _Writer.Write(value);
            return this;
        }
    }
}
