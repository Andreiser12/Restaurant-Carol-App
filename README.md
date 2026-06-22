# Restaurant Carol App

A desktop restaurant management app built with **WPF** and **C#**. 

## Roles
The app has 4 user roles: Customer, Chef, Delivery Driver, and Manager. 

**Customers** can browse the menu by category, add items to their cart and place orders with a delivery address. Discounts are applied automatically and loyalty points are earned on each completed order. Order status can be tracked in real time.

**Chefs** manage incoming orders, can pick them up and update their status to "Preparing". They can also add, edit or delete dishes and categories, including uploading photos.

**Delivery drivers** see orders ready for pickup and mark them as "Delivered", which automatically deducts stock from the database.

**Managers** have full access: they manage the dish catalog and combo menus, monitor stock levels and can view the full order history.

## Architecture

The project is split into 3 layers:

- **Presentation (WPF)** — Views + ViewModels with data binding and MVVM pattern
- **Business Logic (BLL)** — validations and calculations (pricing, discounts, loyalty points)
- **Data Access (DAL)** — talks to SQL Server exclusively through Stored Procedures (ADO.NET)

## Database

SQL Server, designed in Third Normal Form. All DB operations go through 40+ stored procedures. Complex operations, like placing an order or deleting a dish use SQL transactions to keep data consistent if something goes wrong.

## How to Run

**Requirements:** Visual Studio 2022, .NET 8 SDK, SQL Server (SQLEXPRESS or any local instance)

1. Clone the repo
2. Run the SQL scripts from the project folder to create the tables and stored procedures
3. Update the connection string in `App.config` with your SQL Server instance name
4. Open the solution in Visual Studio and press F5

## Tech stack

- C#
- .NET 8
- WPF
- ADO.NET
- SQL Server
- BCrypt
- MVVM
