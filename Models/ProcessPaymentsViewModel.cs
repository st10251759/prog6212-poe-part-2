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

using System.Collections.Generic; // Importing the namespace for handling generic collections, such as List

namespace ST10251759_PROG6212_POE.Models // Defining the namespace for model classes specific to the project
{
    // Defining a view model class named ProcessPaymentsViewModel
    // This view model is designed to facilitate payment processing by organizing data related to claims and payment details
    // It serves as a data transfer object between the controller and view for managing payment processing
    public class ProcessPaymentsViewModel
    {
        // Property to store a list of Claim objects
        // This list holds the individual claims that are currently under consideration for payment
        // Each item in the list represents a single claim with its relevant details, such as hours worked and payment amount
        public List<Claim> Claims { get; set; }

        // Property to store the total number of claims
        // This integer value represents the count of all claims available for processing
        // It provides a summary measure, useful for displaying the total number of claims in the payment processing view
        public int TotalClaims { get; set; }

        // Property to store the total amount to be paid out for all claims
        // This decimal value represents the sum of amounts across all claims that are due for payment
        // It helps in understanding the overall financial obligation for the claims in consideration
        public decimal TotalAmountToPay { get; set; }

        // Property to store the count of claims with pending payments
        // This integer value indicates the number of claims that have yet to be processed for payment
        // It is useful for displaying a summary of outstanding payments in the view, helping to track pending financial commitments
        public int PendingPayments { get; set; }
    }
}
