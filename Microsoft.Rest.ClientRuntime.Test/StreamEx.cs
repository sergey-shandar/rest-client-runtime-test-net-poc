using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test
{
    public static class StreamEx
    {
        public static async Task<int> ReadByteAsync(this Stream stream)
        {
            var buffer = new byte[1];
            var count = await stream.ReadAsync(buffer, 0, 1);            
            return count == 1 ? buffer[0] : -1;
        }

        public static Task WriteAsync(this Stream stream, byte[] array)
            => stream.WriteAsync(array, 0, array.Length);

        public static Task WriteUtf8Async(this Stream stream, string value)
            => stream.WriteAsync(Encoding.UTF8.GetBytes(value));

        public static async Task ReadBufferAsync(this Stream stream, byte[] array, int offset, int length)
        {
            while (0 < length)
            {
                var count = await stream.ReadAsync(array, offset, length);
                offset += count;
                length -= count;
            }
        }
    }
}
