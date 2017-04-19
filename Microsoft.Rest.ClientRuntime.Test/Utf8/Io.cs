namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public sealed class Io
    {
        public IReader Reader { get; }

        public IWriter Writer { get; }

        public Io(IReader reader, IWriter writer)
        {
            Reader = reader;
            Writer = writer;
        }
    }
}
