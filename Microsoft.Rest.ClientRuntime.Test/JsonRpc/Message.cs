namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public abstract class Message
    {
        public string jsonrpc { get; set; }

        public string id { get; set; }

        protected Message(string id)
        {
            this.jsonrpc = "2.0";
            this.id = id;
        }
    }
}
