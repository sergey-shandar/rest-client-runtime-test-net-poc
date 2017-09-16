using System.Net.Http;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Error<E>
    {
        public int code { get; set; }

        public string message { get; set; }

        public E data { get; set; } 

        public HttpResponseMessage HttpResponse { get; set; }
    }
}
