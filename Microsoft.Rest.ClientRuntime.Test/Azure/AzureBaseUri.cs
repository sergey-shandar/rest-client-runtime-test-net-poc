using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public abstract class AzureBaseUri
    {
        public abstract Uri GetUri<E>(AzureRequest<E> request);
    }
}
