using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure.Constraints
{
    public abstract class AzureConstraint
    {
        public abstract void Validate(AzureParam value);
    }
}
