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

        // This is an HTTP POST method to create a new claim
        [HttpPost]
        // The [ValidateAntiForgeryToken] attribute ensures that requests are secure by verifying the token in the form to protect against CSRF (Cross-Site Request Forgery) attacks.
        [ValidateAntiForgeryToken]
        // The 'Create' method accepts a ClaimViewModel object 'model' containing data from the form submitted by the user
        public async Task<IActionResult> Create(ClaimViewModel model)
        {
            // First, check if the model is valid, meaning the data submitted meets all required validation rules
            if (!ModelState.IsValid)
            {
                // If not valid, return the view with the model to show validation errors to the user
                return View(model);
            }

            // Validate the date range: the claim's start and end date must be within the same month and no more than 31 days apart
            if ((model.EndDate - model.StartDate).Days > 31 || model.StartDate.Month != model.EndDate.Month)
            {
                // Add a model error with a specific message if the date range is invalid
                ModelState.AddModelError("", "The date range must be within one month and cannot exceed 31 days.");
                // Return the view with the validation error message
                return View(model);
            }

            // Validate that the claim is for the current or previous month
            var currentDate = DateTime.Now;
            // Define valid months: current month and the previous month
            var validMonths = new[] { currentDate.Month, currentDate.AddMonths(-1).Month };
            // Check if the claim's start and end dates are within the valid months
            if (!validMonths.Contains(model.StartDate.Month) || !validMonths.Contains(model.EndDate.Month))
            {
                // If not, add a model error message
                ModelState.AddModelError("", "Claims can only be submitted for the current or previous month.");
                return View(model);
            }

            // Retrieve the logged-in user from the UserManager
            var user = await _userManager.GetUserAsync(User);

            // Check if the user has already submitted a claim for the same month and year that is still pending or approved
            bool existingClaim = _context.Claims.Any(c =>
                c.ApplicationUserId == user.Id && // Claims associated with the logged-in user
                c.StartDate.Month == model.StartDate.Month && // Claims within the same month
                c.StartDate.Year == model.StartDate.Year && // Claims within the same year
                (c.Status == "Pending" || c.Status == "Approved by Manager" || c.Status == "Approved by Coordinator") // Claims that are not rejected
            );

            // If an existing claim is found, add an error and return the view
            if (existingClaim)
            {
                ModelState.AddModelError("", "You have already submitted a claim for this month, and it is either pending or approved.");
                ViewData["ClaimExists"] = existingClaim; // Set a flag to indicate the existence of a claim
                return View(model);
            }

            // Check if the supporting documents are attached to the claim
            if (model.SupportingDocuments == null || model.SupportingDocuments.Count == 0)
            {
                // If no documents are attached, add a validation error
                ModelState.AddModelError("", "At least one supporting document must be attached.");
                return View(model);
            }

            // Loop through each file in the supporting documents to validate them
            foreach (var file in model.SupportingDocuments)
            {
                // If the document is invalid (not PDF or exceeds size limit), add a validation error
                if (!IsValidDocument(file))
                {
                    ModelState.AddModelError("", "Only PDF files under 15 MB are allowed.");
                    return View(model);
                }
            }

            // Create a new claim object with the data from the model
            var claim = new Claim
            {
                HoursWorked = model.HoursWorked, // Set the hours worked from the model
                HourlyRate = model.HourlyRate, // Set the hourly rate from the model
                Notes = model.Notes, // Set any additional notes from the model
                DateSubmitted = DateTime.Now, // Set the current date as the submission date
                ApplicationUserId = user.Id, // Link the claim to the logged-in user
                TotalAmount = model.HourlyRate * model.HoursWorked, // Calculate the total amount from hours worked and hourly rate
                StartDate = model.StartDate, // Set the claim's start date
                EndDate = model.EndDate // Set the claim's end date
            };

            // Add the newly created claim to the context and save changes asynchronously
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Define the path to save the supporting documents
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in model.SupportingDocuments)
            {
                // Generate a unique file name using a GUID and the original file name
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName); // Define the full file path

                // Create the directory for uploads if it doesn't already exist
                Directory.CreateDirectory(uploadsFolder);

                // Save the file asynchronously to the specified file path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Create a new document record linked to the claim and save the file path and document name
                var document = new Document
                {
                    ClaimId = claim.ClaimId, // Link the document to the created claim
                    DocumentName = uniqueFileName, // Store the unique file name
                    FilePath = filePath // Store the file path
                };

                // Add the document to the context
                _context.Documents.Add(document);
            }

            // Save the document records to the database
            await _context.SaveChangesAsync();

            // Set a success message to be displayed after the claim is successfully submitted
            TempData["SuccessMessage"] = "Claim submitted successfully!";
            // Redirect to the "Dashboard" action in the "Lecturer" controller
            return RedirectToAction("Dashboard", "Lecturer");
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

