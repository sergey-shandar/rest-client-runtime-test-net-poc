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

        public static void ReadBuffer(this Stream stream, byte[] array, int offset, int length)
        {
            while (0 < length)
            {
                var count = stream.Read(array, offset, length);
                offset += count;
                length -= count;
            }
        }
    }
}
