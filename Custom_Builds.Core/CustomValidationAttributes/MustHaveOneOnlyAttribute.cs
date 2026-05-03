using System;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.CustomValidationAttributes
{
    public class MustHaveOneOnlyAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public MustHaveOneOnlyAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
            var otherProperty = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

            if (otherProperty == null && value == null)
            {
                return new ValidationResult(
                    string.Format(
                        ErrorMessage ?? $"must at least have {validationContext.DisplayName} or {_otherPropertyName}" ,
                        validationContext.DisplayName, _otherPropertyName
                        )
                    );
            }

            if (otherProperty != null && value != null)
            {
                return new ValidationResult(
                    string.Format(
                        ErrorMessage ?? $"cannot have both {validationContext.DisplayName} and {_otherPropertyName}",
                        validationContext.DisplayName, _otherPropertyName
                        )
                    );
            }

            return ValidationResult.Success;
        }
    }
}