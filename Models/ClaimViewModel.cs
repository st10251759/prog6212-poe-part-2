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
        [Required(ErrorMessage = "Hours Worked is required.")]
        [Range(1, 150, ErrorMessage = "Hours Worked must be between 1 and 150.")]
        public decimal HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(200, 1000, ErrorMessage = "Hourly Rate must be between 200 and 1000.")]
        public decimal HourlyRate { get; set; }

        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")]
        public string Notes { get; set; }

        [Display(Name = "Supporting Documents")]
        public List<IFormFile> SupportingDocuments { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }
    }
}
