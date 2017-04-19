namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public static class Ex
    {
        public static IWriter WriteLine(this IWriter writer)
            => writer.Write(Writer.Eol);

        public static IWriter WriteLine(this IWriter writer, string value)
            => writer.Write(value).WriteLine();
    }
}
