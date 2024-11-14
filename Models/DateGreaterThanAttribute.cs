using System.ComponentModel.DataAnnotations;

namespace ST10251759_PROG6212_POE.Models
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonProperty == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime?)comparisonProperty.GetValue(validationContext.ObjectInstance);

            // Check for null values first
            if (!currentValue.HasValue || !comparisonValue.HasValue)
                return ValidationResult.Success;

            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage ?? "End Date must be after Start Date.");

            return ValidationResult.Success;
        }
    }

}
