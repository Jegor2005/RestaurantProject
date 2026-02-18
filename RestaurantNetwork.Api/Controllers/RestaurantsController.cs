using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantNetwork.Api.Data;
using RestaurantProject.DataModel;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<List<Restaurant>>> GetAll()
        {
            var items = await _db.Restaurants.ToListAsync();
            return Ok(items);
        }

        // GET: api/restaurants/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Restaurant>> GetById(int id)
        {
            var item = await _db.Restaurants.FindAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST: api/restaurants
        [HttpPost]
        public async Task<ActionResult<Restaurant>> Create([FromBody] Restaurant restaurant)
        {
            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant);
        }
    }

}
