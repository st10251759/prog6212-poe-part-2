using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ST10251759_PROG6212_POE.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly Prog6212DbContext _context;

        public ProgrammeCoordinatorController(Prog6212DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Only show claims that are pending and not yet approved by the coordinator
            // Only show claims that are pending and not yet approved by the coordinator
            var pendingClaims = _context.Claims
                .Include(c => c.ApplicationUser) // Include the ApplicationUser
                .Where(c => !c.IsApprovedByCoordinator && c.Status == "Pending")
                .ToList();

            return View(pendingClaims);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.IsApprovedByCoordinator = true;
                claim.Status = "Approved by Coordinator";
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
                claim.Status = "Rejected by Coordinator";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
