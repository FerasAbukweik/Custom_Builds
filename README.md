# Custom Builds

**Custom Builds** is a full-stack e-commerce platform for designing and ordering custom gaming peripherals (controllers and mechanical keyboards). Users can pick a base part, customize it section by section with priced modifications, track orders in real time, and chat live with support — all backed by a clean-architecture .NET API and a modern zoneless Angular frontend.

## ✨ Features

- **Custom Build Configurator** — Modular part → section → modification structure lets users build a controller or keyboard piece by piece, with live price calculation as selections change.
- **Storefront & Cart** — Product catalog with infinite/lazy scrolling, optimistic-update cart (instant UI feedback with automatic rollback on failure), and debounced quantity syncing to the server.
- **Order Tracking & History** — Step-based order progress tracking for customers, plus paginated order history with spend summaries.
- **Admin Dashboard** — Revenue charts (weekly/monthly), inventory stock alerts, paginated order management, and low-stock monitoring.
- **Live Support Chat** — Real-time customer ↔ support messaging over SignalR, with typing indicators and lazy-loaded chat history.
- **Authentication** — Cookie-based JWT auth with automatic silent token refresh on 401s via an HTTP interceptor, plus route guards for public/private areas.

## 🏗️ Architecture

The backend follows **Clean Architecture** with clear separation of concerns:

```
src/
├── Custom_Builds.Core/           # Domain entities, DTOs, enums, service & repository interfaces
├── Custom_Builds.Infrastructure/ # EF Core DbContext, repository implementations, external services
└── Custom_Builds.WebApi/         # Controllers, SignalR hubs, DI wiring, middleware
```

Key backend patterns:
- **Result pattern** instead of exceptions for predictable, explicit error handling across services
- **Repository pattern** with a consistent `Filter/GetById/Add/Remove` shape and expression-based includes/predicates
- **Interface Segregation** for cross-cutting concerns (e.g. split cookie read/write services)
- **Thin controllers** — business logic lives in services, not controllers
- Recursive CTE stored procedures (via `SqlQueryRaw`) for efficient hierarchical queries

The frontend is built with the latest **Angular** (standalone components, signals, zoneless change detection, and the new `@for`/`@if` control-flow syntax) styled with **Tailwind CSS 4**.

## 🧰 Tech Stack

| Layer | Technologies |
|---|---|
| Backend | ASP.NET Core (.NET 10), Entity Framework Core, SignalR, ASP.NET Identity |
| Frontend | Angular 21, TypeScript, Tailwind CSS, RxJS, Signals |
| Database & Caching | SQL Server, Redis |
| Real-time | SignalR (live chat, typing indicators) |
| Infra & DevOps | Docker, Docker Compose, Nginx |
| Logging & Media | Serilog + Seq, Cloudinary |
| Testing | xUnit, Moq, FluentAssertions, AutoFixture, EF Core InMemory provider |

## 🚀 Running Locally

### Run everything with Docker Compose

Create a `.env` file in the project root with the required variables:

```env
ASPNETCORE_ENVIRONMENT=Development
JWT_KEY=your-secret-key
DB_NAME=CompanySystemDb
SA_PASSWORD=YourStrong@Passw0rd
SeqPassword=YourSeqPassword
CloudName=your-cloudinary-cloud-name
ApiKey=your-cloudinary-api-key
ApiSecret=your-cloudinary-api-secret
```

Then run:

```bash
1. docker compose up -d --build
2. dotnet ef database update -p src/Company_System.Infrastructure -s Company_System.WebApi
3. docker compose down
4. docker compose up
```

## 📌 Project Status

Actively developed. Current focus areas include expanding the admin inventory/modification management tooling and refining the real-time support chat experience.
