using Favly.Application.Auth.Commands.EsquecerSenha;
using Favly.Application.Auth.Commands.Login;
using Favly.Application.Auth.Commands.RedefinirSenha;
using Favly.Application.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMessageBus _bus, IConfiguration _config) : ControllerBase
    {
        private const string CookieName = "favly_token";

        /// <summary>Autentica o usuário, emite cookie HttpOnly e retorna dados do usuário (sem o token)</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<LoginResponse>(command, ct);

            Response.Cookies.Append(CookieName, result.Token, new CookieOptions
            {
                HttpOnly = true,                          // JS não consegue ler
                Secure = !IsDevEnvironment(),             // HTTPS em produção
                SameSite = SameSiteMode.Strict,
                Expires = result.Expiracao
            });

            // Retorna tudo exceto o token — ele fica apenas no cookie
            return Ok(result with { Token = string.Empty });
        }

        /// <summary>Encerra a sessão removendo o cookie de autenticação</summary>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(CookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = !IsDevEnvironment(),
                SameSite = SameSiteMode.Strict
            });

            return NoContent();
        }

        /// <summary>Solicita redefinição de senha</summary>
        [HttpPost("esqueci-senha")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> EsqueciSenha(
            [FromBody] EsqueciSenhaCommand command, CancellationToken ct)
        {
            await _bus.InvokeAsync(command, ct);
            return NoContent();
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

        private bool IsDevEnvironment() =>
            _config["ASPNETCORE_ENVIRONMENT"] == "Development";
    }
}
