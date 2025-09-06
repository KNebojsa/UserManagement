# 📦 UserManagement API

A clean, modular API for managing users, built with **.NET 8 (LTS)** and **Entity Framework Core (Code-First)**.  
Includes Swagger support, authentication, and initial test data for quick API validation.

---

## 🛠️ Tech Stack

- **.NET 8.0 (LTS)**
- **Entity Framework Core**
- **SQL Server / SQL Express**
- **Swagger / OpenAPI**
- **RESTful API principles**

---

## 📌 Why .NET 8?

While .NET 9.0 is available, it is **not** a long-term support release.  
For stability and support reasons, **.NET 8.0** was chosen. This ensures the project benefits from the latest stable features while maintaining long-term reliability.

> Upgrading to newer versions like .NET 9 or beyond can be considered once they reach LTS status.

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
```

---

### 2. Set `UserManagement.Api` as Startup Project

Make sure to set the `UserManagement.Api` project as the **Startup Project**:

> 🖥️ **Visual Studio**:  
> Right-click on `UserManagement.Api` in Solution Explorer → **"Set as Startup Project"**

---

### 3. Update the connection string

Open `appsettings.json` inside `UserManagement.Api` and set your SQL Server configuration:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\YOUR_SERVER_NAME;Database=UserManagementDb;Trusted_Connection=True;TrustServerCertificate=true;"
}
```
> ⚠️ **Important:** Never commit real credentials or secrets to version control.

---

### 4. Apply EF Core migrations

Navigate to the infrastructure project directory and apply migrations:

```bash
cd src/UserManagement.Infrastructure
dotnet ef database update
```

✅ This command will:
- Create the database if it doesn't exist.
- Apply all existing migrations.
- Seed initial test data automatically.

---

### 5. Test Data (for development)

Use the following data for testing login/authentication:

- **Test API Key:** `398dec7c-a80d-4428-b31e-4c0dafea9b4f`  
- **Test User ID:** `F5436636-54DA-46DD-B869-31C1239508C6`

#### 🔐 Test Credentials: 

```json
{
  "userName": "Nebojsa123",
  "password": "Password123!"
}
```
> ⚠️ **Important:** Test data are included here solely for assignment purposes and would not typically be present in a README file in a professional setting.
---

### 6. Build and Run the Application

From the `UserManagement.Api` folder:

```bash
dotnet build
dotnet run
```

---

### 7. Access the Swagger API Documentation

Once the app is running, open your browser and navigate to:

```
https://localhost:7200/swagger
```

---

## 📎 Notes

- Make sure **.NET 8.0 SDK** is installed (LTS version).
- EF Core migrations are located in the `UserManagement.Infrastructure` project.
- Initial test data helps you verify functionality without manually adding data.
- The solution uses a **modular architecture** separating infrastructure, domain, and API layers.

---

## ✅ Useful EF Core Commands

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Remove the last migration
dotnet ef migrations remove -p

# Update the database
dotnet ef database update -p
```

---

## 📬 Contact

Have questions or suggestions?  
Open an issue or submit a pull request!

Or reach to developer at kovacic.nebojsa96@gmail.com

---
