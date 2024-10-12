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

using Microsoft.AspNetCore.Http; // Importing the namespace for HTTP-related functionalities, including file uploads
using System.Collections.Generic; // Importing the namespace for generic collections like List
using System.ComponentModel.DataAnnotations; // Importing the namespace for data annotations used for validation

namespace ST10251759_PROG6212_POE.Models // Defining the namespace for the models used in the project
{
    // Defining a view model class named ClaimViewModel
    // This class is used to transfer data between the view and the controller
    public class ClaimViewModel
    {
        // Property to hold the number of hours worked by the user
        // The Required attribute enforces that this field must be filled out
        [Required(ErrorMessage = "Hours Worked is required.")] // Custom error message if validation fails
        [Range(1, 100, ErrorMessage = "Hours Worked must be between 1 and 100.")] // Validates that the value must be between 1 and 100
        public decimal HoursWorked { get; set; } // Decimal property for storing hours worked

        // Property to hold the user's hourly rate
        // The Required attribute enforces that this field must be filled out
        [Required(ErrorMessage = "Hourly Rate is required.")] // Custom error message if validation fails
        [Range(50, 1000, ErrorMessage = "Hourly Rate must be between 50 and 1000.")] // Validates that the value must be between 50 and 1000
        public decimal HourlyRate { get; set; } // Decimal property for storing the hourly rate

        // Property to hold additional notes related to the claim
        // The MaxLength attribute restricts the length of the notes to a maximum of 500 characters
        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")] // Custom error message if validation fails
        public string Notes { get; set; } // String property for storing notes

        // Property to hold a list of supporting documents for the claim
        // This property will accept a list of files uploaded by the user
        [Display(Name = "Supporting Documents")] // This attribute specifies the display name for the property in the view
        public List<IFormFile> SupportingDocuments { get; set; } // List of IFormFile to hold uploaded documents
    }
}
