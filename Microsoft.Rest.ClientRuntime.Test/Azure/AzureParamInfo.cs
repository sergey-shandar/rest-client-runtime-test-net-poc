using Microsoft.Rest.ClientRuntime.Test.Azure.Constraints;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureParamInfo
    {
        public string Name { get; }

        public AzureParamLocation Location { get; }

        public AzureConstraint[] Constraints { get; }

        public bool IsRequired { get; }

        public bool SkipUrlEncoding { get; }

        public bool IsStream { get; }

        public AzureParamInfo(
            string name,
            AzureParamLocation location,            
            bool isRequired = false,
            AzureConstraint[] constraints = null,
            bool skipUrlEncoding = false,
            bool isStream = false)
        {
            Name = name;
            Location = location;
            IsRequired = isRequired;
            Constraints = constraints.DefaultIfNull();
            SkipUrlEncoding = skipUrlEncoding;
            IsStream = isStream;
        }
    }
}
