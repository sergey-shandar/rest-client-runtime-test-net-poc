using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureConstBaseUri : AzureBaseUri
    {
        public Uri Value { get; }

        public AzureConstBaseUri(Uri value)
        {
            Value = value;
        }

        public override Uri GetUri<E>(AzureRequest<E> request)
            => Value;
    }
}
