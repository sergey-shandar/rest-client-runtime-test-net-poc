namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Response<T> : Message
    {
        public T result { get; set; }
        public Error error { get; set; }

        public Response(string id, T result, Error error): base(id)
        {
            this.result = result;
            this.error = error;
        }
    }
}
