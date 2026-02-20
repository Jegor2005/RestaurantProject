using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantProject.DataModel;
namespace RestaurantNetwork.Api.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _db;

        public RestaurantService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<RestaurantDto>> GetAllAsync()
        {
            return await _db.Restaurants.Select(
                r => new RestaurantDto
                {
                    Id = r.Id,
                    Color = r.Color,
                    Address = r.Address,
                    Rent = r.Rent
                })
                .ToListAsync();
        }

        public async Task<RestaurantDto?> GetByIdAsync(int id)
        {
            return await _db.Restaurants
                .Where(r => r.Id == id)
                .Select(r => new RestaurantDto
                {
                    Id = r.Id,
                    Color = r.Color,
                    Address = r.Address,
                    Rent = r.Rent
                })
                .FirstOrDefaultAsync();
        }

        public async Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto)
        {
            var restaurant = new Restaurant
            {
                Color = dto.Color,
                Address = dto.Address,
                Rent = dto.Rent
            };

            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();

            return new RestaurantDto
            {
                Id = restaurant.Id,
                Color = restaurant.Color,
                Address = restaurant.Address,
                Rent = restaurant.Rent
            };
        }
        public async Task<bool> UpdateAsync (int id, UpdateRestaurantDto dto)
        {
            var entity =  await _db.Restaurants.FindAsync(id);
            if (entity is null) return false;
            entity.Color = dto.Color;
            entity.Address = dto.Address;
            entity.Rent = dto.Rent;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Restaurants.FindAsync(id);
            if (entity is null) return false;
            _db.Restaurants.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
