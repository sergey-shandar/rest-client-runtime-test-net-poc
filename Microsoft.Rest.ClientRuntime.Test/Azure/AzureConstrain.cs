using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public abstract class AzureConstrain
    {
        public abstract void Validate(object value);
    }
}
