using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ST10251759_PROG6212_POE.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly Prog6212DbContext _context;

        public AcademicManagerController(Prog6212DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Only show claims that are approved by the coordinator but pending manager approval
            var pendingClaims = _context.Claims
                .Include(c => c.ApplicationUser) // Include the ApplicationUser
                .Include(c => c.Documents) // Include the Documents
                .Where(c => c.IsApprovedByCoordinator && !c.IsApprovedByManager && c.Status == "Approved by Coordinator")
                .ToList();

            return View(pendingClaims);
        }


        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.IsApprovedByManager = true;
                claim.Status = "Approved by Manager";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.Status = "Rejected by Manager";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
