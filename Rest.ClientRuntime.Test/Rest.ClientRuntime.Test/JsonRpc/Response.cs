namespace Rest.ClientRuntime.Test.JsonRpc
{
    sealed class Response<T> : Message
    {
        public T result { get; set; }
        public Error error { get; set; }
    }
}
