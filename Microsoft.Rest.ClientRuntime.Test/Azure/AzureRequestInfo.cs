using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureRequestInfo
    {
        public string Title { get; }

        public string Id { get; }

        public string Method { get; }

        public IEnumerable<AzurePathPart> Path { get; }

        public AzureRequestInfo(string title, string id, string method, IEnumerable<AzurePathPart> path)
        {
            Title = title;
            Id = id;
            Method = method;
        }
    }
}
