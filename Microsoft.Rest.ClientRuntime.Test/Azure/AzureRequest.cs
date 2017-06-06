using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureRequest
    {
        public AzureRequestInfo Info { get; }

        public AzureBaseUri BaseUri { get; }

        public IEnumerable<AzureParam> ParamList { get; }

        public Dictionary<string, List<string>> CustomHeaders { get; }

        public CancellationToken CancellationToken { get; }

        public IEnumerable<AzureParam> ConstAndParamList
            => Info.ConstList.Concat(ParamList);

        public Uri GetBaseUri()
            => BaseUri.GetUri(this);

        public AzureRequest(
            AzureRequestInfo info,
            AzureBaseUri baseUri,
            IEnumerable<AzureParam> paramList,
            Dictionary<string, List<string>> customHeaders,
            CancellationToken cancellationToken)
        {
            Info = info;
            BaseUri = baseUri;
            ParamList = paramList;
            CustomHeaders = customHeaders;
            CancellationToken = cancellationToken;
        }
    }
}
