using System;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public abstract class AzureBaseUri
    {
        public abstract Uri GetUri(IAzureRequest request);
    }
}
