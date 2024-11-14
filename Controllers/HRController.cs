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

 Author: w3schools
 Link: https://www.w3schools.com/html/
 Date Accessed: 12 October 2024

 Author: w3schools
 Link: https://www.w3schools.com/css/
 Date Accessed: 12 October 2024

 Microsfot Identity
 Author: Andy Malone MVP
 Link: https://www.youtube.com/watch?v=zS79FDhAhBs
 Date Accessed: 11 October 2024

 Database Work
 Author: Microsoft
 Link: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-8.0&tabs=visual-studio
 Date Accessed: 11 October 2024

 LINQ Resource
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/Employee_Management_LINQ_FileHandling_G1
 Date Accessed: 11 October 2024

 Pdf Creating for Reports Resource
 Author: C# Corner
 Link: https://www.c-sharpcorner.com/UploadFile/f2e803/basic-pdf-creation-using-itextsharp-part-i/
 Date Accessed: 14 Novemeber 2024

 ==============================Code Attribution==================================

 */

// Importing necessary namespaces for working with iTextSharp (for PDF generation), MVC controllers, Entity Framework, and authorization.
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Data;
using ST10251759_PROG6212_POE.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace ST10251759_PROG6212_POE.Controllers
{
    // The [Authorize] attribute restricts access to this controller to users with the "HR Manager" role only.
    [Authorize(Roles = "HR Manager")]
    public class HRController : Controller
    {
        // Declaring a private read-only field to access the database context (Prog6212DbContext).
        private readonly Prog6212DbContext _context;

        // Constructor to initialize the HRController with the injected Prog6212DbContext.
        // The context is used to interact with the database.
        public HRController(Prog6212DbContext context)
        {
            _context = context;
        }

        // Action method for displaying the HR Dashboard.
        public IActionResult Dashboard()
        {
            // Fetching data for the dashboard, including counts of claims and payments.
            var totalClaims = _context.Claims.Count(); // Total number of claims in the database.
            var pendingClaims = _context.Claims.Count(c => c.Status == "Pending"); // Claims with "Pending" status.
            var approvedClaims = _context.Claims.Count(c => c.Status == "Approved by Manager"); // Claims approved by the manager.

            // Summing up payment amounts for claims based on their payment status.
            var totalPayments = _context.Claims.Where(c => c.PaymentStatus == "Paid" || c.PaymentStatus == "Processing")
                                               .Sum(c => c.TotalAmount); // Total payments for "Paid" and "Processing" claims.
            var pendingPayments = _context.Claims.Where(c => c.PaymentStatus == "Processing")
                                                 .Sum(c => c.TotalAmount); // Payments for claims still processing.
            var completedPayments = _context.Claims.Where(c => c.PaymentStatus == "Paid")
                                                   .Sum(c => c.TotalAmount); // Payments that have been completed.

            // Creating a DashboardViewModel object to pass the gathered data to the view.
            var dashboardData = new DashboardViewModel
            {
                TotalClaims = totalClaims,
                PendingClaims = pendingClaims,
                ApprovedClaims = approvedClaims,
                TotalPayments = totalPayments,
                PendingPayments = pendingPayments,
                CompletedPayments = completedPayments
            };

            // Returning the view with the dashboard data.
            return View(dashboardData);
        }

        // Action method for displaying the "Process Payments" page.
        public async Task<IActionResult> ProcessPayments()
        {
            // Fetching claims that are "Approved by Manager" and have a "Processing" payment status.
            var claims = await _context.Claims
                .Include(c => c.ApplicationUser) // Including the related ApplicationUser for each claim.
                .Where(c => c.Status == "Approved by Manager" && c.PaymentStatus == "Processing")
                .ToListAsync();

            // Fetching claims that are "Approved by Manager" and have been "Paid."
            var claimsprocesssed = await _context.Claims
                .Include(c => c.ApplicationUser)
                .Where(c => c.Status == "Approved by Manager" && c.PaymentStatus == "Paid")
                .ToListAsync();

            // Calculating the total number of claims that have been paid, total amount to pay, and pending payments.
            var totalClaims = claimsprocesssed.Count(c => c.PaymentStatus == "Paid");
            var totalAmountToPay = claims.Sum(c => c.TotalAmount);
            var pendingPayments = claims.Count(c => c.PaymentStatus == "Processing");

            // Creating a ProcessPaymentsViewModel to hold the claims and payment data.
            var model = new ProcessPaymentsViewModel
            {
                Claims = claims,
                TotalClaims = totalClaims,
                TotalAmountToPay = totalAmountToPay,
                PendingPayments = pendingPayments
            };

            // Returning the view with the payment processing data.
            return View(model);
        }

        // Action method for processing a specific payment, triggered by a POST request.
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int claimId)
        {
            // Finding the claim with the specified claimId.
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null)
            {
                // Returning a 404 error if the claim is not found.
                return NotFound();
            }

            // Updating the payment status of the claim to "Paid."
            claim.PaymentStatus = "Paid";
            await _context.SaveChangesAsync(); // Saving the changes to the database.

            // Redirecting to the "ProcessPayments" action after processing the payment.
            return RedirectToAction(nameof(ProcessPayments));
        }

        // Action method for displaying the "Generate Report" page with existing reports.
        public async Task<IActionResult> GenerateReport()
        {
            // Fetching all existing reports from the database.
            var existingReports = await _context.Reports.ToListAsync();

            // Creating a GenerateReportViewModel to pass existing reports to the view.
            var viewModel = new GenerateReportViewModel
            {
                ExistingReports = existingReports
            };

            // Returning the view with the existing reports.
            return View(viewModel);
        }
        [HttpPost]  // Marks the method as an HTTP POST endpoint for handling form submissions or actions.
        public async Task<IActionResult> GenerateReport(string reportType, DateTime startDate, DateTime endDate, string reportName)
        {
            // Validating that the start date is not after the end date.
            if (startDate > endDate)
            {
                // Adding an error message to the ModelState if the dates are invalid.
                ModelState.AddModelError(string.Empty, "Start date cannot be after the end date.");
                return View();  // Returning the view to the user with the error message displayed.
            }

            // Querying the Claims table in the database to get all claims submitted within the specified date range.
            IQueryable<ST10251759_PROG6212_POE.Models.Claim> claimsQuery = _context.Claims.Where(c => c.DateSubmitted >= startDate && c.DateSubmitted <= endDate);

            // If the report type is "payments", filter claims based on their payment status (only include "Processing" or "Paid" claims).
            if (reportType == "payments")
            {
                claimsQuery = claimsQuery.Where(c => c.PaymentStatus == "Processing" || c.PaymentStatus == "Paid");
            }

            // Execute the query asynchronously to fetch the claims from the database.
            var claims = await claimsQuery.ToListAsync();

            // Check if no claims were found in the specified date range.
            if (!claims.Any())
            {
                // If no claims are found, add an error message and set a flag to indicate that no claims exist for the date range.
                ModelState.AddModelError(string.Empty, "No claims found for the given date range.");
                ViewBag.NoClaimsForDateRange = true;  // Set a flag for the view to display a popup indicating no claims were found.

                // Create a new ViewModel and pass the list of existing reports to the view for display.
                return View(new GenerateReportViewModel { ExistingReports = await _context.Reports.ToListAsync() });
            }

            // Perform calculations to gather report statistics based on the retrieved claims.
            var totalClaims = claims.Count();  // Total number of claims within the date range.
            var pendingClaims = claims.Count(c => c.Status == "Pending");  // Claims that are in "Pending" status.
            var approvedClaims = claims.Count(c => c.Status == "Approved by Manager");  // Claims that are approved.
            var rejectedClaims = claims.Count(c => c.Status == "Rejected by Coordinator" || c.Status == "Rejected by Manager");  // Claims that are rejected.
            var longestHoursWorked = claims.Max(c => c.HoursWorked);  // The maximum number of hours worked in the claims.
            var highestHourlyRate = claims.Max(c => c.HourlyRate);  // The highest hourly rate found in the claims.
            var totalPayments = claims.Sum(c => c.TotalAmount);  // Total sum of payments made in the claims.
            var processingPayments = claims.Count(c => c.PaymentStatus == "Processing");  // Number of claims with "Processing" payment status.
            var completedPayments = claims.Count(c => c.PaymentStatus == "Paid");  // Number of claims with "Paid" payment status.

            // Define the folder path where the report PDF will be stored.
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");

            // Ensure the directory for storing reports exists, and create it if it doesn't.
            Directory.CreateDirectory(folderPath);

            // Create a unique file name based on the report name and the current date/time.
            var fileName = $"{reportName}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            // Combine the folder path with the file name to get the full path for the report file.
            var filePath = Path.Combine(folderPath, fileName);

            // The relative file path to store in the database.
            var relativeFilePath = Path.Combine("reports", fileName);

            // Using a MemoryStream to generate the PDF document in memory before saving it to a file.
            using (var ms = new MemoryStream())
            {
                // Create a new iTextSharp document.
                var document = new iTextSharp.text.Document();

                // Initialize the PdfWriter to write the PDF to the MemoryStream.
                var writer = PdfWriter.GetInstance(document, ms);

                // Open the document to start adding content.
                document.Open();

                // Add a header for the report.
                document.Add(new Paragraph("========================================================"));
                document.Add(new Paragraph(" ClaimStream Lecturer Contract Monthly Claim System Report"));
                document.Add(new Paragraph("========================================================"));

                // Add a blank line and some general report information.
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("REPORT DETAILS"));
                document.Add(new Paragraph("--------------------------"));
                document.Add(new Paragraph($"Report: {reportName}"));
                document.Add(new Paragraph($"Report Type: {reportType}"));
                document.Add(new Paragraph($"Date Range: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}"));
                document.Add(new Paragraph($"Generated By: {User.Identity.Name}"));
                document.Add(new Paragraph("\n"));

                // Add a section for the statistics based on the report type.
                document.Add(new Paragraph("STATISTICS"));
                document.Add(new Paragraph("------------------"));

                // Depending on the report type, display different statistics.
                if (reportType == "payments")
                {
                    document.Add(new Paragraph($"Total Payments: {totalPayments:C}"));
                    document.Add(new Paragraph($"Processing Payments: {processingPayments}"));
                    document.Add(new Paragraph($"Completed Payments: {completedPayments}"));
                }
                else
                {
                    document.Add(new Paragraph($"Total Claims: {totalClaims}"));
                    document.Add(new Paragraph($"Pending Claims: {pendingClaims}"));
                    document.Add(new Paragraph($"Approved Claims: {approvedClaims}"));
                    document.Add(new Paragraph($"Rejected Claims: {rejectedClaims}"));
                    document.Add(new Paragraph($"Longest Hours Worked: {longestHoursWorked}"));
                    document.Add(new Paragraph($"Highest Hourly Rate: {highestHourlyRate:C}"));
                }
                document.Add(new Paragraph("\n"));

                // Add a section to list all claims in the report.
                document.Add(new Paragraph("CLAIM LIST"));
                document.Add(new Paragraph("------------------"));
                document.Add(new Paragraph("\n"));

                // Create a table to display claim details. The number of columns depends on the report type.
                var table = new PdfPTable(reportType == "payments" ? 5 : 4);

                // If the report type is "payments", define the table columns for payment-related information.
                if (reportType == "payments")
                {
                    table.AddCell("Claim ID");
                    table.AddCell("Total Amount");
                    table.AddCell("Start Date");
                    table.AddCell("End Date");
                    table.AddCell("Payment Status");

                    // Add each claim's details to the table.
                    foreach (var claim in claims)
                    {
                        table.AddCell(claim.ClaimId.ToString());
                        table.AddCell(claim.TotalAmount.ToString("C"));
                        table.AddCell(claim.StartDate.ToShortDateString());
                        table.AddCell(claim.EndDate.ToShortDateString());
                        table.AddCell(claim.PaymentStatus);
                    }
                }
                else
                {
                    // Otherwise, define the table columns for claim status and hours worked.
                    table.AddCell("Claim ID");
                    table.AddCell("Hours Worked");
                    table.AddCell("Hourly Rate");
                    table.AddCell("Status");

                    // Add each claim's details to the table.
                    foreach (var claim in claims)
                    {
                        table.AddCell(claim.ClaimId.ToString());
                        table.AddCell(claim.HoursWorked.ToString());
                        table.AddCell(claim.HourlyRate.ToString("C"));
                        table.AddCell(claim.Status);
                    }
                }

                // Add the table to the document.
                document.Add(table);

                // Close the document to complete the PDF creation.
                document.Close();

                // Save the generated PDF file to the specified file path.
                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
            }

            // Create a new Report object to save the generated report details to the database.
            var report = new Report
            {
                ReportName = reportName,
                ReportType = reportType,
                StartDate = startDate,
                EndDate = endDate,
                FilePath = relativeFilePath
            };

            // Add the new report to the database.
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // If the model state is not valid, retrieve existing reports and return the view with the model.
            if (!ModelState.IsValid)
            {
                var existingReports = await _context.Reports.ToListAsync();
                var model = new GenerateReportViewModel
                {
                    ExistingReports = existingReports
                };
                return View(model);
            }

            // Redirect to the GenerateReport action to show the updated page.
            return RedirectToAction(nameof(GenerateReport));
        }






    }//controller end

}//namespace end
