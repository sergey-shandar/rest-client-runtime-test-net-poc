using System.IO;

namespace Microsoft.Rest.ClientRuntime.Test
{
    public static class StreamEx
    {
        public static void Write(this Stream stream, byte[] array)
            => stream.Write(array, 0, array.Length);
    }
}
