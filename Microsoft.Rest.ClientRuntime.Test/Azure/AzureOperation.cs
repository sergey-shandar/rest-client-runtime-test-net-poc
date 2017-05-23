using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public class AzureOperation<Response>
    {
        public string Title { get; }

        public string Id { get; }

        public string SubscriptionId { get; }

        public IEnumerable<AzureParam> ParamList { get; }

        public AzureOperation(
            string title,
            string id,
            string subscriptionId,
            IEnumerable<AzureParam> paramList)
        {
            Title = title;
            Id = id;
            SubscriptionId = subscriptionId;
            ParamList = paramList;
        }
    }
}
