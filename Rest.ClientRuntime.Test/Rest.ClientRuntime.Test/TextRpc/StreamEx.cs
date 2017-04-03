using Microsoft.Rest.ClientRuntime.Test.Rpc;
using System.IO;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.TextRpc
{
    public static class StreamEx
    {
        private const string ContentLength = "Content-Length";

        private const string Eol = "\n\r";

        public static string ReadMessage(this StreamReader reader)
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
            var buffer = new char[size];
            reader.Read(buffer, 0, size);
            return new string(buffer);
        }

        public static T ReadMessage<T>(this StreamReader reader, IMarshalling marshalling)
            => marshalling.Deserialize<T>(reader.ReadMessage());

        public static void WriteMessage(this StreamWriter writer, string message)
        {
            writer.Write(message);
        }

        public static void WriteMessage(this StreamWriter writer, IMarshalling marshalling, object value)
            => writer.WriteMessage(marshalling.Serialize(value) + Eol);
    }
}
