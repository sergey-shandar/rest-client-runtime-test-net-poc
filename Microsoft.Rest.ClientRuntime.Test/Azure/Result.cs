using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class Result<T>
    {
        public int statusCode { get; set; }

        public Dictionary<string, List<string>> headers { get; set; }

        public T response { get; set; }
    }
}
