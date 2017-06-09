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

        public static AzureParam CreateConst(string name, AzureParamLocation location, object value)
            => new AzureParam(
                info: new AzureParamInfo(
                    name: name,
                    location: location,
                    isRequired: false,
                    constraints: new Constraints.AzureConstraint[0]),
                value: value);
    }
}
