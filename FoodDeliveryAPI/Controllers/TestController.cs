using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/protected")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]  // Requires authentication
        [HttpGet("test")]
        public IActionResult GetProtectedData()
        {
            return Ok(new { message = "You have accessed a protected route!" });
        }
    }
}
