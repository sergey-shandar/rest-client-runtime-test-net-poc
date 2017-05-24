using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public class AzureRequest
    {
        public string Title { get; }

        public System.Uri BaseUri { get; }

        public string Id { get; }

        public string Method { get; }

        public string SubscriptionId { get; }

        public IEnumerable<AzureParam> ParamList { get; }

        public AzureRequest(
            string title,
            System.Uri baseUri,
            string id,
            string method,
            string subscriptionId,
            IEnumerable<AzureParam> paramList)
        {
            Title = title;
            BaseUri = baseUri;
            Id = id;
            Method = method;
            SubscriptionId = subscriptionId;
            ParamList = paramList;
        }
    }
}
