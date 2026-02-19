using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantNetwork.Api.Data;
using RestaurantProject.DataModel;
using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.DTO;
namespace RestaurantNetwork.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RestaurantsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/restaurants
        [HttpGet]
        public async Task<ActionResult<List<RestaurantDto>>> GetAll()
        {
            var items = await _db.Restaurants.Select(r => new RestaurantDto
            {
                Id= r.Id,
                Color= r.Color,
                Address= r.Address,
                Rent= r.Rent
            }).ToListAsync();
            return Ok(items);
        }

        // GET: api/restaurants/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var item = await _db.Restaurants.Where(r => r.Id == id).Select(r => new RestaurantDto
            {
                Id = r.Id,
                Color = r.Color,
                Address = r.Address,
                Rent = r.Rent
            }).FirstOrDefaultAsync();
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST: api/restaurants
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> Create(CreateRestaurantDto dto)
        {
            var restaurant = new Restaurant
            {
                Color = dto.Color,
                Address = dto.Address,
                Rent = dto.Rent
            };
            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();

            var result = new RestaurantDto
            {
                Id = restaurant.Id,
                Color = restaurant.Color,
                Address = restaurant.Address,
                Rent = restaurant.Rent
            };

            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, result);
        }
    }

}
