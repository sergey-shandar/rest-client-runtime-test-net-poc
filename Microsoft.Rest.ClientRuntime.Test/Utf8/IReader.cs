namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public interface IReader
    {
        string ReadLine();

        string ReadBlock(int length);
    }
}
