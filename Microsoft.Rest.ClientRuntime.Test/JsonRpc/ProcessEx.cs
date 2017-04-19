using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.Diagnostics;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public static class ProcessEx
    {
        public static Io CreateIo(this Process process)
            => new Io(
                new Reader(process.StandardOutput.BaseStream),
                new Writer(process.StandardInput.BaseStream));
    }
}
