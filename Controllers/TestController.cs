using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthAPI.Controllers
{
    [ApiController]
    [Route("test")]
    [Authorize(Roles = "Admin")]
    public class TestController : ControllerBase
    {

        [HttpGet(Name = "TestResource")]
        public async Task<ActionResult<string>> Get()
        {
            return Ok("Test done with success.");
        }
    }
}
