using RestaurantNetwork.Api.DTO;
namespace RestaurantNetwork.Api.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDto>> GetAllAsync();
        Task<RestaurantDto?> GetByIdAsync(int id);
        Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto);
        Task<bool> UpdateAsync(int id, UpdateRestaurantDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
