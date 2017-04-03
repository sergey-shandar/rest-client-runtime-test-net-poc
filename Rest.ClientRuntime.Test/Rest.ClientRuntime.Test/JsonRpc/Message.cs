namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public abstract class Message
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }
    }
}
