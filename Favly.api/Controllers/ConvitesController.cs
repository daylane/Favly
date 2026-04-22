using Favly.Application.Convites.Commands.AceitarConvite;
using Favly.Application.Convites.Commands.CriarConvite;
using Favly.Application.Convites.DTOs;
using Favly.Application.Convites.Queries.ListarConvites;
using Favly.Application.Grupos.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    public class ConvitesController(IMessageBus _bus) : ControllerBase
    {
        private Guid UsuarioId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        /// <summary>
        /// Lista os convites enviados para o grupo (requer ser membro).
        /// </summary>
        [HttpGet("api/grupos/{grupoId:guid}/convites")]
        [ProducesResponseType(typeof(IEnumerable<ConviteResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(Guid grupoId, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<ConviteResponse>>(
                new ListarConvitesQuery(grupoId, UsuarioId), ct);
            return Ok(result);
        }

        /// <summary>
        /// Cria um convite por e-mail para o grupo (requer ser membro).
        /// </summary>
        [HttpPost("api/grupos/{grupoId:guid}/convites")]
        [ProducesResponseType(typeof(ConviteResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Criar(Guid grupoId, [FromBody] CriarConviteRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<ConviteResponse>(
                new CriarConviteCommand(grupoId, UsuarioId, request.EmailConvidado), ct);
            return Created($"api/grupos/{grupoId}/convites/{result.Id}", result);
        }

        /// <summary>
        /// Aceita um convite por e-mail usando o código recebido.
        /// O e-mail do usuário logado deve corresponder ao e-mail do convite.
        /// </summary>
        [HttpPost("api/convites/aceitar")]
        [ProducesResponseType(typeof(GrupoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Aceitar([FromBody] AceitarConviteRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<GrupoResponse>(
                new AceitarConviteCommand(UsuarioId, request.Codigo, request.Apelido), ct);
            return Ok(result);
        }
    }
}
