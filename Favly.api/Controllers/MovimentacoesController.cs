using Favly.Application.Movimentacoes.Commands.RegistrarEntrada;
using Favly.Application.Movimentacoes.Commands.RegistrarSaida;
using Favly.Application.Movimentacoes.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Favly.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grupos/{grupoId:guid}/movimentacoes")]
    public class MovimentacoesController(IMessageBus _bus) : ControllerBase
    {
        [HttpPost("entrada")]
        [ProducesResponseType(typeof(MovimentacaoResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegistrarEntrada(
            Guid grupoId, [FromBody] RegistrarEntradaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MovimentacaoResponse>(
                new RegistrarEntradaCommand(
                    grupoId, request.ProdutoId, request.MembroId,
                    request.Quantidade, request.Preco, request.MercadoId, request.Observacao), ct);
            return Created($"api/grupos/{grupoId}/movimentacoes/{result.Id}", result);
        }

        [HttpPost("saida")]
        [ProducesResponseType(typeof(MovimentacaoResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegistrarSaida(
            Guid grupoId, [FromBody] RegistrarSaidaRequest request, CancellationToken ct)
        {
            var result = await _bus.InvokeAsync<MovimentacaoResponse>(
                new RegistrarSaidaCommand(
                    grupoId, request.ProdutoId, request.MembroId,
                    request.Quantidade, request.Observacao), ct);
            return Created($"api/grupos/{grupoId}/movimentacoes/{result.Id}", result);
        }
    }

}
