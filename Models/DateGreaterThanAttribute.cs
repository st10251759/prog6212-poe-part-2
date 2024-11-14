/*
 --------------------------------Student Information----------------------------------
 STUDENT NO.: ST10251759
 Name: Cameron Chetty
 Course: BCAD Year 2
 Module: Programming 2B
 Module Code: PROG6212
 Assessment: Portfolio of Evidence (POE) Part 3
 Github repo link: https://github.com/st10251759/prog6212-poe-part-2
 --------------------------------Student Information----------------------------------

 ==============================Code Attribution==================================

 Attributes
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/TaskManager_Attributes_demo/TaskManager_Attributes_demo
 Date Accessed: 11 October 2024

 MVC APP
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/blob/main/EmployeeLeaveManagement_G1.zip
 Date Accessed: 11 October 2024

 ==============================Code Attribution==================================

 */

using System.ComponentModel.DataAnnotations; // Importing data annotations namespace for creating custom validation attributes

namespace ST10251759_PROG6212_POE.Models // Defining the namespace for the project models
{
    // Defining a custom validation attribute class named DateGreaterThanAttribute
    // This attribute is used to ensure that a date property is greater than (after) another specified date property
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        // Private field to store the name of the property to compare against
        private readonly string _comparisonProperty;

        // Constructor that accepts the name of the comparison property as a parameter
        // This property is the one the decorated property will be validated against
        public DateGreaterThanAttribute(string comparisonProperty)
        {
            // Assign the comparison property name to the private field
            _comparisonProperty = comparisonProperty;
        }

        // Overriding the IsValid method to perform custom validation logic
        // This method is called by the validation framework during model validation
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Attempt to cast the value being validated to a nullable DateTime type
            // This represents the current property value to validate (e.g., an End Date)
            var currentValue = (DateTime?)value;

            // Retrieve the property information of the comparison property using reflection
            // The comparison property is the one specified in the attribute (e.g., Start Date)
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

            // If the comparison property does not exist, throw an exception
            // This ensures the attribute is applied correctly to existing properties only
            if (comparisonProperty == null)
                throw new ArgumentException("Property with this name not found");

            // Retrieve the value of the comparison property from the object being validated
            var comparisonValue = (DateTime?)comparisonProperty.GetValue(validationContext.ObjectInstance);

            // Check for null values in both the current property and the comparison property
            // If either is null, skip the validation and consider it valid (return success)
            if (!currentValue.HasValue || !comparisonValue.HasValue)
                return ValidationResult.Success;

            // Perform the comparison: check if the current property value is less than or equal to the comparison value
            // If it is, return a validation error with a specified error message
            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage ?? "End Date must be after Start Date.");

            // If all checks pass, the date is valid, and return success
            return ValidationResult.Success;
        }
    }
}
