namespace Microsoft.Rest.ClientRuntime.Test.Azure.Constraints
{
    public sealed class AzureMinLength : AzureConditionConstraint<int>
    {
        public override string Rule => ValidationRules.MinLength;

        public override bool Condition(string v)
            => v.Length >= Value;

        public AzureMinLength(int value) : base(value) { }
    }
}
