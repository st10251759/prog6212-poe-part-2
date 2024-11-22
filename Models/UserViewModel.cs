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

 Attributes
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/TaskManager_Attributes_demo/TaskManager_Attributes_demo
 Date Accessed: 11 October 2024

 MVC APP
 Author: Fatima Shaik
 Link: https://github.com/fb-shaik/PROG6212-Group1-2024/blob/main/EmployeeLeaveManagement_G1.zip
 Date Accessed: 11 October 2024

 ==============================Code Attribution==================================

 */

namespace ST10251759_PROG6212_POE.Models // Defines the namespace for the UserViewModel class, organizing it within the application's structure
{
    // This class represents a view model for a user, specifically designed to capture and display user-related information
    public class UserViewModel
    {
        // Property to store the unique identifier for the user
        // 'Id' is a string that typically holds a GUID or other unique identifier to distinguish each user record in the system
        public string Id { get; set; }

        // Property to store the first name of the user
        // 'FirstName' is a string that represents the user's first or given name
        public string FirstName { get; set; }

        // Property to store the last name of the user
        // 'LastName' is a string that represents the user's family name or surname
        public string LastName { get; set; }

        public string Faculty { get; set; } // New Faculty property to store faculty user belongs to e.g Faculty of IT, Faculty of Law

        public string IDNumber { get; set; } //New Proptery for ID Number

        public string HomeAddress { get; set; } //New Property for HomeAdress

        // Property to store the phonenumber of the user
        // 'PhoneNumbe' is a string that represents the user's first or given name
        public string PhoneNumber { get; set; }
        

        // Property to store the email address of the user
        // 'Email' is a string representing the user's email, which can serve as a unique identifier for login purposes
        public string Email { get; set; }

        // Property to store the role assigned to the user
        // 'Role' is a string representing the user's role in the system, such as "Admin," "User," or other roles defined in the application
        public string Role { get; set; }
    }
}
