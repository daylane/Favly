using Favly.Application.Categorias.Commands.AtualizarCategoria;
using Favly.Application.Categorias.Commands.CriarCategoria;
using Favly.Application.Categorias.Commands.RemoverCategoria;
using Favly.Application.Categorias.DTOs;
using Favly.Application.Categorias.Queries.ListarCategorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grupos/{grupoId:guid}/categorias")]
    public class CategoriasController(IMessageBus _bus) : ControllerBase
    {
        private Guid UsuarioId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoriaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(Guid grupoId, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<CategoriaResponse>>(
                new ListarCategoriasQuery(grupoId, UsuarioId), ct);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Criar(Guid grupoId, [FromBody] CriarCategoriaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<CategoriaResponse>(
                new CriarCategoriaCommand(grupoId, UsuarioId, request.Nome, request.Icone ?? "📦"), ct);
            return Created($"api/grupos/{grupoId}/categorias/{result.Id}", result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Atualizar(Guid grupoId, Guid id, [FromBody] AtualizarCategoriaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<CategoriaResponse>(
                new AtualizarCategoriaCommand(grupoId, UsuarioId, id, request.Nome, request.Icone), ct);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Remover(Guid grupoId, Guid id, CancellationToken ct)
        {
            await _bus.InvokeAsync(new RemoverCategoriaCommand(grupoId, UsuarioId, id), ct);
            return NoContent();
        }
    }
}
