using Microsoft.AspNetCore.Mvc;
using RestaurantNetwork.Api.Data;

namespace RestaurantNetwork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public DemoController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetDemoData()
        {
            var isEnabled = _configuration.GetValue<bool>("EnableDemoReset");

            if (!isEnabled)
            {
                return NotFound();
            }

            await DbSeeder.ResetAsync(_db);

            return NoContent();
        }
    }
}