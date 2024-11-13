using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Data;
using ST10251759_PROG6212_POE.Models;

namespace ST10251759_PROG6212_POE.Controllers
{
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



    }

}
