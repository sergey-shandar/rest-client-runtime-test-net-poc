namespace Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Response<T> : Message
    {
        public T result { get; set; }
        public Error error { get; set; }
    }
}
