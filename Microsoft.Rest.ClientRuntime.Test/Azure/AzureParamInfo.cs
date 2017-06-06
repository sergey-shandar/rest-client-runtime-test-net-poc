using Microsoft.Rest.ClientRuntime.Test.Azure.Types;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureParamInfo
    {
        public string Name { get; }

        public AzureParamLocation Location { get; }

        public AzureType Type { get; }

        public bool IsRequired { get; }

        public AzureParamInfo(
            string name,
            AzureParamLocation location,
            AzureType type,
            bool isRequired)
        {
            Name = name;
            Location = location;
            Type = type;
            IsRequired = isRequired;
        }
    }
}
