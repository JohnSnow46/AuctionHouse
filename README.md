# AuctionHouse - Online Auction System

A comprehensive online auction management application built with modern .NET technologies. This project demonstrates a professional approach to web application development, including best practices, SOLID architecture principles, and a responsive user interface.

## 🎯 Features

### For Users
- **User Registration & Authentication** - Secure account creation and login
- **Browse Auctions** - Search and view ongoing, upcoming, and completed auctions with detailed item descriptions
- **Bid Management** - Place bids on auction items and receive real-time notifications when outbid
- **Item Listings** - Create auction listings with configurable starting prices, reserve prices, and auction durations
- **Transaction History** - Track completed auctions, placed bids, and purchase history

### Technical Features
- Responsive user interface across all devices
- Real-time notification system
- Secure transaction processing
- Detailed bidding history and audit trail

## 🛠️ Technology Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | .NET Core, C#, ASP.NET MVC/Core |
| **Frontend** | HTML5, CSS3, JavaScript |
| **Database** | SQL Server |
| **Architecture** | MVC, Entity Framework Core |

## 📋 Requirements

- **.NET 6.0+** or higher
- **SQL Server 2019+** (or SQL Server Express)
- **Visual Studio 2022** (recommended) or Visual Studio Code with C# extensions

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/JohnSnow46/AuctionHouse.git
cd AuctionHouse
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Configure Database
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AuctionHouseDb;Trusted_Connection=true;"
  }
}
```

### 4. Apply Database Migrations
```bash
dotnet ef database update
```

### 5. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001`

## 📁 Project Structure

```
AuctionHouse/
├── Models/              # Data models (Auction, User, Bid, etc.)
├── Controllers/         # ASP.NET MVC controllers
├── Views/              # HTML/Razor views
├── Services/           # Business logic layer
├── Data/               # Entity Framework DbContext
├── wwwroot/            # Static assets (CSS, JS, images)
└── appsettings.json    # Application configuration
```

## 🔐 Security

- ASP.NET Identity-based authentication
- Password encryption
- CSRF protection
- Server-side and client-side data validation
- SQL Injection prevention via Entity Framework Core
- Authorization checks on sensitive operations

## 📝 License

This project is licensed under the MIT License.