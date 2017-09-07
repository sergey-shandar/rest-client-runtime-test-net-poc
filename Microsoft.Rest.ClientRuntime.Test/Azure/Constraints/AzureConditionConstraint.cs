namespace Microsoft.Rest.ClientRuntime.Test.Azure.Constraints
{
    public abstract class AzureConditionConstraint<T> : AzureConstraint
    {
        public T Value { get; }

        public override void Validate(AzureParam p)
        {
            var v = p.Value.ToString();
            if (!Condition(v))
            {
                throw new ValidationException(Rule, p.Info.Name, Value);
            }
        }

        public abstract bool Condition(string v);

        public abstract string Rule { get; }

        protected AzureConditionConstraint(T value)
        {
            Value = value;
        }
    }
}
