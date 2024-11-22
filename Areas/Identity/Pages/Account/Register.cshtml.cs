// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
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

 Fluent Validation Logic
 Author: FluentValidation
 Link: https://docs.fluentvalidation.net/en/latest/index.html
 Date Accessed: 15 Novemeber 2024

 Regular Expression Language
 Author: Microsoft
 Link: https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
 Date Accessed: 15 Novemeber 2024

 ==============================Code Attribution==================================

 */



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ST10251759_PROG6212_POE.Models;

namespace ST10251759_PROG6212_POE.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Faculty { get; set; }
            public string IDNumber { get; set; }
            public string HomeAddress { get; set; } 
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }

            // Remove the Role field from the model
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                user.Firstname = Input.Firstname;
                user.Lastname = Input.Lastname;
                user.PhoneNumber = Input.PhoneNumber; // Set the PhoneNumber property
                user.Faculty = Input.Faculty;
                user.IDNumber = Input.IDNumber;
                user.HomeAddress = Input.HomeAddress;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "Default");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("/DefaultUser/Index");  // Redirect to DefaultUser view
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }


        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }

    // RegisterValidator is a class that defines the validation rules for user registration. - used fluent balidation
    // It inherits from AbstractValidator<RegisterModel.InputModel>, meaning it will be used to validate the InputModel class from the RegisterModel.
    public class RegisterValidator : AbstractValidator<RegisterModel.InputModel>
    {
        // The constructor of RegisterValidator where the validation rules are defined
        public RegisterValidator()
        {
            // Define validation rules for the "Firstname" property of the RegisterModel.InputModel.
            // Ensures that the Firstname is not empty and does not exceed 50 characters.
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("First Name is required.") // Firstname must not be empty
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters."); // Firstname must not exceed 50 characters

            // Define validation rules for the "Lastname" property of the RegisterModel.InputModel.
            // Ensures that the Lastname is not empty and does not exceed 50 characters.
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Last Name is required.") // Lastname must not be empty
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters."); // Lastname must not exceed 50 characters

            // Define validation rules for the "Email" property of the RegisterModel.InputModel.
            // Ensures that the Email is not empty and follows a valid email format.
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.") // Email must not be empty
                .EmailAddress().WithMessage("Invalid Email Address."); // Email must match a valid email address format

            // Define validation rules for the "PhoneNumber" property of the RegisterModel.InputModel.
            // Ensures that the PhoneNumber is not empty and matches a specific pattern (South African phone format in this case).
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.") // PhoneNumber must not be empty
                .Matches(@"^\+27\d{9}$").WithMessage("Phone Number must be in the format +27123456789."); // Validates the phone number with regex to match a South African phone format

            // Define validation rules for the "Password" property of the RegisterModel.InputModel.
            // Ensures that the Password is not empty and has a minimum length of 6 characters.
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.") // Password must not be empty
                .MinimumLength(6).WithMessage("Your password length must be at least 6."); // Password must be at least 6 characters long

            // Define validation rules for the "ConfirmPassword" property of the RegisterModel.InputModel.
            // Ensures that the ConfirmPassword matches the Password field.
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match."); // ConfirmPassword must be equal to Password

            // Validation for Faculty
            RuleFor(x => x.Faculty)
                .NotEmpty().WithMessage("Faculty is required.") // Faculty must not be empty
                .MaximumLength(50).WithMessage("Faculty cannot exceed 50 characters."); // Faculty must not exceed 50 characters

            // Validation for IDNumber
            RuleFor(x => x.IDNumber)
                .NotEmpty().WithMessage("ID Number is required.") // ID Number must not be empty
                .Matches(@"^\d{13}$").WithMessage("ID Number must be 13 digits."); // Validate the ID Number format as 13 digits

            // Validation for HomeAddress
            RuleFor(x => x.HomeAddress)
                .NotEmpty().WithMessage("Home Address is required.") // Home Address must not be empty
                .MaximumLength(200).WithMessage("Home Address cannot exceed 200 characters."); // Home Address must not exceed 200 characters
        }
    }



}
