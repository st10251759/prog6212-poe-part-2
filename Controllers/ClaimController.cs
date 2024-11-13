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

 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/TaskManager_Attributes_demo
 Date Accessed: 11 October 2024

 Microsfot Identity
 Author: Andy Malone MVP
 Link: https://www.youtube.com/watch?v=zS79FDhAhBs
 Date Accessed: 11 October 2024

 Database Work
 Author: Microsoft
 Link: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-8.0&tabs=visual-studio
 Date Accessed: 11 October 2024

 PDF Doc - File Handling Resource
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/FileHandlingApp
 Date Accessed: 11 October 2024

 MVC APP
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/blob/main/EmployeeLeaveManagement_G1.zip
 Date Accessed: 11 October 2024


 ==============================Code Attribution==================================

 */

//Import List
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST10251759_PROG6212_POE.Data;
using ST10251759_PROG6212_POE.Models;
using System;

namespace ST10251759_PROG6212_POE.Controllers
{//namespace begin
    //Using Micrisoft Idenitity with Roles - This line means that only users with the "Lecturer" role can access the actions in this controller.
    [Authorize(Roles = "Lecturer")]
    public class ClaimController : Controller
    {//ClaimController class begin

        //private fields declaration - These lines declare three private fields that will be used throughout the controller.
        private readonly Prog6212DbContext _context; //interact with the database.
        private readonly UserManager<IdentityUser> _userManager; //helps manage user accounts, like retrieving the currently logged-in user.
        private readonly IWebHostEnvironment _environment; //provides information about the web hosting environment


        //Constructor method - initializes the private fields with the values passed in, allowing the controller to use them for database access, user management, and environment information.
        public ClaimController(Prog6212DbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {//Construct begin
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }//constructor end

        // GET: Claim/Create - This method responds to GET requests to the "Claim/Create" URL. It simply returns a view (web page) for creating a new claim.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claim/Create - This method handles POST requests to submit a new claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate date range (within the same month and 30 days max)
            if ((model.EndDate - model.StartDate).Days > 30 || model.StartDate.Month != model.EndDate.Month)
            {
                ModelState.AddModelError("", "The date range must be within one month and cannot exceed 30 days.");
                return View(model);
            }

            // Validate that the date is in the current or previous month
            var currentDate = DateTime.Now;
            var validMonths = new[] { currentDate.Month, currentDate.AddMonths(-1).Month };
            if (!validMonths.Contains(model.StartDate.Month) || !validMonths.Contains(model.EndDate.Month))
            {
                ModelState.AddModelError("", "Claims can only be submitted for the current or previous month.");
                return View(model);
            }

            // Check if a claim has already been submitted for the month
            var user = await _userManager.GetUserAsync(User);
            bool claimExists = _context.Claims.Any(c =>
                c.ApplicationUserId == user.Id &&
                c.StartDate.Month == model.StartDate.Month &&
                c.StartDate.Year == model.StartDate.Year);

            if (claimExists)
            {
                ModelState.AddModelError("", "You have already submitted a claim for this month.");
                return View(model);
            }

            if (model.SupportingDocuments == null || model.SupportingDocuments.Count == 0)
            {
                ModelState.AddModelError("", "At least one supporting document must be attached.");
                return View(model);
            }

            bool isInvalidFile = false;
            foreach (var file in model.SupportingDocuments)
            {
                if (!IsValidDocument(file))
                {
                    isInvalidFile = true;
                    ModelState.AddModelError("", "Only PDF files under 15 MB are allowed.");
                    return View(model);
                }
            }

            if (!isInvalidFile)
            {
                var claim = new Claim
                {
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    Notes = model.Notes,
                    DateSubmitted = DateTime.Now,
                    ApplicationUserId = user.Id,
                    TotalAmount = model.HourlyRate * model.HoursWorked,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                foreach (var file in model.SupportingDocuments)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var document = new Document
                    {
                        ClaimId = claim.ClaimId,
                        DocumentName = uniqueFileName,
                        FilePath = filePath
                    };

                    _context.Documents.Add(document);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction("Dashboard", "Lecturer");
            }

            return View(model);
        }


        // GET: Claims/Track
        public async Task<IActionResult> TrackClaims()
        {
            // Get the logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            // Fetch claims for the current user
            var claims = _context.Claims
                .Where(c => c.ApplicationUserId == currentUser.Id)
                .ToList();
            //pass the list of claims to the view
            return View(claims);
        }

        // Method to validate if the document is a PDF and does not exceed 15 MB - Used in Unit Testing - same functionality as if statement 
        public bool IsValidDocument(IFormFile file)
        {
            // Check if the file is not null
            if (file == null)
            {
                return false;
            }

            // Validate the file type and size
            return file.ContentType == "application/pdf" && file.Length <= 15 * 1024 * 1024; // 15 MB
        }



    }//ClaimController class end

}//namespace end
