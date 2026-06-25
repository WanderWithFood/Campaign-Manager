# Campaign Management

ASP.NET Core MVC application for managing campaigns.

## Project Structure

```
├── Controllers/       - MVC Controllers
├── Helpers/           - Base controllers, middleware, DB context, extensions
│   ├── DbContexts/    - Entity Framework DbContext
│   └── Middlewares/   - Global exception handling, activity logging, token helper
├── Interfaces/        - Repository and service interfaces
├── Models/            - Data models
│   ├── DTOs/          - Data Transfer Objects
│   └── Entity/        - EF Core entity models
├── Repositories/      - Repository implementations
├── Services/          - Business logic services
├── Views/             - Razor views
│   ├── Auth/          - Authentication views
│   ├── Home/          - Home/dashboard views
│   └── Shared/        - Shared layouts and partials
└── wwwroot/           - Static files (CSS, JS, images)
```

## Prerequisites

- .NET 10 SDK
- MySQL 8.0+
- Visual Studio 2022+

## Setup

1. Create the database by running `CreateTables.sql` against your MySQL server
2. Update the connection string in `appsettings.json`
3. Run the application:
   ```
   dotnet run
   ```

## Default Credentials

- **Email:** admin@campaignmanagement.com
- **Password:** Admin@123
