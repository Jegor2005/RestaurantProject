using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;

namespace RestaurantNetwork.Api.Tests
{
    public class ApiIntegrationTests : IClassFixture<RestaurantApiFactory>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(RestaurantApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetRestaurants_ReturnsSeededRestaurants()
        {
            var restaurants = await _client.GetFromJsonAsync<List<RestaurantDto>>("/api/restaurants");

            Assert.NotNull(restaurants);
            Assert.Equal(3, restaurants.Count);
        }

        [Fact]
        public async Task GetDishes_WithPagination_ReturnsPagedResult()
        {
            var result = await _client.GetFromJsonAsync<PagedResultDto<DishDto>>(
                "/api/dishes?pageNumber=1&pageSize=5");

            Assert.NotNull(result);
            Assert.Equal(9, result.TotalCount);
            Assert.Equal(5, result.Items.Count());
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(5, result.PageSize);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        public async Task GetDish_WithMissingId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/dishes/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    public class RestaurantApiFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.RemoveAll<AppDbContext>();

                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(_connection));
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _connection?.Dispose();
            }
        }
    }
}