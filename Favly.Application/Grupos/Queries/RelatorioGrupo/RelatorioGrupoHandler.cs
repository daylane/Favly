using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Queries.RelatorioGrupo
{
    public class RelatorioGrupoHandler
    {
        public static async Task<RelatorioGrupoResponse> Handle(
            RelatorioGrupoQuery query,
            IGrupoRepository grupoRepository,
            IMovimentacaoRepository movimentacaoRepository,
            ICategoriaRepository categoriaRepository,
            IMercadoRepository mercadoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var gastosBrutos = await movimentacaoRepository.ObterGastosPorCategoriaAsync(query.GrupoId, ct);
            var categorias = (await categoriaRepository.ListarPorGrupoAsync(query.GrupoId, ct))
                .ToDictionary(c => c.Id);

            var gastosPorCategoria = gastosBrutos
                .Where(g => categorias.ContainsKey(g.CategoriaId))
                .Select(g => new GastoPorCategoriaItem(
                    g.CategoriaId,
                    categorias[g.CategoriaId].Nome,
                    categorias[g.CategoriaId].Icone,
                    g.TotalGasto))
                .OrderByDescending(x => x.TotalGasto)
                .ToList();

            var rankingBruto = await movimentacaoRepository.ObterRankingMercadosAsync(query.GrupoId, ct);
            var mercados = (await mercadoRepository.ListarPorGrupoAsync(query.GrupoId, ct))
                .ToDictionary(m => m.Id);

            var rankingMercados = rankingBruto
                .Where(r => mercados.ContainsKey(r.MercadoId))
                .Select(r => new RankingMercadoItem(
                    r.MercadoId,
                    mercados[r.MercadoId].Nome,
                    r.TotalGasto,
                    r.TotalCompras))
                .ToList();

            return new RelatorioGrupoResponse(gastosPorCategoria, rankingMercados);
        }
    }
}
