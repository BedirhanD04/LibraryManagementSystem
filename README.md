# Library Management System API

A RESTful backend API for managing a library's books, authors, members, and loans — built with **ASP.NET Core** and a clean **N-tier (layered) architecture**.

## Architecture

The project is split into four independent layers, each with a single responsibility and a strict, one-directional dependency rule (a layer only knows the layer directly beneath it):

```
API            -> Controllers, HTTP concerns, Swagger
Business       -> DTOs, services, business rules (validation, stock/loan limits)
DataAccess     -> EF Core DbContext, Repository pattern, migrations
Entities       -> Plain domain models (no dependencies)
```

This separation means the persistence technology (currently SQLite via EF Core) can be swapped without touching business logic, and business rules are enforced in exactly one place regardless of how many clients call the API.

## Tech Stack

- **.NET 9 / ASP.NET Core Web API**
- **Entity Framework Core** (Code-First, SQLite provider)
- **Swashbuckle (Swagger UI)** for interactive API documentation
- **Repository pattern** with a generic `IRepository<T>` and a specialized `ILoanRepository`
- **DataAnnotations** for request validation
- Centralized exception handling via `IExceptionHandler`

## Features

- CRUD operations for Authors, Books, and Members
- Loan management with real business rules:
  - A book can only be borrowed if it has available copies
  - A member can have at most 3 active loans at a time
  - Loans have a 14-day due date, tracked via `IsReturned` / `ReturnDate`
  - Overdue loan reporting
- Input validation on all write endpoints (`[Required]`, `[StringLength]`, `[EmailAddress]`, `[Range]`)
- Global exception handler — internal errors never leak stack traces to the client
- Auto-generated interactive API docs (Swagger UI)

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Setup

```bash
git clone <your-repo-url>
cd LibraryManagementSystem

# Apply EF Core migrations to create the SQLite database
cd src/LibraryManagementSystem.API
dotnet ef database update --project ../LibraryManagementSystem.DataAccess --startup-project .

# Run the API
dotnet run
```

Open `http://localhost:<port>/swagger` in your browser to explore and test the API interactively.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/api/authors` | List all authors |
| GET | `/api/authors/{id}` | Get an author by id |
| POST | `/api/authors` | Create an author |
| GET | `/api/books` | List all books |
| GET | `/api/books/{id}` | Get a book by id |
| POST | `/api/books` | Create a book |
| GET | `/api/members` | List all members |
| GET | `/api/members/{id}` | Get a member by id |
| POST | `/api/members` | Create a member |
| POST | `/api/loans?memberId={id}&bookId={id}` | Create a loan |
| POST | `/api/loans/{id}/return` | Return a loan |
| GET | `/api/loans/overdue` | List overdue loans |

## Project Structure

```
src/
├── LibraryManagementSystem.Entities/     # Domain models: Author, Book, Member, Loan
├── LibraryManagementSystem.DataAccess/   # DbContext, Repositories, Migrations
├── LibraryManagementSystem.Business/     # DTOs, Services (business rules)
└── LibraryManagementSystem.API/          # Controllers, Program.cs, Swagger
```

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
