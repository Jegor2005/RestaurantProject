using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantProject.DataModel;

namespace RestaurantNetwork.Api.Services
{
    public class DishService : IDishService
    {
        private readonly AppDbContext _db;

        public DishService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DishDto>> GetAllAsync()
        {
            return await _db.Dishes
                .AsNoTracking()
                .Select(dish => new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Category = dish.Category,
                    Description = dish.Description,
                    MenuId = dish.MenuId
                })
                .ToListAsync();
        }

        public async Task<DishDto?> GetByIdAsync(int id)
        {
            return await _db.Dishes
                .AsNoTracking()
                .Where(dish => dish.Id == id)
                .Select(dish => new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Category = dish.Category,
                    Description = dish.Description,
                    MenuId = dish.MenuId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DishDto>> GetByMenuIdAsync(int menuId)
        {
            return await _db.Dishes
                .AsNoTracking()
                .Where(dish => dish.MenuId == menuId)
                .Select(dish => new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Category = dish.Category,
                    Description = dish.Description,
                    MenuId = dish.MenuId
                })
                .ToListAsync();
        }

        public async Task<bool> MenuExistsAsync(int menuId)
        {
            return await _db.Menus.AnyAsync(menu => menu.Id == menuId);
        }

        public async Task<DishDto> CreateForMenuAsync(int menuId, CreateDishDto dto)
        {
            var dish = new Dish
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = dto.Category,
                Description = dto.Description,
                MenuId = menuId
            };

            _db.Dishes.Add(dish);
            await _db.SaveChangesAsync();

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Category = dish.Category,
                Description = dish.Description,
                MenuId = dish.MenuId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDishDto dto)
        {
            var dish = await _db.Dishes.FindAsync(id);

            if (dish is null)
            {
                return false;
            }

            dish.Name = dto.Name;
            dish.Price = dto.Price;
            dish.Category = dto.Category;
            dish.Description = dto.Description;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dish = await _db.Dishes.FindAsync(id);

            if (dish is null)
            {
                return false;
            }

            _db.Dishes.Remove(dish);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}