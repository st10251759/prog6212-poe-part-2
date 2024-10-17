# Contract Monthly Claim System (CMCS) - ClaimStream Prototype

## Overview
ClaimStream is a prototype web application designed to manage lecturer claims within an academic institution. This prototype focuses on the front-end design and basic functionalities for lecturers, programme coordinators, and academic managers.This is a Web App Prototype, that provides a clear foundation for the eventual automation of claim submissions, approvals, and payment processing. This README outlines the development phases, application usage, and future requirements for the system.

## Table of Contents
1. [Overview](#overview)
2. [Sprints](#sprints)
   - [Sprint Part 1](#sprint-part-1)
   - [Sprint Part 2](#sprint-part-2)
   - [Sprint Part 3](#sprint-part-3)
3. [Cloning the Project and Restoring the Database](#cloning-the-project-and-restoring-the-database)
4. [Assumptions for Usage](#assumptions-for-usage)
5. [Functionality and App Usage](#functionality-and-app-usage)
6. [Future Requirements](#future-requirements)
7. [Code Attribution](#code-attribution)

## Sprints

### Sprint Part 1
**Part 1: Project Documentation**
- **Deliverables:**
  - UML Class Diagram for the Database
  - Project Plan
  - GUI Design
- **Goal:**
  - Develop a clear visual plan for the application.
  - Implement a class diagram to illustrate the structure of the database.
  - Create a project plan and wireframes for the graphical user interface.

### Sprint Part 2
**Part 2: Implementing the Web Application Prototype**
- **Deliverables:**
  - MVC ASP.NET Core Web Application linked to SQL Server.
  - The application should allow lecturers to submit claims, and for coordinators and academic managers to approve or reject claims.
  - Provide a clean, functional interface for submitting claims and managing approval workflows.
  
- **Features Implemented:**
  - **Lecturer Claims Submission:** Simple form to input hours worked, hourly rate, and additional notes.
  - **Document Upload:** Support for uploading PDF files up to 15MB in size.
  - **Approval Workflow:** Programme coordinators and academic managers can approve or reject claims.
  - **Claim Status Tracking:** Real-time status updates (Pending, Approved, Rejected) visible to users.
  - **Unit Testing:** Tests for claim submission and file validation.

### Sprint Part 3
**Part 3: Future Functionality**
- **Planned Features:**
  - Automate final payment processing based on approved claims.
  - Implement an automated system to verify submitted claims against predefined criteria.
  - Automatically generate reports for HR Managers, summarizing approved claims for processing.

## Cloning the Project and Restoring the Database
1. Clone the repository from GitHub:
    ```bash
    git clone https://github.com/st10251759/prog6212-poe-part-2.git
    ```
2. Open the project in Visual Studio and restore NuGet packages.
3. Ensure that SQL Server Management Studio (SSMS) is installed and running.
4. Import the database backup file from the `Database` folder within the repository:
    - Open SSMS and connect to your local server.
    - Right-click on `Databases`, select `Restore Database`, and follow the instructions to restore the database from the `.bak` file provided.
5. Update the connection string in the `appsettings.json` file with your local database configuration.

## Assumptions for Usage
- **User Roles:**
  - **Lecturers:** Can submit claims with supporting documents.
  - **Programme Coordinators:** Verify and approve/reject claims.
  - **Academic Managers:** Provide final approval or rejection.
  
- **Hourly Rate:** Ranges between R200 and R1000.
- **Claim Period:** Lecturers can submit claims for either the current or the previous month.
- **Supporting Documentation:** Lecturers must upload a signed document verifying their hourly rate and contract conditions.

## Functionality and App Usage

1. **Claim Submission:**
    - Lecturers can submit their claims through a form that includes fields for hours worked, hourly rate, and additional notes.
    - A file upload button allows lecturers to attach PDF documents (e.g., proof of contract, hourly rate).
    - Upon submission, the claim is routed to the Programme Coordinator for approval.

2. **Approval Process:**
    - **Programme Coordinator:** Approves or rejects claims submitted by lecturers. If approved, the claim is forwarded to the Academic Manager.
    - **Academic Manager:** Provides the final approval or rejection. Claims that are fully approved are marked as "Approved" in the system.

3. **Claim Tracking:**
    - Lecturers can track their claims' status as they move through the approval process.
    - Statuses include "Pending", "Approved", and "Rejected."

4. **Document Management:**
    - Uploaded files are securely stored in the system and linked to the relevant claim.
    - A maximum file size of 15MB is enforced, and only PDF files are accepted.

5. **Error Handling:**
    - Unit tests ensure that claim submission forms are correctly validated.
    - Tests for invalid file uploads and error messages help maintain system reliability.

## Future Requirements
- **Automation of Final Payment:** Automate the calculation and payment process for claims based on the hours worked and the hourly rate.
- **Automated Verification System:** Automatically check submitted claims against predefined conditions (e.g., contract hours, hourly rate limits).
- **Reporting and Invoices:** HR Managers will have the ability to generate invoices and reports summarizing approved claims for payment processing.
- **Integration with Payment Gateways:** Allow for seamless payment processing once claims are approved.

## Code Attribution

- **HTML & CSS Resources:**
  - Author: [w3schools](https://www.w3schools.com/html/)
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

---

For any further questions or issues, feel free to contact the project owner - Cameron Chetty - ST10251759@vvconnect.edu.za - 081 262 5090
