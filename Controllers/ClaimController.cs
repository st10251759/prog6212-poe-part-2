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
            // Validate model state - checks if the incoming data (from ClaimViewModel) is valid. If not, it returns the same view with the current model to show any validation errors.
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate supporting documents - This block checks if the user has uploaded any supporting documents. If none are found, it adds an error message
            if (model.SupportingDocuments == null || model.SupportingDocuments.Count == 0)
            {
                ModelState.AddModelError("", "At least one supporting document must be attached.");
                return View(model);
            }

            // Validate file types and sizes - checks if the document uploaded is a valid document - of the type PDF and the size of the document is no greater than 15 MB - if this is not true returns current model with errors
            foreach (var file in model.SupportingDocuments)
            {
                if (file.ContentType != "application/pdf" || file.Length > 15 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Only PDF files under 15 MB are allowed.");
                    return View(model);
                }
            }

            // If the model is valid and documents are valid, proceed to create the claim
            var user = await _userManager.GetUserAsync(User);

            //Creates a new claim object - retrives the HoursWorked, HourlyRate and Notes from the view the user interacts with, and also stores the user id of the user currently logged in
            var claim = new Claim
            {
                HoursWorked = model.HoursWorked,
                HourlyRate = model.HourlyRate,
                Notes = model.Notes,
                DateSubmitted = DateTime.Now,
                ApplicationUserId = user.Id,
                TotalAmount = model.HourlyRate * model.HoursWorked

            };

            //adds the claim to the database table and saves changes
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Handle file upload
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            // for each loop that goes through each file in the SupportingDocuments list of the model. Each file represents a document that the user uploaded to support their claim.
            foreach (var file in model.SupportingDocuments)
            {
                //generates a new unique identifier (GUID). This ensures that every file has a unique name, even if multiple files with the same original name are uploaded.
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Ensure directory exists
                Directory.CreateDirectory(uploadsFolder);

                // Save file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Create document entry and link it to the claim - A new Document object is created to represent the uploaded file in the database.
                var document = new Document
                {
                    ClaimId = claim.ClaimId,
                    DocumentName = uniqueFileName,
                    FilePath = filePath
                };

                //This line adds the newly created document object to the _context.Documents collection. This prepares the document to be saved in the database when changes are committed.
                _context.Documents.Add(document);
            }

            await _context.SaveChangesAsync();

            //This line stores a success message in TempData. This is a temporary data storage mechanism that allows the message to be displayed to the user on the next page they visit
            TempData["SuccessMessage"] = "Claim submitted successfully!";
            //this line redirects the user to the "Dashboard" action of the "Lecturer" controller.
            return RedirectToAction("Dashboard", "Lecturer");
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
    }//ClaimController class end

}//namespace end
