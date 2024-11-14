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

// Import List

using Microsoft.AspNetCore.Http; // Importing the namespace for HTTP-related functionalities, including file uploads
using System.Collections.Generic; // Importing the namespace for generic collections like List
using System.ComponentModel.DataAnnotations; // Importing the namespace for data annotations used for validation

namespace ST10251759_PROG6212_POE.Models // Defining the namespace for the models used in the project
{
    // Defining a view model class named ClaimViewModel
    // This class is used to transfer data between the view and the controller
    public class ClaimViewModel
    {
        // The HoursWorked property stores the number of hours worked for the claim.
        // It is marked as required and must be between 1 and 150 hours.
        [Required(ErrorMessage = "Hours Worked is required.")] // Ensures that the field is not empty and provides a custom error message if not filled
        [Range(1, 150, ErrorMessage = "Hours Worked must be between 1 and 150.")] // Specifies a valid range for the number of hours worked
        public decimal HoursWorked { get; set; }

        // The HourlyRate property represents the hourly rate for the work done.
        // It is marked as required and must be between 200 and 1000 units (e.g., currency).
        [Required(ErrorMessage = "Hourly Rate is required.")] // Ensures that the field is not empty and provides a custom error message if not filled
        [Range(200, 1000, ErrorMessage = "Hourly Rate must be between 200 and 1000.")] // Specifies a valid range for the hourly rate
        public decimal HourlyRate { get; set; }

        // The Notes property stores additional information related to the claim.
        // It is a string with a maximum length of 500 characters.
        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")] // Restricts the length of the string to 500 characters and provides an error message if exceeded
        public string Notes { get; set; }

        // The SupportingDocuments property holds a list of files that support the claim.
        // These are uploaded as files and represented by the IFormFile interface, which is commonly used for handling file uploads in ASP.NET Core.
        [Display(Name = "Supporting Documents")] // Provides a display name for the property, which can be used in the UI
        public List<IFormFile> SupportingDocuments { get; set; } // A list of files representing the supporting documents for the claim

        // The StartDate property stores the start date of the work period for the claim.
        // It is marked as required.
        [Required(ErrorMessage = "Start Date is required.")] // Ensures that the start date is provided and gives an error message if missing
        public DateTime StartDate { get; set; }

        // The EndDate property stores the end date of the work period for the claim.
        // It is marked as required.
        [Required(ErrorMessage = "End Date is required.")] // Ensures that the end date is provided and gives an error message if missing
        public DateTime EndDate { get; set; }
    }
}

