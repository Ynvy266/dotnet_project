Pottery Management System

I.Overview

The Pottery Management System is a .NET-based application designed to streamline the management and sales of pottery items. This system is tailored for pottery businesses, enabling them to handle inventory, orders, customer information, and sales reports efficiently.

II.Features

1. Inventory Management

- Add, update, and delete pottery items.
- Track stock levels and categorize items.
  
2. Sales Management

- Record and manage sales transactions.
- Generate sales invoices and receipts.

3. Customer Management

- Maintain a database of customer details.
- Track purchase history.

4. Reporting and Analytics

- Generate sales and inventory reports.

5. Technology Stack

- Framework: .NET 6.0 or higher
- Frontend: ASP.NET Core MVC with Razor Pages
- Backend: ASP.NET Core Web API
- Database: Microsoft SQL Server
- Authentication: ASP.NET Identity
- Tools: Entity Framework Core, LINQ

III. Prerequisites

Install .NET SDK 6.0 or higher.
Install Microsoft SQL Server.
Ensure Visual Studio (2022 or higher) or Visual Studio Code is installed.

IV. Setup Instructions

B1: Clone the repository:
git clone <repository-url>
B2: Navigate to the project directory:
cd SellPotteryManagement
B3: Restore dependencies:
dotnet restore
B4: Configure the database connection string in appsettings.json.
Set up Google OAuth2 credentials for authentication in appsettings.json:
"Authentication": {
  "Google": {
    "ClientId": "<your-client-id>",
    "ClientSecret": "<your-client-secret>"
  }
}
B5:Apply database migrations:
dotnet ef database update
B6:Run the application:
dotnet run

V. Usage

Access the application at https://localhost:5030.
Log in with admin credentials (default: admin@example.com / password) or use Google login.
Navigate through the dashboard to manage inventory, sales, and customers.

VI. License

This project is licensed under the MIT License. See the LICENSE file for more information.
