using Microsoft.AspNetCore.Mvc;
using RestaurantNetwork.Api.DTO;
using RestaurantNetwork.Api.Services;

namespace RestaurantNetwork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<DishDto>>> GetAll([FromQuery] DishQueryDto query)
        {
            var dishes = await _dishService.GetAllAsync(query);

            return Ok(dishes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DishDto>> GetById(int id)
        {
            var dish = await _dishService.GetByIdAsync(id);

            if (dish is null)
            {
                return NotFound();
            }

            return Ok(dish);
        }

        [HttpGet("/api/menus/{menuId:int}/dishes")]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetByMenuId(int menuId)
        {
            var menuExists = await _dishService.MenuExistsAsync(menuId);

            if (!menuExists)
            {
                return NotFound($"Menu with id {menuId} was not found.");
            }

            var dishes = await _dishService.GetByMenuIdAsync(menuId);

            return Ok(dishes);
        }

        [HttpPost("/api/menus/{menuId:int}/dishes")]
        public async Task<ActionResult<DishDto>> CreateForMenu(
            int menuId,
            CreateDishDto dto)
        {
            var menuExists = await _dishService.MenuExistsAsync(menuId);

            if (!menuExists)
            {
                return NotFound($"Menu with id {menuId} was not found.");
            }

            var createdDish = await _dishService.CreateForMenuAsync(menuId, dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDish.Id },
                createdDish);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateDishDto dto)
        {
            var updated = await _dishService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _dishService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}