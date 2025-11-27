# EComLite – GRAD-695 Applied Project

## Main Features

### Authentication
- Register / Login using ASP.NET Core Identity
- Customized login & registration UI
- Authenticated pages (Cart & Orders)

### Product Catalog
- Products stored in SQL Server
- Product list + Product details page

### Shopping Cart
- Session-based cart using `CartService`
- Add items
- Remove items  
- Clear cart
- Automatic total calculation

### Checkout & Orders
- Converts cart to `Order + OrderItem` database records
- Price snapshot stored at checkout time
- Auto-generated random **OrderNumber**
- Order History page
- Order Details page with per-item breakdown

### Key Components

#### `Program.cs`
- Configures services, Identity, EF Core, sessions, and routing.

#### `ApplicationDbContext.cs`
- Contains DB sets:
  - `Products`
  - `Orders`
  - `OrderItems`
  - Identity tables

#### `CartService.cs`
- Stores cart in session
- Add / Remove / Clear cart
- Used by Cart page & Checkout logic

#### Razor Pages
- `/Products` — list & detail
- `/Cart` — view cart + checkout
- `/Orders` — list & detail
- `/Account` — login / register (Identity)

## Prerequisites

Before running the project, ensure you have installed:

- **.NET SDK 8.0.x**
- **SQL Server Express or any SQL Server**
- **Git**
- (Optional) **Visual Studio Code** or **Visual Studio 2022**

# How to Set Up & Run the Project

These steps allow anyone (including the instructor) to run the project on a fresh computer.

# Clone the repository

```bash
git clone https://github.com/minhsuanwu-project/EComLite.git
cd EComLite
```
## Setup Instructions

This section explains how to configure the database connection, apply EF Core migrations, and run the EComLite application on any machine (Windows, SQL Server Express, .NET 8).

---

# Configure the Database Connection

The project includes a development config example file:
```
EComLite.Web/appsettings.Development.json.example
```
From the project root (EComLite folder), run:
```
copy .\EComLite.Web\appsettings.Development.json.example .\EComLite.Web\appsettings.Development.json
```
Then edit the connection string, Open EComLite.Web/appsettings.Development.json and set the SQL Server connection

# Apply EF Core Database Migrations

Navigate into the Web project
```
cd EComLite.Web
```
Apply migrations
```
dotnet ef database update
```
You should see output like: 
Applying migration 'InitialIdentitySchema'
Applying migration 'AddECommerceEntities'
Done.

After this step, SQL Server will contain a database named EComLite with the full schema.
If the database already exists, you may see:
No migrations were applied. The database is already up to date.

# Run the Application
Make sure you're still inside EComLite.Web

run
```
dotnet run
```
You will see something similar to:
Now listening on: https://localhost:7xxx
Now listening on: http://localhost:5xxx

Go to the URL printed in the console, you can now use EComLite!


