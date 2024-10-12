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

 Microsfot Identity
 Author: Andy Malone MVP
 Link: https://www.youtube.com/watch?v=zS79FDhAhBs
 Date Accessed: 11 October 2024

 ==============================Code Attribution==================================

 */

//Import List

using Microsoft.AspNetCore.Identity; // Importing the ASP.NET Core Identity namespace for user authentication and management

namespace ST10251759_PROG6212_POE.Models // Defining a namespace for the project models
{
    // Defining a class ApplicationUser that extends the IdentityUser class provided by ASP.NET Core Identity
    public class ApplicationUser : IdentityUser
    {
        // Property to store the user's first name
        public string Firstname { get; set; } // Represents the user's first name

        // Property to store the user's last name
        public string Lastname { get; set; } // Represents the user's last name

        // Navigation property to establish a one-to-many relationship with the Claim class
        // This property will hold a collection of claims associated with this user
        public virtual ICollection<Claim> Claims { get; set; } // Represents the list of claims linked to the user
    }
}
