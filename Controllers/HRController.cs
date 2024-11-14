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
    [Authorize(Roles = "HR Manager")]
    public class HRController : Controller
    {
        private readonly Prog6212DbContext _context;

        public HRController(Prog6212DbContext context)
        {
            _context = context;
        }

        // View Dashboard
        public IActionResult Dashboard()
        {
            var totalClaims = _context.Claims.Count();
            var pendingClaims = _context.Claims.Count(c => c.Status == "Pending");
            var approvedClaims = _context.Claims.Count(c => c.Status == "Approved by Manager");

            var totalPayments = _context.Claims.Where(c => c.PaymentStatus == "Paid" || c.PaymentStatus == "Processing").Sum(c => c.TotalAmount);
            var pendingPayments = _context.Claims.Where(c => c.PaymentStatus == "Processing").Sum(c => c.TotalAmount);
            var completedPayments = _context.Claims.Where(c => c.PaymentStatus == "Paid").Sum(c => c.TotalAmount);

            var dashboardData = new DashboardViewModel
            {
                TotalClaims = totalClaims,
                PendingClaims = pendingClaims,
                ApprovedClaims = approvedClaims,
                TotalPayments = totalPayments,
                PendingPayments = pendingPayments,
                CompletedPayments = completedPayments
            };

            return View(dashboardData);
        }

        // Display Process Payments page
        public async Task<IActionResult> ProcessPayments()
        {
            var claims = await _context.Claims
                .Include(c => c.ApplicationUser)
                .Where(c => c.Status == "Approved by Manager" && c.PaymentStatus == "Processing")
                .ToListAsync();

            var claimsprocesssed = await _context.Claims
                .Include(c => c.ApplicationUser)
                .Where(c => c.Status == "Approved by Manager" && c.PaymentStatus == "Paid")
                .ToListAsync();

            var totalClaims = claimsprocesssed.Count(c => c.PaymentStatus == "Paid");
            var totalAmountToPay = claims.Sum(c => c.TotalAmount);
            var pendingPayments = claims.Count(c => c.PaymentStatus == "Processing");

            var model = new ProcessPaymentsViewModel
            {
                Claims = claims,
                TotalClaims = totalClaims,
                TotalAmountToPay = totalAmountToPay,
                PendingPayments = pendingPayments
            };

            return View(model);
        }

        // Process a specific payment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null)
            {
                return NotFound();
            }

            claim.PaymentStatus = "Paid";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProcessPayments));
        }


        // Display Report Generation page with existing reports
        public async Task<IActionResult> GenerateReport()
        {
            // Fetch all existing reports from the database
            var existingReports = await _context.Reports.ToListAsync();

            // Pass the existing reports to the view
            var viewModel = new GenerateReportViewModel
            {
                ExistingReports = existingReports
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(string reportType, DateTime startDate, DateTime endDate, string reportName)
        {
            if (startDate > endDate)
            {
                ModelState.AddModelError(string.Empty, "Start date cannot be after the end date.");
                return View();
            }

            // Query to fetch claims within the specified date range
            IQueryable<ST10251759_PROG6212_POE.Models.Claim> claimsQuery = _context.Claims.Where(c => c.DateSubmitted >= startDate && c.DateSubmitted <= endDate);

            if (reportType == "payments")
            {
                // Filter claims based on payment status for the payment report
                claimsQuery = claimsQuery.Where(c => c.PaymentStatus == "Processing" || c.PaymentStatus == "Paid");
            }

            var claims = await claimsQuery.ToListAsync();

            // Check if there are any claims to report
            if (!claims.Any())
            {
                ModelState.AddModelError(string.Empty, "No claims found for the given date range.");
                ViewBag.NoClaimsForDateRange = true; // Set a flag for the view to display the popup
                return View(new GenerateReportViewModel { ExistingReports = await _context.Reports.ToListAsync() });
            }

            // Calculations for report statistics
            var totalClaims = claims.Count();
            var pendingClaims = claims.Count(c => c.Status == "Pending");
            var approvedClaims = claims.Count(c => c.Status == "Approved by Manager");
            var rejectedClaims = claims.Count(c => c.Status == "Rejected by Coordinator" || c.Status == "Rejected by Manager");
            var longestHoursWorked = claims.Max(c => c.HoursWorked);
            var highestHourlyRate = claims.Max(c => c.HourlyRate);
            var totalPayments = claims.Sum(c => c.TotalAmount);
            var processingPayments = claims.Count(c => c.PaymentStatus == "Processing");
            var completedPayments = claims.Count(c => c.PaymentStatus == "Paid");

            // File path for PDF report storage
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
            Directory.CreateDirectory(folderPath);
            var fileName = $"{reportName}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(folderPath, fileName);
            var relativeFilePath = Path.Combine("reports", fileName);

            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Report Header and General Information
                document.Add(new Paragraph("========================================================"));
                document.Add(new Paragraph(" ClaimStream Lecturer Contract Monthly Claim System Report"));
                document.Add(new Paragraph("========================================================"));

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("REPORT DETAILS"));
                document.Add(new Paragraph("--------------------------"));
                document.Add(new Paragraph($"Report: {reportName}"));
                document.Add(new Paragraph($"Report Type: {reportType}"));
                document.Add(new Paragraph($"Date Range: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}"));
                document.Add(new Paragraph($"Generated By: {User.Identity.Name}"));
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph("STATISTICS"));
                document.Add(new Paragraph("------------------"));

                // Statistics based on report type
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

                document.Add(new Paragraph("CLAIM LIST"));
                document.Add(new Paragraph("------------------"));
                document.Add(new Paragraph("\n"));
                // Add Claim List in table format
                var table = new PdfPTable(reportType == "payments" ? 5 : 4);
                if (reportType == "payments")
                {
                    table.AddCell("Claim ID");
                    table.AddCell("Total Amount");
                    table.AddCell("Start Date");
                    table.AddCell("End Date");
                    table.AddCell("Payment Status");

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
                    table.AddCell("Claim ID");
                    table.AddCell("Hours Worked");
                    table.AddCell("Hourly Rate");
                    table.AddCell("Status");

                    foreach (var claim in claims)
                    {
                        table.AddCell(claim.ClaimId.ToString());
                        table.AddCell(claim.HoursWorked.ToString());
                        table.AddCell(claim.HourlyRate.ToString("C"));
                        table.AddCell(claim.Status);
                    }
                }

                document.Add(table);
                document.Close();

                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
            }

            // Save report details in the database
            var report = new Report
            {
                ReportName = reportName,
                ReportType = reportType,
                StartDate = startDate,
                EndDate = endDate,
                FilePath = relativeFilePath
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                var existingReports = await _context.Reports.ToListAsync();
                var model = new GenerateReportViewModel
                {
                    ExistingReports = existingReports
                };
                return View(model);
            }

            return RedirectToAction(nameof(GenerateReport));
        }





    }

}
