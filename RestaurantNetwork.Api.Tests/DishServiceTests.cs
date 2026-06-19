using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantNetwork.Api.Services;
using RestaurantProject.DataModel;
using System;

namespace RestaurantNetwork.Api.Tests
{
    public class DishServiceTests
    {
        [Fact]
        public async Task GetAllAsync_WithPagination_ReturnsExpectedPage()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new DishService(db);

                var result = await service.GetAllAsync(new DishQueryDto
                {
                    PageNumber = 1,
                    PageSize = 2
                });

                Assert.Equal(4, result.TotalCount);
                Assert.Equal(2, result.Items.Count());
                Assert.Equal(2, result.TotalPages);
            }
        }

        [Fact]
        public async Task GetAllAsync_WithCategoryFilter_ReturnsOnlyMatchingCategory()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new DishService(db);

                var result = await service.GetAllAsync(new DishQueryDto
                {
                    Category = "Salad"
                });

                Assert.Equal(2, result.TotalCount);
                Assert.All(result.Items, dish => Assert.Equal("Salad", dish.Category));
            }
        }

        [Fact]
        public async Task GetAllAsync_WithPriceSortingDescending_ReturnsMostExpensiveFirst()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new DishService(db);

                var result = await service.GetAllAsync(new DishQueryDto
                {
                    SortBy = "price",
                    SortDirection = "desc"
                });

                var dishes = result.Items.ToList();

                Assert.Equal("Classic Burger", dishes[0].Name);
                Assert.True(dishes[0].Price >= dishes[1].Price);
            }
        }

        [Fact]
        public async Task CreateForMenuAsync_WithExistingMenu_CreatesDish()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new DishService(db);

                var createdDish = await service.CreateForMenuAsync(1, new CreateDishDto
                {
                    Name = "Tomato Soup",
                    Price = 6.50m,
                    Category = "Soup",
                    Description = "Fresh tomato soup"
                });

                var dishFromDb = await db.Dishes.FindAsync(createdDish.Id);

                Assert.NotNull(dishFromDb);
                Assert.Equal("Tomato Soup", dishFromDb!.Name);
                Assert.Equal(1, dishFromDb.MenuId);
            }
        }

        private static async Task<(AppDbContext Db, SqliteConnection Connection)> CreateDbContextAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var db = new AppDbContext(options);

            await db.Database.EnsureCreatedAsync();

            var restaurant = new Restaurant
            {
                Color = "Red",
                Address = "Test Address",
                Rent = 1000,
                Menu = new Menu
                {
                    Name = "Test Menu",
                    Description = "Test menu description",
                    Dishes = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Classic Burger",
                            Price = 12.50m,
                            Category = "Main Course",
                            Description = "Burger description"
                        },
                        new Dish
                        {
                            Name = "Caesar Salad",
                            Price = 8.90m,
                            Category = "Salad",
                            Description = "Salad description"
                        },
                        new Dish
                        {
                            Name = "Greek Salad",
                            Price = 7.50m,
                            Category = "Salad",
                            Description = "Greek salad description"
                        },
                        new Dish
                        {
                            Name = "Chocolate Cake",
                            Price = 5.50m,
                            Category = "Dessert",
                            Description = "Cake description"
                        }
                    }
                }
            };

            db.Restaurants.Add(restaurant);
            await db.SaveChangesAsync();

            return (db, connection);
        }
    }
}