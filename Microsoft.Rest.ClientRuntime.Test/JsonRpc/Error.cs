namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; } 
    }
}
