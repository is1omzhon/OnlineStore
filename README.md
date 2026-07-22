# 🛒 OnlineStore

> A feature-rich console-based e-commerce application built with C# and .NET 10.  
> Manage products, process orders, track inventory, and handle real-time notifications.

[![C#](https://img.shields.io/badge/C%23-10.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-black.svg)](https://github.com/is1omzhon/OnlineStore)

---

## 🚀 Features

### Core Functionality
- ✅ **Product Management** – Add, view, search, and update products
- ✅ **Inventory Tracking** – Monitor stock levels with real-time updates
- ✅ **Order Processing** – Place orders with queue-based processing
- ✅ **Category Management** – Organize products by categories
- ✅ **Data Persistence** – Save/load products from file (`products.txt`)
- ✅ **Advanced Search** – Search products by name
- ✅ **Statistics Dashboard** – View product metrics (min/max/avg price, total stock)

### Smart Features
- ⚠️ **Low Stock Alerts** – Automatic notifications when stock < 5
- 📧 **Email Simulation** – Notify admins about low stock
- 🔔 **Event-Driven Architecture** – Using delegates and events
- 🛡️ **Custom Exceptions** – ProductNotFoundException, InvalidPriceException, ProductOutOfStockException

---

## 🛠️ Tech Stack

| Technology | Description |
|------------|-------------|
| **C# 10.0** | Primary programming language |
| **.NET 10** | Framework |
| **Collections** | Dictionary, HashSet, Queue, List |
| **Asynchronous Programming** | Async/await for file I/O |
| **Event Delegates** | ProductAddedHandler, StockChangedHandler |
| **LINQ** | Query operations (Where, OrderBy, GroupBy, Sum, Max, Min) |

---

## 📁 Project Structure
#### OnlineStore/
#### ├── Models/
#### │ └── Product.cs # Product entity with properties
#### ├── Exceptions/
#### │ ├── ProductNotFoundException.cs
#### │ ├── InvalidPriceException.cs
#### │ └── ProductOutOfStockException.cs
#### ├── Program.cs # Main application logic
#### ├── OnlineStore.csproj # Project configuration
#### └── products.txt # Data storage (auto-generated)



---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)

### Installation

```bash
# Clone the repository
git clone https://github.com/is1omzhon/OnlineStore.git

# Navigate to project directory
cd OnlineStore

# Run the application
dotnet run