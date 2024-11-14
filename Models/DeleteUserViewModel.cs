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
namespace ST10251759_PROG6212_POE.Models // Defining the namespace for the project models
{
    // Defining a view model class named DeleteUserViewModel
    // This class is specifically created to store and transfer data related to user deletion
    // It is commonly used to confirm and carry the information needed to delete a user in the application
    public class DeleteUserViewModel
    {
        // Property to store the unique identifier of the user to be deleted
        // This ID is typically a primary key in the database, often represented as a GUID or string identifier
        // Used to precisely identify which user to delete
        public string Id { get; set; }

        // Property to store the email address of the user to be deleted
        // Email is generally used as a secondary identifier, providing a human-readable form of user identification
        // This property can be displayed to confirm that the correct user is being deleted
        public string Email { get; set; }
    }
}

