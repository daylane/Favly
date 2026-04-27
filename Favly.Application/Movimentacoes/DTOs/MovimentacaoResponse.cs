using Favly.Domain.Common.Enums;
using Favly.Domain.Interfaces;

namespace Favly.Application.Movimentacoes.DTOs
{
    public record MovimentacaoResponse(
        Guid Id,
        Guid ProdutoId,
        string NomeProduto,
        Guid MembroId,
        string NomeMembro,
        Guid? MercadoId,
        string? NomeMercado,
        TipoMovimentacao Tipo,
        decimal Quantidade,
        decimal? Preco,
        string? Observacao,
        DateTime DataCriacao)
    {
        public static MovimentacaoResponse FromDetalhada(MovimentacaoDetalhada m) => new(
            m.Id, m.ProdutoId, m.NomeProduto,
            m.MembroId, m.NomeMembro,
            m.MercadoId, m.NomeMercado,
            m.Tipo, m.Quantidade, m.Preco, m.Observacao, m.DataCriacao);
    }
}
