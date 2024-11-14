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

namespace ST10251759_PROG6212_POE.Models // Defining the namespace for the project's model classes
{
    // Defining a view model class named EditUserViewModel
    // This class is used to facilitate editing user information in the application
    // It serves as a data transfer object between the view and the controller, specifically for updating user details
    public class EditUserViewModel
    {
        // Property to store the unique identifier for the user
        // This ID is typically a primary key, allowing precise identification of the user in the database
        public string Id { get; set; }

        // Property to store the user's first name
        // Used for displaying or updating the first name associated with a user account
        public string FirstName { get; set; }

        // Property to store the user's last name
        // Used for displaying or updating the last name associated with a user account
        public string LastName { get; set; }

        // Property to store the email address of the user
        // Email serves as a unique identifier for users, providing an easy-to-recognize, human-readable value for display
        public string Email { get; set; }

        // Property to store the current role of the user
        // This represents the main role assigned to the user, like "Admin" or "User," and is displayed for editing or confirming role changes
        public string Role { get; set; }

        // Property to store a list of all available roles in the system
        // The list provides options for the user to be reassigned to a different role
        // Useful for generating dropdown lists or selection inputs in the user interface
        public List<string> Roles { get; set; }
    }
}
