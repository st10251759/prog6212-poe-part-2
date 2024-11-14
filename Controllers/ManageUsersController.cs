using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using ST10251759_PROG6212_POE.Models;

namespace ST10251759_PROG6212_POE.Controllers
{
    [Authorize(Roles = "HR Manager")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch users from the database first
            var users = await _userManager.Users
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email
                })
                .ToListAsync();

            // Then, for each user, get the roles in a separate loop
            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(appUser);
                user.Role = roles.FirstOrDefault();
            }

            return View(users);
        }


        // GET: Edit action to edit a user’s role
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRole = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Role = userRole.FirstOrDefault(),
                Roles = roles
            };

            return View(model);
        }

        // POST: Edit action to update the user’s role
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
                await _userManager.RemoveFromRoleAsync(user, userRoles.First());

            await _userManager.AddToRoleAsync(user, model.Role);
            return RedirectToAction(nameof(Index));
        }

        // GET: Confirm delete view
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var model = new DeleteUserViewModel
            {
                Id = user.Id,
                Email = user.Email
            };

            return View(model);
        }

        // POST: Delete action to delete a user
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index), "ManageUsers");

        }
    }
}
