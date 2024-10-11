using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Add this line
using ST10251759_PROG6212_POE.Data;
using ST10251759_PROG6212_POE.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ST10251759_PROG6212_POE.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : Controller
    {
        private readonly Prog6212DbContext _context;
        private readonly UserManager<IdentityUser> _userManager; 

        public LecturerController(Prog6212DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
        {
            // Get current logged-in user's ID
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            // Fetch claims for the logged-in lecturer
            var claimsQuery = _context.Claims
                .Include(c => c.Documents) // This requires the EF Core namespace
                .Where(c => c.ApplicationUserId == userId);

            //Apply date filtering if dates are provided
            if (startDate.HasValue && endDate.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.DateSubmitted >= startDate.Value && c.DateSubmitted <= endDate.Value);
            }

            var claims = await claimsQuery.ToListAsync();

            return View(claims);
        }
    }
}
