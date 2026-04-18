using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.DTOs
{
    public record MovimentacaoResponse(
        Guid Id,
        Guid ProdutoId,
        Guid MembroId,
        Guid? MercadoId,
        TipoMovimentacao Tipo,
        decimal Quantidade,
        decimal? Preco,
        string? Observacao,
        DateTime DataCriacao)
    {
        public static MovimentacaoResponse FromEntity(Movimentacao m) => new(
            m.Id, m.ProdutoId, m.MembroId, m.MercadoId,
            m.Tipo, m.Quantidade, m.Preco, m.Observacao, m.DataCriacao);
    }
}
