using Rest.ClientRuntime.Test.Rpc;
using System.Text;

namespace Rest.ClientRuntime.Test.TextRpc
{
    /// <summary>
    /// Utilities to form and send messages in text format. The format of the message is
    /// <code>
    /// Content-Length: ${MessageLength}
    /// 
    /// ${Message}
    /// </code>
    /// </summary>
    public static class StreamEx
    {
        private const string ContentLength = "Content-Length";

        public static string ReadMessage(this Utf8Reader reader)
        {
            var line = reader.ReadLine();
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
                var line2 = reader.ReadLine();
                if (line2.Trim() == string.Empty)
                {
                    break;
                }
            }

            var size = int.Parse(splitArray[1].Trim());
            return reader.ReadBlock(size);
        }

        public static void WriteMessage(this Utf8Writer writer, string message)
        {
            var count = Encoding.UTF8.GetByteCount(message);
            writer
                .Write(ContentLength)
                .Write(":")
                .WriteLine(count.ToString())
                .WriteLine()
                .Write(message);
        }

        public static T ReadMessage<T>(this Utf8Reader reader, IMarshalling marshalling)
            => marshalling.Deserialize<T>(reader.ReadMessage());

        public static void WriteMessage(
            this Utf8Writer writer, IMarshalling marshalling, object value)
            => writer.WriteMessage(marshalling.Serialize(value) + Utf8Writer.Eol);
    }
}
