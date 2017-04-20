using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System;
using System.IO;

namespace Microsoft.Rest.ClientRuntime.Test.Log
{
    public static class Ex
    {
        public static Action<string> ToLog(this Stream stream)
            => v =>
            {
                stream.WriteUtf8(v);
                stream.Flush();
            };

        public static Io WithLog(this Io io, Action<string> log)
            => new Io(new ReaderAndLog(io.Reader, log), new WriterAndLog(io.Writer, log));

        public static Io WithLog(this Io io, Stream stream)
            => io.WithLog(stream.ToLog());
    }
}
