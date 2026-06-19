# RestaurantProject
[![.NET Build and Test](https://github.com/Jegor2005/RestaurantProject/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Jegor2005/RestaurantProject/actions/workflows/dotnet.yml)

RestaurantProject is a .NET 8 ASP.NET Core Web API for managing a small restaurant network.

The project supports CRUD operations for restaurants, menus, and dishes. It uses Entity Framework Core with SQLite, DTO-based API contracts, service layer architecture, validation, automatic migrations, seed data, Swagger documentation, HTTP logging, and global error handling with ProblemDetails.

## Technologies

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* Swagger / OpenAPI
* DTOs
* Service Layer
* DataAnnotations validation
* ProblemDetails
* HTTP Logging
* xUnit
* EF Core SQLite in-memory testing

## Main Features

* Manage restaurants
* Manage menus assigned to restaurants
* Manage dishes assigned to menus
* Validate incoming API requests
* Automatically apply EF Core migrations on startup
* Automatically seed initial data
* Explore and test endpoints through Swagger UI
* Filter dishes by category and price range
* Sort dishes by name, category, and price
* Use pagination for dish lists
* Service layer tests for dish filtering, sorting, pagination, and creation

## Domain Model

The current MVP contains three main entities:


Restaurant
   └── Menu
          └── Dishes


A restaurant can have one menu.
A menu belongs to one restaurant.
A menu can contain many dishes.
Each dish belongs to one menu.

## Project Structure

RestaurantNetwork.Api
├── Controllers
│   ├── RestaurantsController.cs
│   ├── MenusController.cs
│   └── DishesController.cs
│
├── Data
│   ├── AppDbContext.cs
│   ├── DbSeeder.cs
│   └── Migrations
│
├── DTO
│   ├── RestaurantDto.cs
│   ├── CreateRestaurantDto.cs
│   ├── UpdateRestaurantDto.cs
│   ├── MenuDto.cs
│   ├── CreateMenuDto.cs
│   ├── UpdateMenuDto.cs
│   ├── DishDto.cs
│   ├── CreateDishDto.cs
│   └── UpdateDishDto.cs
│
├── Services
│   ├── IRestaurantService.cs
│   ├── RestaurantService.cs
│   ├── IMenuService.cs
│   ├── MenuService.cs
│   ├── IDishService.cs
│   └── DishService.cs
│
└── Program.cs
RestaurantNetwork.Api.Tests
└── DishServiceTests.cs

### Domain models are stored in:


RestaurantProject.DataModel

## Tests

The solution contains a test project:

RestaurantNetwork.Api.Tests



## Current tests cover DishService behavior:
* pagination
* category filtering
* price sorting
* dish creation for an existing menu

The tests use SQLite in-memory database to verify EF Core behavior close to the real application database.

## To run tests in Visual Studio:

Test → Run All Tests

#### Or using .NET CLI:

dotnet test



## API Endpoints

### Restaurants


GET    /api/restaurants
GET    /api/restaurants/{id}
POST   /api/restaurants
PUT    /api/restaurants/{id}
DELETE /api/restaurants/{id}


### Menus

GET    /api/menus
GET    /api/menus/{id}
GET    /api/restaurants/{restaurantId}/menu
POST   /api/restaurants/{restaurantId}/menu
PUT    /api/menus/{id}
DELETE /api/menus/{id}


### Dishes


GET    /api/dishes
GET    /api/dishes/{id}
GET    /api/dishes?category=Salad
GET    /api/dishes?minPrice=8&maxPrice=12
GET    /api/dishes?sortBy=price&sortDirection=desc
GET    /api/dishes?pageNumber=1&pageSize=5
GET    /api/menus/{menuId}/dishes
POST   /api/menus/{menuId}/dishes
PUT    /api/dishes/{id}
DELETE /api/dishes/{id}


## Example Requests

### Create Restaurant

```json
{
  "color": "Red",
  "address": "Maribor, Slovenia",
  "rent": 1200
}
```

### Create Menu for Restaurant

```json
{
  "name": "Main Menu",
  "description": "Default restaurant menu"
}
```


### Create Dish for Menu

```json
{
  "name": "Classic Burger",
  "price": 12.5,
  "category": "Main Course",
  "description": "Burger with beef, cheese and sauce"
}
```

## Dish Query Options

The `GET /api/dishes` endpoint supports filtering, sorting, and pagination.

# Filtering

## Filter dishes by category:

http
GET /api/dishes?category=Salad
GET /api/dishes?minPrice=8&maxPrice=12
GET /api/dishes?sortBy=price&sortDirection=desc

## Supported sortBy values:
- name
- category
- price

## Supported sortDirection values:
- asc
- desc

## Pagination
GET /api/dishes?pageNumber=1&pageSize=5

## The response contains:
```json
{
  "items": [],
  "totalCount": 9,
  "pageNumber": 1,
  "pageSize": 5,
  "totalPages": 2
}
```

### Combined Example

GET /api/dishes?category=Main%20Course&minPrice=8&sortBy=price&sortDirection=asc&pageNumber=1&pageSize=3


## How to Run

1. Clone the repository.

2. Open the solution in Visual Studio.

3. Set `RestaurantNetwork.Api` as the startup project.

4. Run the project.

5. Open Swagger UI in the browser.

The application uses SQLite. The database file is created automatically when the project starts.

## Database

The project uses Entity Framework Core with SQLite.

Connection string example:

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=restaurantproject.db"
  }
}
```


Migrations are applied automatically on startup.

Seed data is also inserted automatically if the database is empty.

The seed includes:

* restaurants
* menus
* dishes

## Validation

The API uses DTOs and DataAnnotations validation.

Examples of validation rules:

* required fields
* maximum string length
* valid price range
* valid rent range

Invalid requests return `400 Bad Request`.

## Error Handling

The project uses ASP.NET Core ProblemDetails for global error handling.

Common responses:


- 200 OK
- 201 Created
- 204 No Content
- 400 Bad Request
- 404 Not Found
- 409 Conflict
- 500 Internal Server Error


## Future Improvements

Possible next improvements:
* more service tests
* controller/integration tests
* integration tests
* authentication and authorization
* Docker support
* order management
* staff management
* frontend client

## Current Status

The current version is an MVP-ready backend API.

Implemented:

* Restaurant CRUD
* Menu CRUD
* Dish CRUD
* Restaurant → Menu relationship
* Menu → Dishes relationship
* EF Core migrations
* SQLite database
* seed data
* Swagger documentation
* DTOs
* validation
* service layer
* HTTP logging
* ProblemDetails error handling
