using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantNetwork.Api.Services;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Tests
{
    public class RestaurantServiceTests
    {
        [Fact]
        public async Task CreateAsync_WithValidDto_CreatesRestaurant()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new RestaurantService(db);

                var createdRestaurant = await service.CreateAsync(new CreateRestaurantDto
                {
                    Color = "Blue",
                    Address = "Ljubljana, Slovenia",
                    Rent = 1500
                });

                var restaurantFromDb = await db.Restaurants.FindAsync(createdRestaurant.Id);

                Assert.NotNull(restaurantFromDb);
                Assert.Equal("Blue", restaurantFromDb!.Color);
                Assert.Equal("Ljubljana, Slovenia", restaurantFromDb.Address);
                Assert.Equal(1500, restaurantFromDb.Rent);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithExistingRestaurant_UpdatesRestaurant()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new RestaurantService(db);

                var updated = await service.UpdateAsync(1, new UpdateRestaurantDto
                {
                    Color = "Green",
                    Address = "Celje, Slovenia",
                    Rent = 900
                });

                var restaurantFromDb = await db.Restaurants.FindAsync(1);

                Assert.True(updated);
                Assert.NotNull(restaurantFromDb);
                Assert.Equal("Green", restaurantFromDb!.Color);
                Assert.Equal("Celje, Slovenia", restaurantFromDb.Address);
                Assert.Equal(900, restaurantFromDb.Rent);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithMissingRestaurant_ReturnsFalse()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new RestaurantService(db);

                var updated = await service.UpdateAsync(999, new UpdateRestaurantDto
                {
                    Color = "Green",
                    Address = "Unknown",
                    Rent = 1000
                });

                Assert.False(updated);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithExistingRestaurant_DeletesRestaurant()
        {
            var (db, connection) = await CreateDbContextAsync();

            await using (connection)
            await using (db)
            {
                var service = new RestaurantService(db);

                var deleted = await service.DeleteAsync(1);
                var restaurantFromDb = await db.Restaurants.FindAsync(1);

                Assert.True(deleted);
                Assert.Null(restaurantFromDb);
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

            db.Restaurants.Add(new Restaurant
            {
                Color = "Red",
                Address = "Maribor, Slovenia",
                Rent = 1200
            });

            await db.SaveChangesAsync();

            return (db, connection);
        }
    }
}