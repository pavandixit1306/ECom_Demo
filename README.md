# Ecom_Demo

A custom sports-equipment ordering platform built with **ASP.NET Core 8 MVC**. Customers can browse products, customise gear (colours, logo upload), add items to a cart, apply discount codes and place orders. Admins manage products, inventory and orders, and view a reporting dashboard. A SignalR live chat connects customers with the team.

## Features

**Customers**
- Registration, login and logout (ASP.NET Core Identity, lockout after 5 failed attempts)
- Product catalogue with search, filtering and sorting
- Product customisation with logo upload (JPG/PNG, max 5 MB, magic-byte validated)
- Session-based shopping cart and server-validated discount codes
- Checkout with tax and shipping calculation (Standard / Express)
- Order confirmation, history and tracking
- Live chat with the support team (SignalR)

**Admins**
- Dashboard with revenue, order statistics, top products and customer analytics
- Product CRUD (MVC views and REST API)
- Inventory management with real-time low-stock alerts (SignalR)
- Order status updates and cancellation

**Platform**
- Role-based authorization (`Admin`, `Customer`) and custom policies
- CSRF protection on all POST actions
- Security headers middleware (CSP, X-Frame-Options, etc.)
- Background email queue and worker (MailKit)
- Request logging middleware, global exception filter, action logging filter
- Memory caching and response caching

## Tech stack

| Area | Technology |
|------|------------|
| Framework | ASP.NET Core 8 (MVC + Razor Pages) |
| Data | Entity Framework Core 8, SQL Server (LocalDB for development) |
| Auth | ASP.NET Core Identity, cookie authentication |
| Real-time | SignalR |
| Email | MailKit |
| Front end | Bootstrap 5, jQuery, Font Awesome, SCSS (compiled with Gulp) |
| CI/CD | GitHub Actions to Azure Web App |
| Tests | xUnit (`Ecom_Demo.Tests`) |

## Project structure

```
Controllers/      MVC controllers and the REST API (Controllers/API)
Data/             ApplicationDbContext and EF converters
Filters/          Global exception and action logging filters
Hubs/             SignalR hubs (ChatHub, InventoryHub)
Middleware/       Request logging and security headers
Migrations/       EF Core migrations
Models/           Entities and view models
Repositories/     Product and order data access
Services/         Order, cart, report, notification and email services
Views/            Razor views
wwwroot/          Static files (css, js, scss, images, lib)
Ecom_Demo.Tests/  Unit tests
```

## Getting started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server LocalDB (installed with Visual Studio)
- Node.js 18+ (only needed to rebuild SCSS/JS)

### Setup

1. **Clone and restore**
   ```bash
   git clone <your-repo-url>
   cd Ecom_Demo
   dotnet restore
   ```

2. **Set the admin seed password** (never commit this)
   ```bash
   dotnet user-secrets set "SeedAdmin:Password" "YourStrongPassword123!"
   ```
   The password must meet the Identity rules: at least 8 characters, with an uppercase letter, a lowercase letter, a digit and a symbol. The admin account is created as `admin@ecom_demo.com`.

3. **(Optional) Configure email**
   ```bash
   dotnet user-secrets set "Email:Host" "smtp.example.com"
   dotnet user-secrets set "Email:UserName" "your-user"
   dotnet user-secrets set "Email:Password" "your-smtp-password"
   ```
   If `Email:Host` is empty, emails are only logged and not sent.

4. **Run.** The database is created and migrated automatically on startup.
   ```bash
   dotnet run
   ```
   Then open the HTTPS URL printed in the console.

### Front-end assets (optional)
```bash
npm install
npm run build   # compile SCSS and bundle JS
npm run dev     # watch mode
```

### Tests
```bash
dotnet test
```

## Configuration

Settings live in `appsettings.json`:

| Section | Purpose |
|---------|---------|
| `ConnectionStrings:Ecom_DemoConnection` | SQL Server connection string |
| `OrderSettings` | Tax rate, shipping rates, free-shipping threshold, order limits |
| `Discounts:Codes` | Discount codes (Percentage, FixedAmount, FreeShipping) |
| `Email` | SMTP settings |

Use user-secrets (development) or environment variables / Azure App Settings (production) for anything sensitive. Do not put real credentials in `appsettings.json`.

## API

Base route: `/api/products`

| Method | Route | Access |
|--------|-------|--------|
| GET | `/api/products?page=1&pageSize=20` | Public |
| GET | `/api/products/{id}` | Public |
| GET | `/api/products/search?query=` | Public |
| GET | `/api/products/category/{category}` | Public |
| POST | `/api/products` | Admin |
| PUT | `/api/products/{id}` | Admin |
| DELETE | `/api/products/{id}` | Admin |

SignalR hubs: `/chathub` (authenticated users) and `/inventoryhub` (admins only).

## Deployment

Pushes to `master` trigger `.github/workflows/master_smartgear-angelopuza.yml`, which builds, publishes and deploys to an Azure Web App. Add the publish profile as a repository secret and set the connection string and other secrets in the Azure App Service configuration.

## Security notes

- Keep `bin/`, `obj/` and `.vs/` out of source control (see `.gitignore`).
- Keep dependencies patched. Run `dotnet list package --vulnerable --include-transitive` and `npm audit` regularly.
- Restrict `AllowedHosts` and use an encrypted connection string in production.

## License

Add a license of your choice (the npm manifest currently declares MIT).
