using RestaurantNetwork.Api.DTO;
namespace RestaurantNetwork.Api.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDto>> GetAllAsync();
        Task<RestaurantDto?> GetByIdAsync(int id);
        Task<RestaurantDto> CreateAsync(CreateRestaurantDto dto);

    }
}
