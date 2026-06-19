using Microsoft.AspNetCore.Mvc;
using RestaurantNetwork.Api.DTO;
using RestaurantNetwork.Api.Services;

namespace RestaurantNetwork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetAll()
        {
            var menus = await _menuService.GetAllAsync();

            return Ok(menus);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuDto>> GetById(int id)
        {
            var menu = await _menuService.GetByIdAsync(id);

            if (menu is null)
            {
                return NotFound();
            }

            return Ok(menu);
        }

        [HttpGet("/api/restaurants/{restaurantId:int}/menu")]
        public async Task<ActionResult<MenuDto>> GetByRestaurantId(int restaurantId)
        {
            var restaurantExists = await _menuService.RestaurantExistsAsync(restaurantId);

            if (!restaurantExists)
            {
                return NotFound($"Restaurant with id {restaurantId} was not found.");
            }

            var menu = await _menuService.GetByRestaurantIdAsync(restaurantId);

            if (menu is null)
            {
                return NotFound($"Restaurant with id {restaurantId} does not have a menu.");
            }

            return Ok(menu);
        }

        [HttpPost("/api/restaurants/{restaurantId:int}/menu")]
        public async Task<ActionResult<MenuDto>> CreateForRestaurant(
            int restaurantId,
            CreateMenuDto dto)
        {
            var restaurantExists = await _menuService.RestaurantExistsAsync(restaurantId);

            if (!restaurantExists)
            {
                return NotFound($"Restaurant with id {restaurantId} was not found.");
            }

            var restaurantHasMenu = await _menuService.RestaurantHasMenuAsync(restaurantId);

            if (restaurantHasMenu)
            {
                return Conflict($"Restaurant with id {restaurantId} already has a menu.");
            }

            var createdMenu = await _menuService.CreateForRestaurantAsync(restaurantId, dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdMenu.Id },
                createdMenu);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateMenuDto dto)
        {
            var updated = await _menuService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _menuService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}