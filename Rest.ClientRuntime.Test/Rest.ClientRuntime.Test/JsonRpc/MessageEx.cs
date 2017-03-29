using System.IO;
using System.Text;

namespace Rest.ClientRuntime.Test.JsonRpc
{
    public static class MessageEx
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

        public static void WriteMessage(this Stream stream, string message)
        {
            var writer = new StreamWriter(stream, Encoding.UTF8);
            var count = Encoding.UTF8.GetByteCount(message);
            writer.Write(ContentLength);
            writer.Write(":");
            writer.WriteLine(count.ToString());
            writer.WriteLine();
            writer.Write(message);
            writer.Flush();
        }
    }
}
