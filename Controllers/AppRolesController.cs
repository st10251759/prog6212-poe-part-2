/*
 --------------------------------Student Information----------------------------------
 STUDENT NO.: ST10251759
 Name: Cameron Chetty
 Course: BCAD Year 2
 Module: Programming 2B
 Module Code: CLDV6212
 Assessment: Portfolio of Evidence (POE) Part 2
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ST10251759_PROG6212_POE.Controllers
{
    //Using Micrisoft Idenitity with Roles - This line means that only users with the "Programme Coordinator" role can access the actions in this controller.
    [Authorize(Roles = "HR Manager")]
    public class AppRolesController : Controller
        {
        // The Index action retrieves and displays all roles, while the Create actions (both GET and POST) allow users to create new roles, ensuring that duplicate role names are not allowed. - Thiswill be better handled in Part 3 with the Addition of the HR Manager User
        //declare variable for role manager
        private readonly RoleManager<IdentityRole> _roleManager;
            //constructort
            public AppRolesController(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }
            //List all the Roles created by Users
            public IActionResult Index()
            {
                var roles = _roleManager.Roles;
                return View(roles);
            }
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Create(IdentityRole model)
            {
                //prohibt duplicate role
                if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
                }
                return RedirectToAction("Index");
            }
        }
}
