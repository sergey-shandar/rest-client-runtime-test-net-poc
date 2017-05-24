using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureRequest
    {
        public AzureRequestInfo Info { get; }

        public System.Uri BaseUri { get; }

        public string SubscriptionId { get; }

        public IEnumerable<AzureParam> ParamList { get; }

        public AzureRequest(
            AzureRequestInfo info,
            System.Uri baseUri,
            string subscriptionId,
            IEnumerable<AzureParam> paramList)
        {
            Info = info;
            BaseUri = baseUri;
            SubscriptionId = subscriptionId;
            ParamList = paramList;
        }
    }
}
