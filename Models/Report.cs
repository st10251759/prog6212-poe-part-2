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

using System.ComponentModel.DataAnnotations; // Importing the namespace for data annotations, which provide validation and formatting for model properties

namespace ST10251759_PROG6212_POE.Models // Defining the namespace for model classes specific to the project
{
    // Defining a class named Report
    // The Report class represents a report record in the application, capturing details like name, type, date range, and file path
    public class Report
    {
        // Property to hold the unique identifier for each report
        // ReportId is an integer serving as the primary key for the Report class
        public int ReportId { get; set; }

        // Property to store the name of the report
        // [Required] ensures that ReportName must have a value; it cannot be null or empty
        // [StringLength(100)] enforces a maximum character limit of 100 for the ReportName to prevent overly long names
        [Required(ErrorMessage = "Report Name is required.")]
        [StringLength(100, ErrorMessage = "Report Name cannot exceed 100 characters.")]
        public string ReportName { get; set; }

        // Property to store the type or category of the report
        // [Required] enforces that ReportType must be provided, enhancing data completeness
        // [StringLength(50)] limits the length to 50 characters, ensuring the type is concise
        [Required(ErrorMessage = "Report Type is required.")]
        [StringLength(50, ErrorMessage = "Report Type cannot exceed 50 characters.")]
        public string ReportType { get; set; }

        // Property to specify the start date for the report
        // [Required] ensures a start date is specified, which is essential for date range validation and report accuracy
        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        // Property to specify the end date for the report
        // [Required] enforces that an end date must be provided, forming a valid date range
        // [DateGreaterThan("StartDate")] custom attribute checks that EndDate is after StartDate to ensure logical date order
        [Required(ErrorMessage = "End Date is required.")]
        [DateGreaterThan("StartDate", ErrorMessage = "End Date must be after Start Date.")]
        public DateTime EndDate { get; set; }

        // Property to store the file path of the report document
        // [Required] ensures FilePath is always provided, guaranteeing access to the report document
        // [StringLength(200)] restricts the file path length to a maximum of 200 characters for manageable path length
        // [RegularExpression] validates that the FilePath ends with a specific file extension (.pdf, .docx, or .xlsx), ensuring file type consistency
        [Required(ErrorMessage = "File Path is required.")]
        [StringLength(200, ErrorMessage = "File Path cannot exceed 200 characters.")]
        [RegularExpression(@"^.*\.(pdf|docx|xlsx)$", ErrorMessage = "File Path must be a .pdf, .docx, or .xlsx file.")]
        public string FilePath { get; set; }
    }
}
