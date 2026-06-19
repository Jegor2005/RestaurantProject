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

        public async Task<PagedResultDto<DishDto>> GetAllAsync(DishQueryDto query)
        {
            var dishesQuery = _db.Dishes
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var category = query.Category.ToLower();

                dishesQuery = dishesQuery
                    .Where(dish => dish.Category.ToLower() == category);
            }

            if (query.MinPrice.HasValue)
            {
                dishesQuery = dishesQuery
                    .Where(dish => dish.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                dishesQuery = dishesQuery
                    .Where(dish => dish.Price <= query.MaxPrice.Value);
            }

            var sortBy = query.SortBy.ToLower();
            var sortDirection = query.SortDirection.ToLower();

            dishesQuery = (sortBy, sortDirection) switch
            {
                ("price", "desc") => dishesQuery.OrderByDescending(dish => dish.Price),
                ("price", _) => dishesQuery.OrderBy(dish => dish.Price),

                ("category", "desc") => dishesQuery.OrderByDescending(dish => dish.Category),
                ("category", _) => dishesQuery.OrderBy(dish => dish.Category),

                ("name", "desc") => dishesQuery.OrderByDescending(dish => dish.Name),
                ("name", _) => dishesQuery.OrderBy(dish => dish.Name),

                ("id", "desc") => dishesQuery.OrderByDescending(dish => dish.Id),
                _ => dishesQuery.OrderBy(dish => dish.Id)
            };

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 10 : query.PageSize;
            pageSize = pageSize > 50 ? 50 : pageSize;

            var totalCount = await dishesQuery.CountAsync();

            var items = await dishesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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

            return new PagedResultDto<DishDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
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