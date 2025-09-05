UserManagement API
Running the Project and Applying Migrations

This project uses Entity Framework Core Code-First with .NET 8.0, which is the latest stable long-term support (LTS) version at the time of development.

Why .NET 8?

While .NET 9.0 is available, it is not a long-term support release, so for stability and support reasons, .NET 8.0 was chosen. This ensures the project benefits from the latest stable features while maintaining long-term reliability. Upgrading to newer versions like .NET 9 or beyond can be considered when they reach stable or LTS status.

Steps to Run Locally

Clone the repository:

git clone <repo-url>

Apply migrations to create/update the database:

From the src\UserManagement.Infrastructure directory, run:

dotnet ef database update 

Included migrations insert initial test data:

Test API Key: 398dec7c-a80d-4428-b31e-4c0dafea9b4f

Test User ID: F5436636-54DA-46DD-B869-31C1239508C6

For testing UserAuthentication data:

 "userName": "Nebojsa123",
 "password": "Password123!",

Build the application:

dotnet build

Run the application:

dotnet run

Access Swagger documentation:

Open in your browser:

https://localhost:7200/swagger

Notes

Requires .NET 8.0 SDK installed (LTS stable release).

Migrations reside in the UserManagement.Infrastructure project.

Initial test data enables quick verification of API functionality.