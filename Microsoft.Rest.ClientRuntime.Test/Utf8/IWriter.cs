namespace Microsoft.Rest.ClientRuntime.Test.Utf8
{
    public interface IWriter
    {
        IWriter Write(string value);
        void Flush();
    }
}
