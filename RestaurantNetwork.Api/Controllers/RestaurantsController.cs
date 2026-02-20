using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantNetwork.Api.Data;
using RestaurantNetwork.Api.DTO;
using RestaurantNetwork.Api.Services;
using RestaurantProject.DataModel;
namespace RestaurantNetwork.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantsController(IRestaurantService service)
        {
            _service = service;
        }

        // GET: api/restaurants
        [HttpGet]
        public async Task<ActionResult<List<RestaurantDto>>> GetAll() => Ok(await _service.GetAllAsync());

        // GET: api/restaurants/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var item=await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/restaurants
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> Create(CreateRestaurantDto dto)
        {
            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }

}
