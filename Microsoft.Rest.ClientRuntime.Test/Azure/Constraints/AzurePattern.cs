using System.Text.RegularExpressions;

namespace Microsoft.Rest.ClientRuntime.Test.Azure.Constraints
{
    public sealed class AzurePattern : AzureConditionConstraint<string>
    {
        public override string Rule => ValidationRules.Pattern;

        public override bool Condition(string v)
            => Regex.IsMatch(v, Value);

        public AzurePattern(string value) : base(value) { }
    }
}
