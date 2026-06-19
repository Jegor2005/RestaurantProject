using RestaurantNetwork.Api.DTO;

namespace RestaurantNetwork.Api.Services
{
    public interface IDishService
    {
        Task<PagedResultDto<DishDto>> GetAllAsync(DishQueryDto query);

        Task<DishDto?> GetByIdAsync(int id);

        Task<IEnumerable<DishDto>> GetByMenuIdAsync(int menuId);

        Task<bool> MenuExistsAsync(int menuId);

        Task<DishDto> CreateForMenuAsync(int menuId, CreateDishDto dto);

        Task<bool> UpdateAsync(int id, UpdateDishDto dto);

        Task<bool> DeleteAsync(int id);
    }
}