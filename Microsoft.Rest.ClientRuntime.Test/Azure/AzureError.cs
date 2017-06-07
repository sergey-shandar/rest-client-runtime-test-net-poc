namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureError<E>
    {
        public string Message { get; }
        public HttpRequestMessageWrapper Request { get; }
        public HttpResponseMessageWrapper Response { get; }
        public E Body { get; }

        public AzureError(
            string message,
            HttpRequestMessageWrapper request,
            HttpResponseMessageWrapper response,
            E body)
        {
            Message = message;
            Request = request;
            Response = response;
            Body = body;
        }
    }
}
