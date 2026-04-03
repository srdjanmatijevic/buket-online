using Microsoft.AspNetCore.Mvc;
using BuketOnline.Api.Models;

namespace BuketOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlowerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var flowers = new List<Flower>
            {
                new Flower { Id = 1, Name = "Ruza", ImageUrl = "rose.png", Price = 150 },
                new Flower { Id = 2, Name = "Tulipan", ImageUrl = "tulip.png", Price = 120 },
                new Flower { Id = 3, Name = "Ljiljan", ImageUrl = "lily.png", Price = 200 }
            };

            return Ok(flowers);
        }
    }
}