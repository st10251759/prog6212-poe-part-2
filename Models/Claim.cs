/*
 --------------------------------Student Information----------------------------------
 STUDENT NO.: ST10251759
 Name: Cameron Chetty
 Course: BCAD Year 2
 Module: Programming 2B
 Module Code: CLDV6212
 Assessment: Portfolio of Evidence (POE) Part 2
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

//Import List

using System.ComponentModel.DataAnnotations; // Importing data annotation attributes for model validation
using System.ComponentModel.DataAnnotations.Schema; // Importing attributes for defining database schema details
using System.Reflection.Metadata; // Importing metadata features, although this is not used in the current code

namespace ST10251759_PROG6212_POE.Models // Defining a namespace for the project models
{
    // Defining a Claim class to represent a claim in the application
    public class Claim
    {
        // ClaimId property: a unique identifier for each claim
        public int ClaimId { get; set; }

        //The main assumpt is that Lecturers will enter their Hourly rate, submit supporting documents that verify the hourly rate and Porgramme Coordinators and Academic Managers will verify eveidence - to Aprove and Reject the claim - hence hourly rate is not populated - otis stiputlated to change
        // HoursWorked property: represents the number of hours worked
        [Required(ErrorMessage = "Hours Worked is required.")] // Validation to ensure this field is not empty
        [System.ComponentModel.DataAnnotations.Range(1, 150, ErrorMessage = "Hours Worked must be between 1 and 150.")] // Validation to ensure the value is between 1 and 100  - It is assumped that Lectures can not work more than 160 hours a month
        public decimal HoursWorked { get; set; }

        // HourlyRate property: represents the rate per hour for the claim
        [Required(ErrorMessage = "Hourly Rate is required.")] // Validation to ensure this field is not empty
        [System.ComponentModel.DataAnnotations.Range(200, 1000, ErrorMessage = "Hourly Rate must be between 200 and 1000.")] // Validation to ensure the value is between 50 and 1000 - It is assumpted Lecturers Hourly rate is between R200 and R1000 an hour
        public decimal HourlyRate { get; set; }

        // TotalAmount property: the total amount to be paid for the claim, calculated from HoursWorked and HourlyRate
        [Required] // Validation to ensure this field is not empty
        public decimal TotalAmount { get; set; }

        // Notes property: additional notes related to the claim, with a maximum length
        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")] // Validation to restrict length to 500 characters
        public string Notes { get; set; }

        // DateSubmitted property: the date the claim was submitted
        [Required] // Validation to ensure this field is not empty
        [CustomValidation(typeof(Claim), nameof(ValidateSubmissionDate))] // Custom validation to check the submission date
        public DateTime DateSubmitted { get; set; }

        // Status property: tracks the current status of the claim, defaulting to "Pending"
        public string Status { get; set; } = "Pending";

        // Approval tracking properties to indicate if the claim has been approved by the coordinator and manager
        public bool IsApprovedByCoordinator { get; set; } = false; // Default is false (not approved)
        public bool IsApprovedByManager { get; set; } = false; // Default is false (not approved)

        // ApplicationUserId property: foreign key linking the claim to the user who submitted it
        [ForeignKey("ApplicationUser")] // Indicates that this property is a foreign key
        public string ApplicationUserId { get; set; } // User ID of the person making the claim

        // Navigation property: establishes a relationship with the ApplicationUser model
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Documents property: a collection of documents associated with the claim
        public virtual ICollection<Document> Documents { get; set; } // Allows for multiple documents to be linked to a claim

        // Custom validation method for the DateSubmitted property
        public static ValidationResult ValidateSubmissionDate(DateTime dateSubmitted, ValidationContext context)
        {
            var currentDate = DateTime.Now; // Get the current date and time
            // Check if the submitted date is in the future
            if (dateSubmitted > currentDate)
            {
                return new ValidationResult("Date Submitted cannot be in the future."); // Return an error if it is
            }

            // Check if the submitted date is within the current or previous month
            if (dateSubmitted.Month != currentDate.Month && dateSubmitted.Month != currentDate.AddMonths(-1).Month)
            {
                return new ValidationResult("Date Submitted can only be from the current month or previous month."); // Return an error if it isn't
            }

            return ValidationResult.Success; // Return success if all validations pass
        }
    }
}
