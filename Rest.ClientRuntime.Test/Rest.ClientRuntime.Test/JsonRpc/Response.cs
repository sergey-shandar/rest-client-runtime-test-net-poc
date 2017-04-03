namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    class Response : Message
    {
        public Error error { get; set; }
    }

    class Response<T> : Response
    {
        public T result { get; set; }
    }
}
