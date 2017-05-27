using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureParamBaseUri : AzureBaseUri
    {
        public AzurePathPart[] PartList { get; }

        public AzureParamBaseUri(AzurePathPart[] partList)
        {
            PartList = partList;
        }

        public override Uri GetUri(AzureRequest request)
            => new Uri(request.GetPath(PartList));
    }
}
