using RestaurantNetwork.Api.DTO;

namespace RestaurantNetwork.Api.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuDto>> GetAllAsync();

        Task<MenuDto?> GetByIdAsync(int id);

        Task<MenuDto?> GetByRestaurantIdAsync(int restaurantId);

        Task<bool> RestaurantExistsAsync(int restaurantId);

        Task<bool> RestaurantHasMenuAsync(int restaurantId);

        Task<MenuDto> CreateForRestaurantAsync(int restaurantId, CreateMenuDto dto);

        Task<bool> UpdateAsync(int id, UpdateMenuDto dto);

        Task<bool> DeleteAsync(int id);
    }
}