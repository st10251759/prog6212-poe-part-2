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
 ==============================Code Attribution==================================

 */

// Importing the necessary namespaces
using Microsoft.AspNetCore.Authorization; // Provides attributes and classes for role-based and policy-based authorization.
using Microsoft.AspNetCore.Identity; // Provides Identity classes, such as RoleManager and IdentityRole, which help manage roles and user identities.
using Microsoft.AspNetCore.Mvc; // Provides classes and attributes for creating MVC controllers, views, and handling HTTP requests.

namespace ST10251759_PROG6212_POE.Controllers // Defining the namespace for the controller, organizing it under the project name.
{
    // This attribute ensures that only users with the "HR Manager" role can access any action in this controller.
    // By using the [Authorize(Roles = "HR Manager")] attribute, all actions in this controller require the user to have the "HR Manager" role.
    [Authorize(Roles = "HR Manager")]
    public class AppRolesController : Controller // Defining the AppRolesController class, which inherits from the base Controller class in ASP.NET Core MVC.
    {
        // Declaring a private readonly field for RoleManager, which manages roles in ASP.NET Core Identity.
        // RoleManager allows us to create, delete, and manage roles.
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor to initialize the RoleManager field when an instance of AppRolesController is created.
        // The RoleManager is injected through Dependency Injection to access role management functionalities.
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager; // Assigning the injected RoleManager instance to the private field _roleManager.
        }

        // The Index action method, which retrieves a list of all roles and displays them in a view.
        public IActionResult Index()
        {
            // Retrieving all roles from the RoleManager. The Roles property returns an IQueryable collection of all roles.
            var roles = _roleManager.Roles;

            // Returning the View with the roles collection passed as a model to display in the view.
            return View(roles);
        }

        // GET: Create action to show a form for creating a new role. This action responds to GET requests.
        [HttpGet]
        public IActionResult Create()
        {
            // Simply returning the Create view where the user can input a new role name.
            return View();
        }

        // POST: Create action to handle the form submission when a new role is created.
        // This method asynchronously creates the role if it doesn't already exist.
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            // Checking if the role with the specified name already exists to avoid duplicates.
            // _roleManager.RoleExistsAsync(model.Name) is called to determine if a role with the given name exists.
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                // If the role does not exist, a new role is created using _roleManager.CreateAsync.
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }

            // Redirecting to the Index action to display the list of roles after successfully creating a new role.
            return RedirectToAction("Index");
        }
    }
}
