using System.IO;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test
{
    public static class StreamEx
    {
        public static void Write(this Stream stream, byte[] array)
            => stream.Write(array, 0, array.Length);

        public static void WriteUtf8(this Stream stream, string value)
            => stream.Write(Encoding.UTF8.GetBytes(value));
    }
}
