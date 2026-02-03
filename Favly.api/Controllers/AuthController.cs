using Favly.Application.DTOs;
using Favly.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Favly.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthAppService _authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            if (!response.Success)
                return Unauthorized(new { message = response.Message });

            return Ok(new { token = response.Token, message = response.Message });
        }
    }
}
