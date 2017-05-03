using System.Collections.Generic;
using System.Net;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class Result<T>
    {
        public HttpStatusCode statusCode { get; set; }

        public Dictionary<string, List<string>> headers { get; set; }

        public T response { get; set; }
    }
}
