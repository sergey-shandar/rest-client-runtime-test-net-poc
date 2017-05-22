using Microsoft.Rest.ClientRuntime.Test.Rpc;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.TextRpc
{
    /// <summary>
    /// Utilities to form and send messages in a text format. The format of the messages is
    /// <code>
    /// Content-Length: ${MessageLength}
    /// 
    /// ${Message}
    /// </code>
    /// </summary>
    public static class StreamEx
    {
        private const string ContentLength = "Content-Length";

        public static async Task<string> ReadMessageAsync(this IReader reader)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            var splitArray = line.Split(':');
            if (splitArray.Length != 2 || splitArray[0].Trim() != ContentLength)
            {
                return null;
            }

            // read until the first empty line.
            while (true)
            {
                var line2 = await reader.ReadLineAsync();
                if (line2.Trim() == string.Empty)
                {
                    break;
                }
            }

            var size = int.Parse(splitArray[1].Trim());
            return await reader.ReadBlockAsync(size);
        }

        public static async Task WriteMessageAsync(this IWriter writer, string message)
        {
            var count = Encoding.UTF8.GetByteCount(message);
            await writer.WriteAsync(ContentLength);
            await writer.WriteAsync(":");
            await writer.WriteLineAsync(count.ToString());
            await writer.WriteLineAsync();
            await writer.WriteAsync(message);
            await writer.FlushAsync();
        }

        public static async Task<T> ReadMessageAsync<T>(this IReader reader, IMarshalling marshalling)
            => marshalling.Deserialize<T>(await reader.ReadMessageAsync());

        public static Task WriteMessageAsync(
            this IWriter writer, IMarshalling marshalling, object value)
            => writer.WriteMessageAsync(marshalling.Serialize(value) + Writer.Eol);
    }
}
