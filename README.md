# Restaurant Carol App

![App Demo](RestaurantCarol.WPF/Images/clientVideo.mp4)

A restaurant management app built with **.NET 8**, featuring a **REST API**, **Blazor WebAssembly** client, and legacy **WPF** desktop application.

## Roles
The app has 4 user roles: Customer, Chef, Delivery Driver, and Manager. 

**Customers** can browse the menu by category, add items to their cart and place orders with a delivery address. Discounts are applied automatically and loyalty points are earned on each completed order. Order status can be tracked in real time.

**Chefs** manage incoming orders, can pick them up and update their status to "Preparing". They can also add, edit or delete dishes and categories, including uploading photos.

**Delivery drivers** see orders ready for pickup and mark them as "Delivered", which automatically deducts stock from the database.

**Managers** have full access: they manage the dish catalog and combo menus, monitor stock levels and can view the full order history.

## Architecture

- **RestaurantCarol.Client (Blazor WebAssembly)** — Modern Web UI with JWT auth, role-based authorization and responsive styling.
- **RestaurantCarol.API (ASP.NET Core Web API)** — RESTful backend with JWT authentication, role policies, and Swagger UI.
- **RestaurantCarol.Shared** — Shared DTOs and Enums.
- **RestaurantCarol.WPF** — Original desktop application.

## Database

SQL Server, designed in Third Normal Form. All DB operations go through 40+ stored procedures. Complex operations, like placing an order or deleting a dish use SQL transactions to keep data consistent if something goes wrong.

## How to Run

**Requirements:** Visual Studio 2022, .NET 8 SDK, SQL Server (SQLEXPRESS or any local instance)

1. Clone the repo
2. Run the SQL scripts from the project folder to create the tables and stored procedures
3. Update the connection string in `RestaurantCarol.API/appsettings.json` (and `RestaurantCarol.WPF/App.config`) with your SQL Server instance name
4. Set multiple startup projects in Visual Studio: `RestaurantCarol.API` and `RestaurantCarol.Client`, then press F5.
