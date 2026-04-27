using Favly.Application.Movimentacoes.Commands.RegistrarEntrada;
using Favly.Application.Movimentacoes.Commands.RegistrarSaida;
using Favly.Application.Movimentacoes.DTOs;
using Favly.Application.Movimentacoes.Queries.ListarMovimentacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grupos/{grupoId:guid}/movimentacoes")]
    public class MovimentacoesController(IMessageBus _bus) : ControllerBase
    {
        private Guid UsuarioId => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovimentacaoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar(
            Guid grupoId,
            [FromQuery] Guid? produtoId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 20,
            CancellationToken ct = default)
        {
            var result = await _bus.InvokeAsync<IEnumerable<MovimentacaoResponse>>(
                new ListarMovimentacoesQuery(grupoId, UsuarioId, produtoId, pagina, tamanho), ct);
            return Ok(result);
        }

        [HttpPost("entrada")]
        [ProducesResponseType(typeof(MovimentacaoResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegistrarEntrada(
            Guid grupoId, [FromBody] RegistrarEntradaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MovimentacaoResponse>(
                new RegistrarEntradaCommand(
                    grupoId, request.ProdutoId, UsuarioId,
                    request.Quantidade, request.Preco, request.MercadoId, request.Observacao), ct);

            return CreatedAtAction(nameof(RegistrarEntrada), new { grupoId }, result);
        }

        [HttpPost("saida")]
        [ProducesResponseType(typeof(MovimentacaoResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegistrarSaida(
            Guid grupoId, [FromBody] RegistrarSaidaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MovimentacaoResponse>(
                new RegistrarSaidaCommand(
                    grupoId, request.ProdutoId, UsuarioId,
                    request.Quantidade, request.Observacao), ct);
            return Created($"api/grupos/{grupoId}/movimentacoes/{result.Id}", result);
        }
    }
}
