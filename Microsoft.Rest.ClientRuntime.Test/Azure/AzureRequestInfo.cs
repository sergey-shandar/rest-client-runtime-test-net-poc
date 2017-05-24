using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureRequestInfo
    {
        public string Title { get; }

        public string Id { get; }

        public string Method { get; }

        public AzureRequestInfo(string title, string id, string method)
        {
            Title = title;
            Id = id;
            Method = method;
        }
    }
}
