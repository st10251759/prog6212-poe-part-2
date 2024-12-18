# Contract Monthly Claim System (CMCS) - ClaimStream Prototype

## Overview
ClaimStream is a prototype web application designed to manage lecturer claims within an academic institution. This prototype focuses on front-end design and enhanced functionality for lecturers, programme coordinators, academic managers, and HR managers. The application provides a foundation for automating claim submissions, approvals, and payment processing. This README outlines the development phases, enhanced functionality, and application usage.

## Table of Contents
1. [Overview](#overview)
2. [Repository Contents](#repository-contents)
3. [Sprints](#sprints)
   - [Sprint Part 1](#sprint-part-1)
   - [Sprint Part 2](#sprint-part-2)
   - [Sprint Part 3 - Final Application Build](#sprint-part-3)
4. [Cloning the Project and Restoring the Database](#cloning-the-project-and-restoring-the-database)
5. [Assumptions for Usage](#assumptions-for-usage)
6. [Lecturer Feedback](#lecturer-feedback)
7. [Functionality and App Usage](#functionality-and-app-usage)
8. [Future Requirements](#future-requirements)
9. [Code Attribution](#code-attribution)

## Repository Contents

The following files are uploaded into this repository:

- **ST10251759_PROG6212_POE_Tests.zip**  
  This is the Unit Tests Project containing 12 unit tests to validate the functionality of the application. The tests ensure accurate form submission, claim processing, and user interaction.

- **Prog6212_DB.bak**  
  This is the SQL Server Management Studio (SSMS) Database Backup of the local database used in the application. It includes all tables, relationships, and seeded data required for testing the application.

- **ST10251759 - CAMERON CHETTY - PROG6212 - POE - Final.pdf**  
  This is the final submission document. It includes submission information, screenshots of the app running locally, code snippets, and code attribution details.

- **ST10251759 - PROG6212 - POE PowerPoint Presentation.pptx**  
  A PowerPoint presentation showcasing how the application adds value to the Contract Monthly Claim System (CMCS). It outlines core features and provides an overview of the app's development process.

- **YouTube Presentation Link**  
  [Watch the presentation here](https://youtu.be/JtpHDXJFdgs).  
  This video provides a demonstration of the application in action, alongside an explanation of its features and workflows.

- **ST10251759_PROG6212_POE.sln**  
  This is the Visual Studio solution file for the MVC application. It includes all associated folders and files, such as controllers, views, models, and configuration files necessary for building and running the project.


## Sprints

### Sprint Part 1
**Part 1: Project Documentation**
- **Deliverables:**
  - UML Class Diagram for the Database
  - Project Plan
  - GUI Design
- **Goal:**
  - Develop a structured design plan for the application.
  - Provide a class diagram illustrating database structure.
  - Create project and wireframe plans for the GUI.

### Sprint Part 2
**Part 2: Implementing the Web Application Prototype**
- **Deliverables:**
  - MVC ASP.NET Core Web Application linked to SQL Server.
  - Initial setup for claim submission and approval workflows.
  
- **Features Implemented:**
  - **Lecturer Claims Submission:** Form for inputting hours, hourly rate, and notes.
  - **Document Upload:** Support for PDF uploads up to 15MB.
  - **Approval Workflow:** Approval process for coordinators and managers.
  - **Claim Status Tracking:** Status updates for Pending, Approved, and Rejected.
  - **Unit Testing:** Tests for claim submission and file validation.

### Sprint Part 3
**Part 3: Application Enhancement (Automation)**
For the final build, enhancements were made to streamline the claim submission, verification, and payment processes, with new roles and automated workflows for a smoother user experience. I also added fluenet validation when users register.

- **Lecturer View: Claim Submission Automation**
  - Added functionality to auto-calculate final payment based on hours worked and hourly rate.
  - Implemented validation checks to ensure accurate data entry.
  - **Tools:** Built using ASP.NET Core MVC with client-side calculations via jQuery, and Entity Framework for database interaction.

- **Programme Coordinator and Academic Manager View: Claim Verification and Approval Automation**
  - Automated claim verification based on predefined criteria (hours worked, hourly rate).
  - Introduced approval workflows to simplify the verification and approval process.
  - **Tools:** ASP.NET Identity for authentication and authorization, Web API for front-end/back-end communication, Entity Framework for data operations, and FluentValidation for workflow criteria validation.

- **HR View: Claim Processing Automation**
  - Developed automated invoice and report generation for HR managers to review and process payments.
  - HR managers can view and download PDF reports summarizing approved claims.
  - Added functionality for managing lecturer data (e.g., updating contact information).
  - **Tools:** Integrated tools to enable PDF export, data viewing, and role-based access for HR managers.

## Cloning the Project and Restoring the Database
1. Clone the repository from GitHub:
    ```bash
    git clone https://github.com/st10251759/prog6212-poe-part-2.git
    ```
2. Open the project in Visual Studio and restore NuGet packages.
3. Ensure SQL Server Management Studio (SSMS) is installed and running.
4. Import the database backup file from the `Database` folder:
    - Open SSMS and connect to your local server.
    - Right-click on `Databases`, select `Restore Database`, and restore from the `.bak` file provided.
5. Update the connection string in `appsettings.json` with your database configuration.

## Assumptions for Usage

1. **User Roles:**  
   - Four roles: **Lecturers**, **Programme Coordinators**, **Academic Managers**, and **HR Managers**, each with role-based page access.
  
2. **Lecturer Claim Submission:**  
   - Lecturers can submit claims with an **hourly rate between R200 and R1000** and **1 to 150 hours** worked per month. Lecturers can only submit claims for the previous month or current month not for the future months or greater than a period of 31 days.
  
3. **Claim Period:**  
   - Claims can only be submitted for the **current** or **previous month**.

4. **Supporting Documentation:**  
   - Proof of hourly rate and contract details required, verified by the Programme Coordinator.

5. **Processing of Payments:**  
   - Hr Managers will process a payment of a claim and pay with a third-party payment tool. 

## Lecturer Feedback
I received no lecturer feedback for Part 1 and Part 2, as I obtained 100% for both parts. All changes and additional features were solely added to meet the requirements for Part 3, including enhancements to claim submission automation, verification workflows, and payment processing functionality.

## Functionality and App Usage

1. **Claim Submission:**
    - Lecturers submit claims via a form with fields for hours worked, hourly rate, and notes.
    - Document upload button for attaching PDF files as proof.
    - Claim is routed to the Programme Coordinator for approval.

2. **Approval Process:**
    - **Programme Coordinator:** Reviews and approves/rejects claims. Approved claims are sent to the Academic Manager.
    - **Academic Manager:** Final approval step. Fully approved claims are marked as "Approved".

3. **HR Role and Automation:**
    - HR Managers can view, download, and process payments of claims marked as approved.
    - Auto-generated reports and invoices summarizing approved claims.

4. **Claim Tracking:**
    - Lecturers can monitor claim status updates throughout the approval process.

5. **Error Handling and Validation:**
    - Comprehensive unit tests for claim submission, form validation, and file handling.

## Future Requirements
- **Integration with Payment Gateways:** Enable seamless payment processing once claims are fully approved.
- **Advanced Reporting:** Expand reporting capabilities for detailed financial insights.
- **Further Workflow Automation:** Implement additional approval steps or criteria for complex claims.

## Code Attribution

- **HTML & CSS Resources:**
  - Author: w3schools
  - Date Accessed: 12 October 2024
  - Author: [w3schools](https://www.w3schools.com/css/)
  - Date Accessed: 12 October 2024

- **MVC ASP.NET Core Application:**
  - Author: Fatima Shaik
  - Link: [EmployeeLeaveManagement_G1](https://github.com/fb-shaik/PROG6212-Group1-2024/blob/main/EmployeeLeaveManagement_G1.zip)
  - Date Accessed: 11 October 2024

- **Database Work:**
  - Author: Microsoft
  - Link: [Working with SQL](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-8.0&tabs=visual-studio)
  - Date Accessed: 11 October 2024

- **LINQ and File Handling:**
  - Author: Fatima Shaik
  - Link: [Employee_Management_LINQ_FileHandling_G1](https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/Employee_Management_LINQ_FileHandling_G1)
  - Date Accessed: 11 October 2024

- **Microsoft Identity Integration:**
  - Author: Andy Malone MVP
  - Link: [Microsoft Identity Tutorial](https://www.youtube.com/watch?v=zS79FDhAhBs)
  - Date Accessed: 11 October 2024

- **PDF File Handling:**
  - Author: Fatima Shaik
  - Link: [FileHandlingApp](https://github.com/fb-shaik/PROG6212-Group1-2024/tree/main/FileHandlingApp)
  - Date Accessed: 11 October 2024

- **PDF Creating for Reports Resource:**
  - Author: C# Corner
  - Link: [GenerateReports](https://www.c-sharpcorner.com/UploadFile/f2e803/basic-pdf-creation-using-itextsharp-part-i/)
  - Date Accessed: 13 Novemeber 2024

- **Admin Panel to Manage Users Resource:**
  - Author: Code A Future
  - Link: [ManageUsers](https://www.youtube.com/watch?v=WIUI_dLZpgs)
  - Date Accessed: 14 Novemeber 2024

- **Fluent Validation Logic:**
  - Author: FluentValidation
  - Link: [FluentValidation](https://docs.fluentvalidation.net/en/latest/index.html)
  - Date Accessed: 15 Novemeber 2024
 
- **Regular Expression Language:**
  - Author: Microsoft
  - Link: [RegualrExpressionC#](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference)
  - Date Accessed: 15 Novemeber 2024

---

For any further questions or issues, feel free to contact the project owner - Cameron Chetty - ST10251759@vvconnect.edu.za - 081 262 5090
