namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureParamInfo
    {
        public string Name { get; }

        public AzureParamLocation Location { get; }

        public AzureParamInfo(string name, AzureParamLocation location)
        {
            Name = name;
            Location = location;
        }
    }
}
