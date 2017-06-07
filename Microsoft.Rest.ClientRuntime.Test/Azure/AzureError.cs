using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureError
    {
        public string Message { get; }
        public HttpRequestMessageWrapper Request { get; }
        public HttpResponseMessageWrapper Response { get; }

        public AzureError(
            string message,
            HttpRequestMessageWrapper request,
            HttpResponseMessageWrapper response)
        {
            Message = message;
            Request = request;
            Response = response;
        }
    }
}
