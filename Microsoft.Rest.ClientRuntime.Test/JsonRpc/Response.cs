namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public static class Response
    {
        public static Response<T> Create<T>(string id, T result, Error error)
            => new Response<T>(id, result, error);
    }

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
