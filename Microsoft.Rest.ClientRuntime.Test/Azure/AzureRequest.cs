using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureRequest
    {
        public AzureRequestInfo Info { get; }

        public AzureBaseUri BaseUri { get; }

        public IEnumerable<AzureParam> ParamList { get; }

        public IEnumerable<AzureParam> ConstAndParamList
            => Info.ConstList.Concat(ParamList);

        public Uri GetBaseUri()
            => BaseUri.GetUri(this);

        public AzureRequest(
            AzureRequestInfo info,
            AzureBaseUri baseUri,
            IEnumerable<AzureParam> paramList)
        {
            Info = info;
            BaseUri = baseUri;
            ParamList = paramList;
        }
    }
}
