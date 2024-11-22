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

 Microsfot Identity
 Author: Andy Malone MVP
 Link: https://www.youtube.com/watch?v=zS79FDhAhBs
 Date Accessed: 11 October 2024

 Database Work
 Author: Microsoft
 Link: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-8.0&tabs=visual-studio
 Date Accessed: 11 October 2024

 Admin Panel to Manage Users Resource
 Author: Code A Future
 Link: https://www.youtube.com/watch?v=WIUI_dLZpgs
 Date Accessed: 13 Novemeber 2024

 ==============================Code Attribution==================================

 */

using Microsoft.AspNetCore.Authorization; // Required for adding authorization attributes to controllers or actions.
using Microsoft.AspNetCore.Identity; // Provides access to ASP.NET Core Identity services for managing users and roles.
using Microsoft.AspNetCore.Mvc; // For building the web API or web MVC controllers and actions.
using Microsoft.EntityFrameworkCore; // For Entity Framework Core to perform database operations asynchronously.
using System.Threading.Tasks; // For working with asynchronous methods and tasks.
using System.Linq; // For LINQ queries to operate on collections and queryable objects.
using ST10251759_PROG6212_POE.Models; // For using model classes that represent data structures and views.

namespace ST10251759_PROG6212_POE.Controllers
{
    // Ensures that only users with the 'HR Manager' role can access the actions in this controller
    [Authorize(Roles = "HR Manager")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager; // Manages users in the ASP.NET Core Identity system.
        private readonly RoleManager<IdentityRole> _roleManager; // Manages roles in the ASP.NET Core Identity system.

        // Constructor to inject dependencies for user and role management
        public ManageUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager; // Assigning the injected UserManager instance to the private field
            _roleManager = roleManager; // Assigning the injected RoleManager instance to the private field
        }

        // Action to display a list of users with their basic details
        public async Task<IActionResult> Index()
        {
            // Fetch all users and map to UserViewModel
            var users = await _userManager.Users
                .OfType<ApplicationUser>() // Cast to ApplicationUser
                .Select(user => new UserViewModel
                {
                    Id = user.Id, // Mapping user Id
                    Email = user.Email, // Mapping user Email
                    PhoneNumber = user.PhoneNumber, // Mapping user Phone Number
                    Faculty = user.Faculty, // Mapping Faculty from ApplicationUser
                    IDNumber = user.IDNumber,
                    HomeAddress = user.HomeAddress
                })
                .ToListAsync();

            // For each user, fetch their roles asynchronously and assign the first role (if exists) to the view model
            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id); // Find the user by Id
                var roles = await _userManager.GetRolesAsync(appUser); // Fetch roles of the user
                user.Role = roles.FirstOrDefault(); // Assign the first role (if any) to the view model
            }

            // Returns the users list to the view (Index view)
            return View(users);
        }

        // GET action to load the edit user page with the current user’s details and available roles
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Find the user by Id asynchronously
            if (user == null) // If no user is found, return a NotFound result
                return NotFound();

            // Fetch the user's roles and all available roles from RoleManager
            var userRole = await _userManager.GetRolesAsync(user); // Get the current roles assigned to the user
            var roles = _roleManager.Roles.Select(r => r.Name).ToList(); // Get all roles in the system

            // Prepare the model to pass to the Edit view, including user details and the list of roles
            var model = new EditUserViewModel
            {
                Id = user.Id, // User Id
                Email = user.Email, // User Email
                PhoneNumber = user.PhoneNumber, // User PhoneNumber
                Faculty = (user as ApplicationUser)?.Faculty, //Faculty user belongs to
                HomeAddress = (user as ApplicationUser)?.HomeAddress,
                IDNumber = (user as ApplicationUser)?.IDNumber,
                Role = userRole.FirstOrDefault(), // The user’s current role (if any)
                Roles = roles // List of all roles available in the system
            };

            return View(model); // Return the Edit view with the model data
        }

        // POST action to update the user’s role after editing
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id); // Find the user by Id
            if (user == null) // If no user is found, return a NotFound result
                return NotFound();

            // Fetch the current roles of the user
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any()) // If the user has any roles
                await _userManager.RemoveFromRoleAsync(user, userRoles.First()); // Remove the first role the user has

            // Add the new selected role from the model
            await _userManager.AddToRoleAsync(user, model.Role); // Assign the new role to the user

            user.PhoneNumber = model.PhoneNumber; // Update PhoneNumber
            if (user is ApplicationUser appUser)
            {
                appUser.Faculty = model.Faculty; // Update Faculty
                appUser.IDNumber = model.IDNumber; // Update IDNumber
                appUser.HomeAddress = model.HomeAddress; // Update HomeAddress
            }

            await _userManager.UpdateAsync(user); // Save changes

            return RedirectToAction(nameof(Index)); // After updating, redirect to the Index action to display the updated user list
        }

        // GET action to confirm deletion of a user
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Find the user by Id
            if (user == null) // If no user is found, return a NotFound result
                return NotFound();

            // Prepare the model to confirm deletion with the user's Email
            var model = new DeleteUserViewModel
            {
                Id = user.Id, // User Id
                Email = user.Email // User Email to confirm which user will be deleted
            };

            return View(model); // Return the Delete confirmation view with the model data
        }

        // POST action to actually delete the user from the system after confirmation
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Find the user by Id
            if (user == null) // If no user is found, return a NotFound result
                return NotFound();

            // Delete the user from the UserManager
            await _userManager.DeleteAsync(user); // Delete the user from the system

            // After successful deletion, redirect back to the Index action to display the updated user list
            return RedirectToAction(nameof(Index), "ManageUsers");
        }
    }
}
