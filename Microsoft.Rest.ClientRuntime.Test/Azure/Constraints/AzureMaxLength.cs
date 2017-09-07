namespace Microsoft.Rest.ClientRuntime.Test.Azure.Constraints
{
    public sealed class AzureMaxLength : AzureConditionConstraint<int>
    {
        public override string Rule => ValidationRules.MaxLength;

        public override bool Condition(string v)
            => v.Length <= Value;

        public AzureMaxLength(int value) : base(value) { }
    }
}
