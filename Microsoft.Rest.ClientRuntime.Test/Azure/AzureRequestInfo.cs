using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureRequestInfo
    {
        public string Title { get; }

        public string Id { get; }

        public string Method { get; }

        public IEnumerable<AzurePathPart> Path { get; }

        public IEnumerable<AzureParam> ConstList { get; }

        public bool IsLongRunningOperation { get; }

        public Func<AzureError, RestException> CreateException { get; }

        public AzureRequestInfo(
            string title,
            string id,
            string method,
            IEnumerable<AzurePathPart> path,
            IEnumerable<AzureParam> constList,
            Func<AzureError, RestException> createException,
            bool isLongRunningOperation)
        {
            Title = title;
            Id = id;
            Method = method;
            Path = path;
            ConstList = constList;
            CreateException = createException;
            IsLongRunningOperation = isLongRunningOperation;
        }
    }
}
