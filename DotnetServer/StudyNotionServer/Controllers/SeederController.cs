using Microsoft.AspNetCore.Mvc;
using StudyNotionServer.Seed;

namespace StudyNotionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeederController : ControllerBase
    {
        //This controller is used to insert some dummy data into database so th
        private readonly IServiceProvider _serviceProvider;

        public SeederController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Run seeding for all collections
        [HttpPost("run")]
        public async Task<IActionResult> RunSeeder()
        {
            try
            {
                await DbSeeder.SeedAsync(_serviceProvider);
                return Ok(new { message = "Seeding completed successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Seeding failed", error = ex.Message });
            }
        }
    }
}
