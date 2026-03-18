using Favly.Application.Auth.Commands.Login;
using Favly.Application.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Favly.api.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMessageBus _bus) : ControllerBase
    {
        /// <summary>Autentica o usuário e retorna o token JWT</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<LoginResponse>(command, ct);
            return Ok(result);
        }
    }
}
