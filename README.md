# Restaurant Carol App 🍽️

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET%208.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-Windows_Presentation_Foundation-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

A robust desktop application built in **WPF (.NET 8.0)** using **C#** for comprehensive restaurant management. The project implements a clean **N-Tier (3-Tier) Architecture** and adheres strictly to the **MVVM (Model-View-ViewModel)** design pattern, ensuring a clear separation between the graphical user interface and the business logic.

## 🌟 Key Features

The application is role-based, providing specific functionalities tailored to each type of user:

### 👤 Client (User)
- **Menu Browsing:** View categories (Food / Drinks), individual dishes, and custom menus.
- **Shopping Cart:** Add products to the cart, adjust quantities, and view subtotals in real-time.
- **Order Placement:** Finalize orders with delivery address selection.
- **Automatic Discounts:** Smart discount system applied automatically in the cart (Loyalty Discount or High-Value Order Discount).
- **Order Tracking:** Monitor order status in real-time (Registered -> Preparing -> Delivered/Canceled).
- **Loyalty Points System:** Accumulate reward points upon order completion.

### 👨‍🍳 Chef (Bucătar)
- **Order Management:** View pending orders assigned to the kitchen.
- **Status Updates:** Change order status to "Preparing".

### 🛵 Delivery Driver (Livrator)
- **Order Pickup:** View orders ready for delivery.
- **Finalize Delivery:** Mark orders as "Delivered" (which instantly triggers the physical stock deduction in the database).

### 👔 Manager
- **Catalog Management:** Add, edit, and delete Dishes and Categories.
- **Custom Menus:** Create complex menus consisting of multiple individual dishes.
- **Stock Management:** View and update inventory. Get alerts for low-stock products.
- **User & Staff Management:** Create new accounts for staff members (Chefs, Delivery Drivers).
- **Order History:** View all active and past orders across the restaurant.

## 🏗️ Project Architecture

The project is structured in layers (N-Tier) to ensure decoupled, testable, and maintainable code:

1. **Presentation Layer (UI - WPF)**
   - Contains `Views` (`.xaml` and `.xaml.cs` files).
   - Contains `ViewModels` handling presentation logic (Data Binding, `ICommand` for button actions, `INotifyPropertyChanged` implementation).
   - Modern UI design (Dark & Gold theme, custom buttons with rounded corners, subtle animations).

2. **Business Logic Layer (BLL)**
   - Contains core application logic (e.g., `ComandaBLL.cs`, `AutentificareBLL.cs`).
   - Handles validations and complex calculations (e.g., automatic discount computation).

3. **Data Access Layer (DAL)**
   - Directly interacts with the SQL Server database (`PreparatDAL.cs`, `ComandaDAL.cs`, etc.).
   - Uses `Microsoft.Data.SqlClient` (ADO.NET) **exclusively through Stored Procedures**, ensuring maximum performance and security against SQL Injection.

4. **Entity Layer**
   - Data models / POCO classes used for data transfer between layers (e.g., `Comanda.cs`, `Preparat.cs`, `Utilizator.cs`).

## 💾 Database (SQL Server)

The database is designed in compliance with the **Third Normal Form (3NF)** to eliminate data redundancy.
- **Stored Procedures:** Over 40 stored procedures handle everything (CRUD operations, authentication, complex transactions).
- **Transactions:** Intensively used for sensitive operations (e.g., placing an order, where the order, its items, and availability are all checked and inserted simultaneously within a `TRY/CATCH` block).
- **Stock Management:** Inventory is NOT deducted at the time of order placement, but rather when the order is marked as "Delivered", ensuring data consistency in the event of cancellations.

## 🚀 How to Run Locally

### System Requirements
- **IDE:** Visual Studio 2022
- **Framework:** .NET 8.0 SDK
- **Database:** SQL Server (e.g., SQLEXPRESS)

### Installation Steps
1. **Clone the repository:**
   ```bash
   git clone https://github.com/YourUsername/Restaurant-Carol-App.git
   ```
2. **Configure the Database:**
   - Ensure your SQL Server instance is running.
   - Execute the SQL scripts located in the project directory (if available) to recreate tables, relationships, and stored procedures.
3. **Configure the Connection String:**
   - Open the `DALHelper.cs` file (located in the `DataAccessLayer/Helpers` folder) or the `.config` file.
   - Update the `Data Source` with your SQL Server instance name (e.g., `.\SQLEXPRESS` or `localhost`).
4. **Build & Run:**
   - Open the `.sln` solution file in Visual Studio.
   - Run the application (`F5`).

## 🎨 UI / UX Principles Used
- Extensive use of WPF-specific **DataBinding**.
- Centralized resources in `App.xaml` for colors, button templates (`ControlTemplate`), and converters (e.g., `BoolToVisibilityConverter`).
- Custom User Controls (`UserControl`) to create a modular, "Single Page Application" feel on desktop via dynamic navigation (switching content inside a central panel without opening new windows).