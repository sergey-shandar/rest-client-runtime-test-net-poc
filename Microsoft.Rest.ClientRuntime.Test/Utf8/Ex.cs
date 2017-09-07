using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public static class Ex
    {
        public static Task WriteLineAsync(this IWriter writer)
            => writer.WriteAsync(Writer.Eol);

        public static async Task WriteLineAsync(this IWriter writer, string value)
        {
            await writer.WriteAsync(value);
            await writer.WriteLineAsync();
        }
    }
}
