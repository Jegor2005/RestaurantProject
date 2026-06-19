using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDbContext _db;

        public MenuService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<MenuDto>> GetAllAsync()
        {
            return await _db.Menus
                .AsNoTracking()
                .Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    Description = menu.Description,
                    RestaurantId = menu.RestaurantId
                })
                .ToListAsync();
        }

        public async Task<MenuDto?> GetByIdAsync(int id)
        {
            return await _db.Menus
                .AsNoTracking()
                .Where(menu => menu.Id == id)
                .Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    Description = menu.Description,
                    RestaurantId = menu.RestaurantId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MenuDto?> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _db.Menus
                .AsNoTracking()
                .Where(menu => menu.RestaurantId == restaurantId)
                .Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    Description = menu.Description,
                    RestaurantId = menu.RestaurantId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _db.Restaurants.AnyAsync(restaurant => restaurant.Id == restaurantId);
        }

        public async Task<bool> RestaurantHasMenuAsync(int restaurantId)
        {
            return await _db.Menus.AnyAsync(menu => menu.RestaurantId == restaurantId);
        }

        public async Task<MenuDto> CreateForRestaurantAsync(int restaurantId, CreateMenuDto dto)
        {
            var menu = new Menu
            {
                Name = dto.Name,
                Description = dto.Description,
                RestaurantId = restaurantId
            };

            _db.Menus.Add(menu);
            await _db.SaveChangesAsync();

            return new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name,
                Description = menu.Description,
                RestaurantId = menu.RestaurantId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateMenuDto dto)
        {
            var menu = await _db.Menus.FindAsync(id);

            if (menu is null)
            {
                return false;
            }

            menu.Name = dto.Name;
            menu.Description = dto.Description;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menu = await _db.Menus.FindAsync(id);

            if (menu is null)
            {
                return false;
            }

            _db.Menus.Remove(menu);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
