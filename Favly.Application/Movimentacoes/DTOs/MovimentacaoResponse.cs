using Favly.Domain.Common.Enums;
using Favly.Domain.Interfaces;

namespace Favly.Application.Movimentacoes.DTOs
{
    public record MovimentacaoResponse(
        Guid Id,
        Guid ProdutoId,
        string NomeProduto,
        string UnidadeSigla,
        Guid MembroId,
        string NomeMembro,
        Guid? MercadoId,
        string? NomeMercado,
        TipoMovimentacao Tipo,
        decimal Quantidade,
        decimal? PrecoUnitario,
        decimal? ValorTotal,
        string? Observacao,
        DateTime DataCriacao)
    {
        public static MovimentacaoResponse FromDetalhada(MovimentacaoDetalhada m) => new(
            m.Id,
            m.ProdutoId, m.NomeProduto, m.UnidadeSigla,
            m.MembroId, m.NomeMembro,
            m.MercadoId, m.NomeMercado,
            m.Tipo, m.Quantidade,
            m.PrecoUnitario, m.ValorTotal,
            m.Observacao, m.DataCriacao);
    }
}
