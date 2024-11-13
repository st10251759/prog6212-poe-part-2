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

 LINQ Resource
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/Employee_Management_LINQ_FileHandling_G1
 Date Accessed: 11 October 2024

 ==============================Code Attribution==================================

 */

//Import List
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
{//namespace begin
    //Using Micrisoft Idenitity with Roles - This line means that only users with the "Lecturer" role can access the actions in this controller.
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : Controller
    {//Lecturer Controller begin

        //Private Field Declaration
        private readonly Prog6212DbContext _context; //This field holds an instance of Prog6212DbContext, which is used to interact with the database
        private readonly UserManager<IdentityUser> _userManager; //This field holds an instance of UserManager<IdentityUser>, which is part of ASP.NET Identity and is used for managing user information, including retrieving user details.


        //Constructor Method - initializes the _context and _userManager fields with instances provided via dependency injection. This allows the controller to use the database context and user management functionalities throughout its methods.
        public LecturerController(Prog6212DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //This action method retrieves claims related to the currently logged-in lecturer and can optionally filter them by submission date.
        public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
        {
            // Get current logged-in user's ID
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            // Fetch claims for the logged-in lecturer
            var claimsQuery = _context.Claims
                .Include(c => c.Documents) // Include documents assosciated witht the claim
                .Where(c => c.ApplicationUserId == userId); //where the user logged in

            //Apply date filtering if dates are provided - This block checks if both startDate and endDate parameters are provided (i.e., not null).
            if (startDate.HasValue && endDate.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.DateSubmitted >= startDate.Value && c.DateSubmitted <= endDate.Value);
            }

            // this line executes the query asynchronously and retrieves the results as a list of claims.
            var claims = await claimsQuery.ToListAsync();

            //eturns the claims list to the view, which will display the claims data to the user.
            return View(claims);
        }
    }//Lecturer Controller end
}//namespace end
