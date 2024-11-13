using Microsoft.AspNetCore.Mvc;
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

        // Update Payment Status
        public IActionResult UpdatePaymentStatus(int claimId, string status)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim == null)
            {
                return NotFound();
            }

            claim.PaymentStatus = status; // Update payment status
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }

}
