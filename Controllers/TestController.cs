using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthAPI.Controllers
{
    [Route("test")]
    //[ApiController]
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
