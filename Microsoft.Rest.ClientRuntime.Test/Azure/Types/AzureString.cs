using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Rest.ClientRuntime.Test.Azure.Types
{
    public sealed class AzureString : AzureType
    {
        public int? MinLength { get; }

        public int? MaxLength { get; }

        public string Pattern { get; }

        public AzureString(
            int? minLength,
            int? maxLength,
            string pattern)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            Pattern = pattern;
        }

        public override void Validate(AzureParam p)
        {
            var v = p.Value.ToString();

            if (MaxLength != null && v.Length > MaxLength)
            {
                throw new ValidationException(ValidationRules.MaxLength, p.Info.Name, MaxLength);
            }

            if (MinLength != null && v.Length < MinLength)
            {
                throw new ValidationException(ValidationRules.MinLength, p.Info.Name, MinLength);
            }

            if (Pattern != null && !Regex.IsMatch(v, Pattern))
            {
                throw new ValidationException(ValidationRules.Pattern, p.Info.Name, Pattern);
            }
        }
    }
}
