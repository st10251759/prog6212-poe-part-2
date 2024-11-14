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

// Importing required namespaces
using Microsoft.AspNetCore.Authorization; // Enables role-based and policy-based authorization for controller actions.
using Microsoft.AspNetCore.Identity; // Provides classes and services for managing user accounts, such as UserManager and IdentityUser.
using Microsoft.AspNetCore.Mvc; // Provides classes for building MVC controllers and handling HTTP requests and responses.
using ST10251759_PROG6212_POE.Data; // Imports the project's Data namespace, which likely includes database context and data models.
using ST10251759_PROG6212_POE.Models; // Imports the Models namespace, where custom models like ClaimViewModel and Claim may be defined.
using System; // Provides fundamental classes and base types, such as DateTime and Array.

namespace ST10251759_PROG6212_POE.Controllers // Defining the namespace for organizing code related to the project’s controllers.
{// Namespace start

    // Restricting access to users with the "Lecturer" role. Only users in this role can access this controller's actions.
    [Authorize(Roles = "Lecturer")]
    public class ClaimController : Controller // Defining the ClaimController class, which inherits from the base Controller class.
    {// ClaimController class start

        // Declaring private fields to store services for use throughout the controller.
        private readonly Prog6212DbContext _context; // Used to interact with the application's database.
        private readonly UserManager<IdentityUser> _userManager; // Used to manage user accounts, retrieve information about the current user, etc.
        private readonly IWebHostEnvironment _environment; // Provides information about the hosting environment, such as web root paths.

        // Constructor for ClaimController - initializes the private fields with instances of the services passed in.
        // These fields are injected through dependency injection.
        public ClaimController(Prog6212DbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {// Constructor start
            _context = context; // Assigning the injected database context to the _context field for database operations.
            _userManager = userManager; // Assigning the UserManager instance to _userManager to manage user-related actions.
            _environment = environment; // Assigning the environment instance to _environment to access hosting environment properties.
        }// Constructor end

        // GET: Claim/Create - Handles GET requests to display the form for creating a new claim.
        public IActionResult Create()
        {
            // Returning the Create view, which provides the form for the user to fill out and submit a new claim.
            return View();
        }

        // POST: Claim/Create - Handles form submissions to create a new claim.
        [HttpPost] // Specifies that this action only handles POST requests.
        [ValidateAntiForgeryToken] // Helps prevent Cross-Site Request Forgery (CSRF) attacks by validating the anti-forgery token.
        public async Task<IActionResult> Create(ClaimViewModel model)
        {
            // Checking if the model is valid (i.e., if all required fields are filled out and satisfy validation rules).
            if (!ModelState.IsValid)
            {
                // If the model is not valid, re-display the form with validation messages.
                return View(model);
            }

            // Validating that the date range is within a single month and does not exceed 31 days.
            if ((model.EndDate - model.StartDate).Days > 32 || model.StartDate.Month != model.EndDate.Month)
            {
                ModelState.AddModelError("", "The date range must be within one month and cannot exceed 31 days.");
                return View(model); // Returning the form view with an error message if validation fails.
            }

            // Checking if the claim is for the current or previous month only.
            var currentDate = DateTime.Now;
            var validMonths = new[] { currentDate.Month, currentDate.AddMonths(-1).Month }; // Array of valid months.
            if (!validMonths.Contains(model.StartDate.Month) || !validMonths.Contains(model.EndDate.Month))
            {
                ModelState.AddModelError("", "Claims can only be submitted for the current or previous month.");
                return View(model); // Returning the form with an error message if the claim date is outside the valid range.
            }

            // Checking if a claim has already been submitted for the specified month by the current user.
            var user = await _userManager.GetUserAsync(User); // Retrieving the logged-in user.
            bool claimExists = _context.Claims.Any(c =>
                c.ApplicationUserId == user.Id &&
                c.StartDate.Month == model.StartDate.Month &&
                c.StartDate.Year == model.StartDate.Year);

            if (claimExists)
            {
                ModelState.AddModelError("", "You have already submitted a claim for this month."); // Error if duplicate.
                ViewData["ClaimExists"] = true; // Setting a flag for frontend (e.g., jQuery) handling.
                return View(model); // Returning the form with an error message.
            }

            // Validating that at least one supporting document is attached.
            if (model.SupportingDocuments == null || model.SupportingDocuments.Count == 0)
            {
                ModelState.AddModelError("", "At least one supporting document must be attached.");
                return View(model); // Returning the form if no documents are attached.
            }

            // Checking if each document meets file criteria (PDF format and under 15 MB).
            bool isInvalidFile = false;
            foreach (var file in model.SupportingDocuments)
            {
                if (!IsValidDocument(file))
                {
                    isInvalidFile = true;
                    ModelState.AddModelError("", "Only PDF files under 15 MB are allowed.");
                    return View(model); // Returning form with error if any file is invalid.
                }
            }

            if (!isInvalidFile)
            {
                // Creating a new Claim object with the values provided in the form.
                var claim = new Claim
                {
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    Notes = model.Notes,
                    DateSubmitted = DateTime.Now, // Setting the current date as submission date.
                    ApplicationUserId = user.Id, // Linking claim to current user.
                    TotalAmount = model.HourlyRate * model.HoursWorked, // Calculating the total amount for the claim.
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };

                _context.Claims.Add(claim); // Adding the new claim to the database context.
                await _context.SaveChangesAsync(); // Saving the claim to the database asynchronously.

                // Setting up the file upload folder for saving supporting documents.
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                foreach (var file in model.SupportingDocuments)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName; // Generating a unique file name.
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName); // Full path for saving the file.
                    Directory.CreateDirectory(uploadsFolder); // Ensuring that the upload folder exists.

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream); // Copying the file to the server.
                    }

                    // Creating a new Document object to link the file to the claim.
                    var document = new Document
                    {
                        ClaimId = claim.ClaimId, // Linking document to the created claim.
                        DocumentName = uniqueFileName, // Storing the generated file name.
                        FilePath = filePath // Storing the file path.
                    };

                    _context.Documents.Add(document); // Adding the document record to the database.
                }

                await _context.SaveChangesAsync(); // Saving all changes (including document records) to the database.
                TempData["SuccessMessage"] = "Claim submitted successfully!"; // Storing a success message for display.
                return RedirectToAction("Dashboard", "Lecturer"); // Redirecting to the Lecturer's Dashboard.
            }

            return View(model); // If file validation fails, returning the form view.
        }

        // GET: Claims/Track - Displays the track claims page for the logged-in user.
        public async Task<IActionResult> TrackClaims()
        {
            // Retrieving the currently logged-in user.
            var currentUser = await _userManager.GetUserAsync(User);

            // Fetching all claims submitted by the current user.
            var claims = _context.Claims
                .Where(c => c.ApplicationUserId == currentUser.Id)
                .ToList();

            // Passing the list of claims to the view.
            return View(claims);
        }

        // Validates if a document is a PDF and is less than 15 MB in size.
        public bool IsValidDocument(IFormFile file)
        {
            // Checking if the file is not null.
            if (file == null)
            {
                return false; // Returning false if the file is null.
            }

            // Validating the file type (must be PDF) and size (must be 15 MB or less).
            return file.ContentType == "application/pdf" && file.Length <= 15 * 1024 * 1024; // 15 MB
        }

    }// ClaimController class end
}// Namespace end

