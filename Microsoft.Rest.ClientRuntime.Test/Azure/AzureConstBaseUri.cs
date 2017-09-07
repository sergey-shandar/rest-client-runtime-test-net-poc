using System;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureConstBaseUri : AzureBaseUri
    {
        public Uri Value { get; }

        public AzureConstBaseUri(Uri value)
        {
            Value = value;
        }

        public override Uri GetUri(IAzureRequest request)
            => Value;
    }
}
