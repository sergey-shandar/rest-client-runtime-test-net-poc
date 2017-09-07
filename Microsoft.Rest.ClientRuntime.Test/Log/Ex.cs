using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Log
{
    public static class Ex
    {
        public static Func<string, Task> ToLog(this Stream stream)
            => async (v) =>
            {
                await stream.WriteUtf8Async(v);
                await stream.FlushAsync();
            };

        public static Io WithLog(this Io io, Func<string, Task> log)
            => new Io(new ReaderAndLog(io.Reader, log), new WriterAndLog(io.Writer, log));

        public static Io WithLog(this Io io, Stream stream)
            => io.WithLog(stream.ToLog());
    }
}
