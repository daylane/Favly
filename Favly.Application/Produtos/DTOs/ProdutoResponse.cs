using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Produtos.DTOs
{
    public record ProdutoResponse(
        Guid Id,
        string Nome,
        string? Marca,
        UnidadeMedida Unidade,
        decimal QuantidadeAtual,
        decimal QuantidadeMinima,
        bool EstoqueAbaixoDoMinimo,
        decimal? UltimoPreco,
        Guid? UltimoMercadoId,
        DateTime? UltimaCompra,
        Guid CategoriaId)
    {
        public static ProdutoResponse FromEntity(Produto p) => new(
            p.Id, p.Nome, p.Marca, p.Unidade,
            p.QuantidadeAtual, p.QuantidadeMinima,
            p.EstoqueAbaixoDoMinimo,
            p.UltimoPreco, p.UltimoMercadoId, p.UltimaCompra,
            p.CategoriaId);
    }
}
