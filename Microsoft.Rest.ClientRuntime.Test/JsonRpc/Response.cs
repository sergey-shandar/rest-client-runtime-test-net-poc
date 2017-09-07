namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public static class Response
    {
        public static Response<T, E> Create<T, E>(string id, T result, Error<E> error)
            => new Response<T, E>(id, result, error);
    }

    public sealed class Response<T, E> : Message
    {
        public T result { get; set; }
        public Error<E> error { get; set; }

        public Response(string id, T result, Error<E> error): base(id)
        {
            this.result = result;
            this.error = error;
        }
    }
}
