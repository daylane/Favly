using Favly.Application.Produtos.Commands.AtualizarProduto;
using Favly.Application.Produtos.Commands.CriarProduto;
using Favly.Application.Produtos.Commands.RemoverProduto;
using Favly.Application.Produtos.DTOs;
using Favly.Application.Produtos.Queries.ListarProdutos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grupos/{grupoId:guid}/produtos")]
    public class ProdutosController(IMessageBus _bus) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(Guid grupoId, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<ProdutoResponse>>(
                new ListarProdutosQuery(grupoId), ct);
            return Ok(result);
        }

        [HttpGet("estoque-baixo")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarEstoqueBaixo(Guid grupoId, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<IEnumerable<ProdutoResponse>>(
                new ListarEstoqueBaixoQuery(grupoId), ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<ProdutoResponse>(
                new ObterProdutoPorIdQuery(id), ct);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Criar(Guid grupoId, [FromBody] CriarProdutoCommand command, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<ProdutoResponse>(command, ct);
            return Created($"api/grupos/{grupoId}/produtos/{result.Id}", result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarProdutoRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<ProdutoResponse>(
                new AtualizarProdutoCommand(id, request.Nome, request.Marca, request.CategoriaId, request.QuantidadeMinima), ct);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Remover(Guid id, CancellationToken ct)
        {
            await _bus.InvokeAsync(new RemoverProdutoCommand(id), ct);
            return NoContent();
        }
    }

}
