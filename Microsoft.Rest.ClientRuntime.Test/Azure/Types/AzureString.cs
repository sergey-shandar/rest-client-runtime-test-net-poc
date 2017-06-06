using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.Azure.Types
{
    public sealed class AzureString : AzureType
    {
        public int? MinLength { get; }

        public int? MaxLength { get; }

        public string Pattern { get; }

        public AzureString(
            int? minLength,
            int? maxLength,
            string pattern)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            Pattern = pattern;
        }
    }
}
