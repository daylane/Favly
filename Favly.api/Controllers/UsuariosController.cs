using Favly.Application.Usuarios.Commands.AtivarUsuario;
using Favly.Application.Usuarios.Commands.AtualizarUsuario;
using Favly.Application.Usuarios.Commands.CriarUsuario;
using Favly.Application.Usuarios.Commands.DesativarUsuario;
using Favly.Application.Usuarios.Commands.ReenviarCodigoAtivacao;
using Favly.Application.Usuarios.DTOs;
using Favly.Application.Usuarios.Queries.ObterUsuarioPorId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController(IMessageBus _bus) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioCommand command, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<UsuarioResponse>(command, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }

        [HttpPost("{email}/ativar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Ativar(string email, [FromBody] AtivarContaRequest request, CancellationToken ct)
        {
            await _bus.InvokeAsync(new AtivarUsuarioCommand(email, request.Codigo), ct);
            return NoContent();
        }
        /// <summary>Reenvia o código de ativação para o e-mail</summary>
        [HttpPost("reenviar-ativacao")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReenviarCodigoAtivacao(
            [FromBody] ReenviarCodigoAtivacaoCommand command,
            CancellationToken ct)
        {
            await _bus.InvokeAsync(command, ct);
            return NoContent();
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<UsuarioResponse>(new ObterUsuarioPorIdQuery(id), ct);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarContaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<UsuarioResponse>(
                new AtualizarUsuarioCommand(id, request.Nome, request.Avatar), ct);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Desativar(Guid id, CancellationToken ct)
        {
            await _bus.InvokeAsync(new DesativarUsuarioCommand(id), ct);
            return NoContent();
        }
    }
}
