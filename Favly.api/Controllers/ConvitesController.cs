using Favly.Application.Convites.Commands.AceitarConvite;
using Favly.Application.Convites.Commands.CriarConvite;
using Favly.Application.Convites.Commands.RegistrarEAceitarConvite;
using Favly.Application.Convites.DTOs;
using Favly.Application.Convites.Queries.ListarConvites;
using Favly.Application.Convites.Queries.ObterConvite;
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

        // ── Gerenciamento de convites (membros do grupo) ──────────────────────

        /// <summary>
        /// Lista os convites enviados para o grupo. Requer ser membro.
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
        /// Cria um convite por e-mail e envia o link ao convidado. Requer ser membro.
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

        // ── Aceitação — usuário JÁ CADASTRADO (precisa estar logado) ─────────

        /// <summary>
        /// Aceita um convite para um usuário já cadastrado.
        /// O e-mail do token JWT deve coincidir com o e-mail do convite.
        /// </summary>
        [HttpPost("api/convites/{codigo}/aceitar")]
        [ProducesResponseType(typeof(GrupoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Aceitar(string codigo, [FromBody] AceitarConviteRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<GrupoResponse>(
                new AceitarConviteCommand(UsuarioId, codigo, request.Apelido), ct);
            return Ok(result);
        }

        // ── Aceitação — usuário NOVO (sem auth) ───────────────────────────────

        /// <summary>
        /// Consulta os dados de um convite pelo código. Endpoint público —
        /// usado pelo frontend para exibir a tela de aceite antes de logar.
        /// </summary>
        [HttpGet("api/convites/{codigo}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ConvitePublicoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterConvite(string codigo, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<ConvitePublicoResponse>(
                new ObterConvitePorCodigoQuery(codigo), ct);
            return Ok(result);
        }

        /// <summary>
        /// Registra um novo usuário e o adiciona ao grupo em uma única operação.
        /// Endpoint público — chamado quando o convidado ainda não tem conta.
        /// Retorna um JWT para que o frontend logue o usuário automaticamente.
        /// </summary>
        [HttpPost("api/convites/{codigo}/registrar-e-aceitar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegistrarEAceitarResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegistrarEAceitar(
            string codigo, [FromBody] RegistrarEAceitarRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<RegistrarEAceitarResponse>(
                new RegistrarEAceitarConviteCommand(
                    codigo, request.Nome, request.Senha, request.Apelido, request.Avatar), ct);
            return Created($"api/convites/{codigo}", result);
        }
    }
}
