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

using System.ComponentModel.DataAnnotations; // Importing data annotation attributes for model validation
using System.ComponentModel.DataAnnotations.Schema; // Importing attributes for defining database schema details
using System.Reflection.Metadata; // Importing metadata features, although this is not used in the current code

namespace ST10251759_PROG6212_POE.Models // Defining a namespace for the project models
{
    // Defining a Claim class to represent a claim in the application
    public class Claim
    {
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Hours Worked is required.")]
        [Range(1, 150, ErrorMessage = "Hours Worked must be between 1 and 150.")]
        public decimal HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(200, 1000, ErrorMessage = "Hourly Rate must be between 200 and 1000.")]
        public decimal HourlyRate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")]
        public string Notes { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; }

        public string Status { get; set; } = "Pending";

        public bool IsApprovedByCoordinator { get; set; } = false;
        public bool IsApprovedByManager { get; set; } = false;

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }
        // New field to track payment status
        [Required]
        public string PaymentStatus { get; set; } = "Pending"; // Values could be "Pending", "Paid", "Processing"
    }

}
