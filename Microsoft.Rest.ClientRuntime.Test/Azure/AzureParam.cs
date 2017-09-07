namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public sealed class AzureParam
    {
        public AzureParamInfo Info { get; }

        public object Value { get; }

        public AzureParam(AzureParamInfo info, object value)
        {
            Info = info;
            Value = value;
        }
    }
}
