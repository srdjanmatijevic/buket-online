using Microsoft.AspNetCore.Mvc;

namespace BuketOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Radi backend 🚀");
        }
    }
}