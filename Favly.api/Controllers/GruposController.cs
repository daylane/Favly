using Favly.Application.Grupos.Commands.EntrarPorCodigo;
using Favly.Application.Grupos.DTOs;
using Favly.Application.Grupos.Queries.ListarGrupos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grupos")]
    public class GruposController(IMessageBus _bus) : ControllerBase
    {
        private Guid UsuarioId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        /// <summary>
        /// Lista todos os grupos dos quais o usuário logado é membro.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GrupoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<GrupoResponse>>(
                new ListarGruposQuery(UsuarioId), ct);
            return Ok(result);
        }

        /// <summary>
        /// Entra em um grupo usando o código de 8 caracteres exibido na tela do grupo.
        /// Qualquer usuário autenticado pode usar este endpoint.
        /// </summary>
        [HttpPost("entrar")]
        [ProducesResponseType(typeof(GrupoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Entrar([FromBody] EntrarPorCodigoRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<GrupoResponse>(
                new EntrarPorCodigoCommand(UsuarioId, request.Codigo, request.Apelido), ct);
            return Ok(result);
        }
    }
}
