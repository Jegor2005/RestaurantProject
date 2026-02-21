# RestaurantProject (ASP.NET Core Web API)

Backend API for managing restaurants (CRUD) with SQLite + Entity Framework Core.

## Tech stack
- .NET 8
- ASP.NET Core Web API
- EF Core
- SQLite
- Swagger / OpenAPI

## How to run
1. Open solution in Visual Studio 2022
2. Set `RestaurantNetwork.Api` as Startup Project
3. Apply database migrations:
   - Tools -> NuGet Package Manager -> Package Manager Console
   - Select Default project: `RestaurantNetwork.Api`
   - Run:
     ```
     Update-Database
     ```
4. Press F5 and open Swagger:
   - `https://localhost:<port>/swagger`

## API endpoints
Base URL: `/api/Restaurants`

- `GET /api/Restaurants` - list restaurants
- `GET /api/Restaurants/{id}` - get by id
- `POST /api/Restaurants` - create
- `PUT /api/Restaurants/{id}` - update
- `DELETE /api/Restaurants/{id}` - delete

## Notes
- SQLite database files are ignored by git (`*.db`, `*.db-wal`, `*.db-shm`)
- DTO validation is enabled via DataAnnotations
- Global exception handling returns ProblemDetails