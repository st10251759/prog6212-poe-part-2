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

namespace ST10251759_PROG6212_POE.Models // Defines the namespace where the DashboardViewModel class resides
{
    // The DashboardViewModel class is used to represent the data that will be displayed on the dashboard
    // This class contains properties to hold the summary information related to claims and payments
    public class DashboardViewModel
    {
        // The TotalClaims property represents the total number of claims in the system.
        // This is typically used to show the overall number of claims submitted, regardless of their status.
        public int TotalClaims { get; set; }

        // The PendingClaims property holds the number of claims that are currently pending approval or processing.
        // This value is important to track the claims that are still under review or need further action.
        public int PendingClaims { get; set; }

        // The ApprovedClaims property represents the number of claims that have been approved.
        // This value shows how many claims have been processed successfully and approved for payment or other actions.
        public int ApprovedClaims { get; set; }

        // The TotalPayments property holds the total amount of money allocated for all claims.
        // This is a summary of all payments associated with claims that have been processed, regardless of their payment status.
        public decimal TotalPayments { get; set; }

        // The PendingPayments property holds the total amount of money that has been allocated to claims but is still pending payment.
        // This helps track the claims that have been approved but have not yet been paid.
        public decimal PendingPayments { get; set; }

        // The CompletedPayments property holds the total amount of money that has been successfully paid out for claims.
        // This represents the total value of claims that have been processed and fully paid.
        public decimal CompletedPayments { get; set; }
    }
}
