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

using System.ComponentModel.DataAnnotations; // Importing data annotation attributes for model validation
using System.ComponentModel.DataAnnotations.Schema; // Importing attributes for defining database schema details
using System.Reflection.Metadata; // Importing metadata features, although this is not used in the current code

namespace ST10251759_PROG6212_POE.Models // Defining a namespace for the project models
{
    // Defining a Claim class to represent a claim in the application
    public class Claim
    {
        // ClaimId represents the unique identifier for each claim
        public int ClaimId { get; set; }

        // HoursWorked property represents the number of hours worked for the claim.
        // It is required and must be between 1 and 150 hours.
        [Required(ErrorMessage = "Hours Worked is required.")]
        [Range(1, 150, ErrorMessage = "Hours Worked must be between 1 and 150.")]
        public decimal HoursWorked { get; set; }

        // HourlyRate property represents the hourly rate for the claim.
        // It is required and must be between 200 and 1000 units (currency or other metric).
        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(200, 1000, ErrorMessage = "Hourly Rate must be between 200 and 1000.")]
        public decimal HourlyRate { get; set; }

        // TotalAmount property represents the total amount to be claimed.
        // It is a required field.
        [Required]
        public decimal TotalAmount { get; set; }

        // Notes property allows the user to add any additional information related to the claim.
        // It has a max length constraint of 500 characters.
        [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")]
        public string Notes { get; set; }

        // DateSubmitted property stores the date when the claim was submitted.
        // It is a required field.
        [Required]
        public DateTime DateSubmitted { get; set; }

        // Status property stores the current status of the claim (e.g., "Pending").
        // It is initialized with the default value "Pending".
        public string Status { get; set; } = "Pending";

        // IsApprovedByCoordinator property is a flag that indicates whether the claim is approved by the coordinator.
        // It is initialized with the default value "false".
        public bool IsApprovedByCoordinator { get; set; } = false;

        // IsApprovedByManager property is a flag that indicates whether the claim is approved by the manager.
        // It is initialized with the default value "false".
        public bool IsApprovedByManager { get; set; } = false;

        // ApplicationUserId is a foreign key that links the claim to a specific application user.
        // The ApplicationUser class represents the user who submitted the claim.
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        // Navigation property to link the claim to an ApplicationUser.
        public virtual ApplicationUser ApplicationUser { get; set; }

        // A collection of Document objects related to the claim, representing any supporting documents attached to the claim.
        public virtual ICollection<Document> Documents { get; set; }

        // StartDate property represents the start date of the work period for the claim.
        // It is a required field.
        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        // EndDate property represents the end date of the work period for the claim.
        // It is a required field.
        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        // PaymentStatus is a new field added to track the payment status of the claim.
        // It is required and can have values such as "Pending", "Paid", or "Processing".
        [Required]
        public string PaymentStatus { get; set; } = "Pending"; // Default value is "Pending"
    }
}
