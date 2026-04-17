using Favly.Application.Auth.Commands.EsquecerSenha;
using Favly.Application.Auth.Commands.Login;
using Favly.Application.Auth.Commands.RedefinirSenha;
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
        /// <summary>Solicita redefinição de senha</summary>
        [HttpPost("esqueci-senha")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> EsqueciSenha(
            [FromBody] EsqueciSenhaCommand command, CancellationToken ct)
        {
            await _bus.InvokeAsync(command, ct);
            return NoContent(); // sempre 204 — não revela se email existe
        }

        /// <summary>Redefine a senha com o token recebido por email</summary>
        [HttpPost("redefinir-senha")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RedefinirSenha(
            [FromBody] RedefinirSenhaCommand command, CancellationToken ct)
        {
            await _bus.InvokeAsync(command, ct);
            return NoContent();
        }
    }
}
