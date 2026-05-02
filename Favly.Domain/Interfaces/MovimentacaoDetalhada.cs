using Favly.Domain.Common.Enums;

namespace Favly.Domain.Interfaces
{
    public record MovimentacaoDetalhada(
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
        DateTime DataCriacao);
}
