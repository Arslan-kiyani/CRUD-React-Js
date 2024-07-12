using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            
            if (login.UserName == "test" && login.Password == "password")
            {
                var tokenString = _jwtService.GenerateToken(login.UserName);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized("Token not Generated");
        }
    }
}
