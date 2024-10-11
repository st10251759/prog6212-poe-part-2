using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST10251759_PROG6212_POE.Data;
using ST10251759_PROG6212_POE.Models;

namespace ST10251759_PROG6212_POE.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class ClaimController : Controller
    {
        private readonly Prog6212DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ClaimController(Prog6212DbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Claim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Create new claim
                var claim = new Claim
                {
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    Notes = model.Notes,
                    DateSubmitted = DateTime.Now,
                    ApplicationUserId = user.Id
                };

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                // Handle file upload if provided
                if (model.SupportingDocuments != null && model.SupportingDocuments.Count > 0)
                {
                    foreach (var file in model.SupportingDocuments)
                    {
                        // Validate file type and size
                        if (file.ContentType != "application/pdf" || file.Length > 15 * 1024 * 1024)
                        {
                            ModelState.AddModelError("", "Only PDF files under 15 MB are allowed.");
                            return View(model);
                        }

                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Ensure directory exists
                        Directory.CreateDirectory(uploadsFolder);

                        // Save file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Create document entry and link it to the claim
                        var document = new Document
                        {
                            ClaimId = claim.ClaimId,
                            DocumentName = uniqueFileName,
                            FilePath = filePath
                        };

                        _context.Documents.Add(document);
                    }

                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction("Index", "Dashboard");
            }

            // If we got this far, something failed; redisplay form with validation errors
            return View(model);
        }
    }
}
