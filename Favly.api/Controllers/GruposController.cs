using Favly.Application.Grupos.Commands.AlterarPapelMembro;
using Favly.Application.Grupos.Commands.AtualizarGrupo;
using Favly.Application.Grupos.Commands.CriarGrupo;
using Favly.Application.Grupos.Commands.EntrarPorCodigo;
using Favly.Application.Grupos.Commands.ExpulsarMembro;
using Favly.Application.Grupos.Commands.SairDoGrupo;
using Favly.Application.Grupos.DTOs;
using Favly.Application.Grupos.Queries.ListarGrupos;
using Favly.Application.Grupos.Queries.ListarMembros;
using Favly.Application.Grupos.Queries.ObterGrupo;
using Favly.Application.Grupos.Queries.RelatorioGrupo;
using Favly.Application.Grupos.Queries.ResumoGrupo;
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GrupoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<GrupoResponse>>(
                new ListarGruposQuery(UsuarioId), ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GrupoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<GrupoResponse>(
                new ObterGrupoQuery(id, UsuarioId), ct);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GrupoResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Criar([FromBody] CriarGrupoRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<GrupoResponse>(
                new CriarGrupoCommand(UsuarioId, request.Nome, request.Avatar, request.Apelido), ct);
            return Created($"api/grupos/{result.Id}", result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(GrupoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarGrupoRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<GrupoResponse>(
                new AtualizarGrupoCommand(id, UsuarioId, request.Nome, request.Avatar), ct);
            return Ok(result);
        }

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

        [HttpDelete("{id:guid}/sair")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Sair(Guid id, CancellationToken ct)
        {
            await _bus.InvokeAsync(new SairDoGrupoCommand(id, UsuarioId), ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}/membros/{membroId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExpulsarMembro(Guid id, Guid membroId, CancellationToken ct)
        {
            await _bus.InvokeAsync(new ExpulsarMembroCommand(id, UsuarioId, membroId), ct);
            return NoContent();
        }

        [HttpPatch("{id:guid}/membros/{membroId:guid}/papel")]
        [ProducesResponseType(typeof(MembroResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarPapel(Guid id, Guid membroId,
            [FromBody] AlterarPapelMembroRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MembroResponse>(
                new AlterarPapelMembroCommand(id, UsuarioId, membroId, request.NovoPapel), ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}/membros")]
        [ProducesResponseType(typeof(IEnumerable<MembroResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarMembros(Guid id, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<MembroResponse>>(
                new ListarMembrosQuery(id, UsuarioId), ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}/resumo")]
        [ProducesResponseType(typeof(ResumoGrupoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumo(Guid id, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<ResumoGrupoResponse>(
                new ResumoGrupoQuery(id, UsuarioId), ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}/relatorio")]
        [ProducesResponseType(typeof(RelatorioGrupoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterRelatorio(Guid id, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<RelatorioGrupoResponse>(
                new RelatorioGrupoQuery(id, UsuarioId), ct);
            return Ok(result);
        }
    }
}
