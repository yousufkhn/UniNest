using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniNest.Api.Services.Auth;
using UniNest.API.Models.User;

namespace UniNest.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Authorized");
        }

        [HttpGet("token")]
        public IActionResult Token(
            [FromServices] IJwtService jwtService
        )
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName="testUser",
                Email= "test@gmail.com"
            };

            var token = jwtService.GenerateToken(user);

            return Ok(token);
        }
    }
}
