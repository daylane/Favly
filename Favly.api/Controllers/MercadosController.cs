using Favly.Application.Mercados.Commands.AtualizarMercado;
using Favly.Application.Mercados.Commands.CriarMercado;
using Favly.Application.Mercados.Commands.ListarMercados;
using Favly.Application.Mercados.Commands.RemoverMercado;
using Favly.Application.Mercados.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grupos/{grupoId:guid}/mercados")]
    public class MercadosController(IMessageBus _bus) : ControllerBase
    {
        private Guid UsuarioId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Jti)!);

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MercadoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(Guid grupoId, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<MercadoResponse>>(
                new ListarMercadosQuery(grupoId, UsuarioId), ct);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MercadoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar(Guid grupoId, [FromBody] CriarMercadoRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MercadoResponse>(
                new CriarMercadoCommand(grupoId, UsuarioId, request.Nome, request.Endereco), ct);
            return CreatedAtAction(nameof(Listar), new { grupoId }, result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(MercadoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(Guid grupoId, Guid id, [FromBody] AtualizarMercadoRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MercadoResponse>(
                new AtualizarMercadoCommand(grupoId, UsuarioId, id, request.Nome, request.Endereco), ct);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(Guid grupoId, Guid id, CancellationToken ct)
        {
            await _bus.InvokeAsync(new RemoverMercadoCommand(grupoId, UsuarioId, id), ct);
            return NoContent();
        }
    }
}
