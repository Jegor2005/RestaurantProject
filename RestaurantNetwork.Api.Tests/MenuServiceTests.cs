using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantNetwork.Api.Services;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Tests
{
    public class MenuServiceTests
    {
        [Fact]
        public async Task RestaurantExistsAsync_WithExistingRestaurant_ReturnsTrue()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new MenuService(db);

                var exists = await service.RestaurantExistsAsync(1);

                Assert.True(exists);
            }
        }

        [Fact]
        public async Task RestaurantExistsAsync_WithMissingRestaurant_ReturnsFalse()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new MenuService(db);

                var exists = await service.RestaurantExistsAsync(999);

                Assert.False(exists);
            }
        }

        [Fact]
        public async Task RestaurantHasMenuAsync_WithExistingMenu_ReturnsTrue()
        {
            var (db, connection) = await CreateDbContextAsync(includeMenu: true);

            await using (connection)
            await using (db)
            {
                var service = new MenuService(db);

                var hasMenu = await service.RestaurantHasMenuAsync(1);

                Assert.True(hasMenu);
            }
        }

        [Fact]
        public async Task CreateForRestaurantAsync_WithExistingRestaurant_CreatesMenu()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new MenuService(db);

                var createdMenu = await service.CreateForRestaurantAsync(1, new CreateMenuDto
                {
                    Name = "Main Menu",
                    Description = "Test menu"
                });

                var menuFromDb = await db.Menus.FindAsync(createdMenu.Id);

                Assert.NotNull(menuFromDb);
                Assert.Equal("Main Menu", menuFromDb!.Name);
                Assert.Equal(1, menuFromDb.RestaurantId);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithExistingMenu_DeletesMenu()
        {
            var (db, connection) = await CreateDbContextAsync(includeMenu: true);

            await using (connection)
            await using (db)
            {
                var service = new MenuService(db);

                var deleted = await service.DeleteAsync(1);
                var menuFromDb = await db.Menus.FindAsync(1);

                Assert.True(deleted);
                Assert.Null(menuFromDb);
            }
        }

        private static async Task<(AppDbContext Db, SqliteConnection Connection)> CreateDbContextAsync(bool includeMenu = false)
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
                Address = "Maribor, Slovenia",
                Rent = 1200
            };

            if (includeMenu)
            {
                restaurant.Menu = new Menu
                {
                    Name = "Existing Menu",
                    Description = "Existing menu description"
                };
            }

            db.Restaurants.Add(restaurant);
            await db.SaveChangesAsync();

            return (db, connection);
        }
    }
}