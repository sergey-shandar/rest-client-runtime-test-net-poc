namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public abstract class Message
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }

        protected Message(int id)
        {
            this.jsonrpc = "2.0";
            this.id = id;
        }
    }
}
