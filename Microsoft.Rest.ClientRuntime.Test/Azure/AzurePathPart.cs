namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzurePathPart
    {
        public string Value { get; }

        public bool IsParam { get; }

        public AzurePathPart(string value, bool isParam)
        {
            Value = value;
            IsParam = isParam;
        }
    }
}
