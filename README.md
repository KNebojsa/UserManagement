# UserManagement API

## Running the Project and Applying Migrations

This project uses **Entity Framework Core (Code-First)** with **.NET 8.0**, which is the latest stable **Long-Term Support (LTS)** version at the time of development.

### Why .NET 8?

While .NET 9.0 is available, it is **not** a long-term support release. For stability and support reasons, .NET 8.0 was chosen. This ensures the project benefits from the latest stable features while maintaining long-term reliability.

> Upgrading to newer versions like .NET 9 or beyond can be considered once they reach LTS status.

---

## 🚀 Steps to Run Locally

### 1. Clone the repository

```bash
git clone <repository-url>
```

### 2. Apply migrations to create/update the database

Navigate to the `src\UserManagement.Infrastructure` directory and run:

```bash
dotnet ef database update
```

✅ The included migrations insert initial test data automatically.

---

### 3. Test Data (for development)

- **Test API Key:** `398dec7c-a80d-4428-b31e-4c0dafea9b4f`  
- **Test User ID:** `F5436636-54DA-46DD-B869-31C1239508C6`

#### User Authentication:

```json
{
  "userName": "Nebojsa123",
  "password": "Password123!"
}
```

---

### 4. Build the application

```bash
dotnet build
```

### 5. Run the application

```bash
dotnet run
```

---

### 6. Access Swagger API documentation

Open your browser and navigate to:

```
https://localhost:7200/swagger
```

---

## 📝 Notes

- Requires **.NET 8.0 SDK** installed (LTS stable release).
- Migrations are located in the `UserManagement.Infrastructure` project.
- Initial test data is automatically inserted to enable quick verification of API functionality.
